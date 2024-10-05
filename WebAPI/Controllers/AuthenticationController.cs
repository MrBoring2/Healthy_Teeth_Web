using Data;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Security.Claims;
using WebApi.Services;
using WebAPI.Identity;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly HealthyTeethDbContext _context;
        public AuthenticationController(HealthyTeethDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity userClaims = null;
                var user = await _context.Employees.Include(p => p.Account).ThenInclude(p => p.Role).FirstOrDefaultAsync(x => x.Account.Login == model.Login);
                Console.WriteLine($"Ялвяется ли null: {user.Id} {user.FirstName} {user.Account.Login} {user.Account.Role.Title}");
                if (user == null || !PasswordHasher.VerifyPasswordHash(model.Password, user.Account.PasswordHash, user.Account.PasswordSalt))
                {
                    return BadRequest("Неверное имя пользователя или пароль");
                }
                else
                {

                    userClaims = ClaimsExtentions.BuildClaimsForUser(user);
                }

                var token = JwtTokenGenerator.GenerateToken(userClaims);
                var refreshToken = GenerateRefreshToken();

                await _tokenService.SaveRefreshToken(user.Id, refreshToken);

                //HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token.access_token);
                return Ok(new LoginResponse
                {
                    Message = "Авторизация успешна",
                    JwtBearer = token.access_token,
                    RefreshJwtBearer = refreshToken,
                    Login = token.user_name,
                    Success = true
                });
            }
            else return BadRequest(new LoginResponse { Message = "Пользователь/пароль не найдены.", Success = false });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Токен обновления истёк");
            }

            try
            {
                Console.WriteLine("Тоен обновления: " + request.RefreshToken);
                var login = await _tokenService.RetrieveLoginByRefreshToken(request.RefreshToken);
                Console.WriteLine("Пользователь: " + login);
                if (string.IsNullOrEmpty(login))
                {
                    return Unauthorized("Недействительный токен обновления");
                }

                var user = await _context.Employees.Include(p => p.Account).ThenInclude(p => p.Role).FirstOrDefaultAsync(x => x.Account.Login.Equals(login));

                var userClaims = ClaimsExtentions.BuildClaimsForUser(user);
                if (user == null)
                {
                    return Unauthorized("Недействительный пользователь");
                }

                var token = JwtTokenGenerator.GenerateToken(userClaims).access_token;
                var newRefreshToken = GenerateRefreshToken();
                await _tokenService.SaveRefreshToken(user.Id, newRefreshToken);
                return Ok(new AuthResponse { Token = token, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("Токен обновления обязателен");
            }
            try
            {
                var result = await _tokenService.RevokeRefreshToken(request.RefreshToken);
                if (!result)
                {
                    return NotFound("Токен обновления не найдён");
                }
                else
                {
                    return Ok("Токен обновления успешно отменён");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(LogoutRequest logout)
        {
            Console.WriteLine($"Выход: {logout.Login}");
            var errorMessage = "";
            var acc = await _context.Accounts.Include(p => p.EmployeeRefreshToken).FirstOrDefaultAsync(p => p.Login.Equals(logout.Login));
            acc.EmployeeRefreshToken.RefreshToken = null;
            acc.EmployeeRefreshToken.RefreshTokenExpiryDate = null;
            await _context.SaveChangesAsync();
            //await _signInManager.SignOutAsync();


            var result = new LogoutResponse()
            {
                Success = true,
                ErrorMessage = errorMessage
            };

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];  // Prepare a buffer to hold the random bytes.
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);  // Fill the buffer with cryptographically strong random bytes.
                return Convert.ToBase64String(randomNumber);  // Convert the bytes to a Base64 string and return.
            }
        }
    }
}
