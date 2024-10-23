using Data;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Serilog;
using Shared.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI.Identity;
using WebAPI.Services;
using WebAPI.SignalR;

try
{
   
    var builder = WebApplication.CreateBuilder(args);
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)          
        .Enrich.FromLogContext()
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Запуск сервера");


    builder.Services.AddControllers()
        .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    builder.Services.AddDbContext<HealthyTeethDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"))); ;

    builder.Services.AddScoped<TokenService>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSignalR();

    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
           new[] { "application/octet-stream" });
    });
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/healthy_teeth_hub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    builder.Services.AddAuthorization(options =>
    {
        var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
        defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
        options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
    });
    builder.Services.AddCors(policy =>
    {
        policy.AddPolicy("_myAllowSpecificOrigins", builder =>
        builder.WithOrigins("https://localhost:8084", "http://localhost:8083", "http://localhost:5107")
            .WithMethods("GET", "POST", "PUT")
            .AllowAnyHeader()
            .AllowCredentials());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "Пользователь {UserName} ({ClientIp}:{ClientPort}) перешёл по {RequestMethod} на {RequestPath} статус {StatusCode} за {Elapsed:0.0000} сек";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                
                   
                string userName = "";
                if(httpContext.Request.Method == "OPTIONS")
                {
                    userName = "CORS Preflight";
                }
                else if (httpContext.User?.Claims.Count() == 0)
                {
                    userName = "Неавторизованный пользователь";
                }
                else
                {
                    userName = httpContext.User?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
                }
                diagnosticContext.Set("UserName", userName);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("ClientIp", httpContext.Connection?.RemoteIpAddress);
                diagnosticContext.Set("ClientPort", httpContext.Connection?.RemotePort);
            };
        });
    }
    else if (app.Environment.IsProduction())
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "Пользователь {UserName} ({ClientIp}:{ClientPort}) перешёл по {RequestMethod} на {RequestPath} со статусом {StatusCode} за {Elapsed:0.0000} сек";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                if (httpContext.Request.Method == "OPTIONS")
                    return;

                string userName = "";
                if (httpContext.User?.Claims.Count() == 0)
                {
                    userName = "Неавторизованный пользователь";
                }
                else
                {
                    userName = httpContext.User?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
                }
                diagnosticContext.Set("UserName", userName);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("ClientIp", httpContext.Connection?.RemoteIpAddress);
                diagnosticContext.Set("ClientPort", httpContext.Connection?.RemotePort);
            };
        });
    }
    app.UseCors("_myAllowSpecificOrigins");
    //app.UseHttpsRedirection();

    app.UseResponseCompression();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHub<MainHub>("/healthy_teeth_hub");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Неожиданное завершение работы сервера");
}
finally
{
    await Log.CloseAndFlushAsync();
}