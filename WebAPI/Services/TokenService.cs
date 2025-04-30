using Data;
using Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;

namespace WebAPI.Services
{
    public class TokenService
    {
        public readonly HealthyTeethDbContext _context;
        private readonly ILogger<TokenService> _logger;

        public TokenService(HealthyTeethDbContext context, ILogger<TokenService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveRefreshToken(int userId, string userAgent, string token)
        {
            var user = _context.Accounts.Include(p => p.EmployeeRefreshTokens).FirstOrDefault(p => p.EmployeeId == userId);
            if (user.EmployeeRefreshTokens == null)
                user.EmployeeRefreshTokens = new List<EmployeeRefreshToken>();

            var empToken = await _context.EmployeeRefreshTokens?.FirstOrDefaultAsync(p => p.EmployeeId == userId && p.UserAgent.Equals(userAgent));

            if (empToken != null)
            {
                empToken.RefreshToken = token;
                empToken.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            }
            else
            {
                var newToken = new EmployeeRefreshToken();
                newToken.RefreshToken = token;
                newToken.UserAgent = userAgent;
                newToken.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
                user.EmployeeRefreshTokens.Add(newToken);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> RetrieveLoginByRefreshToken(string refreshToken, string userAgent)
        {
            var tokenRecord = await _context.Accounts.Include(p => p.EmployeeRefreshTokens).FirstOrDefaultAsync(p => p.EmployeeRefreshTokens.Any(d => d.RefreshToken.Equals(refreshToken) &&
                                        d.RefreshTokenExpiryDate > DateTime.UtcNow && d.UserAgent.Equals(userAgent)));
            return tokenRecord?.Login;
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            Account tokenRecord = new Account();
            try
            {
                tokenRecord = await _context.Accounts.Include(p => p.EmployeeRefreshTokens)
                                                     .FirstOrDefaultAsync(p => p.EmployeeRefreshTokens.Any(p => p.RefreshToken.Equals(refreshToken)));
                if (tokenRecord != null)
                {
                    _context.EmployeeRefreshTokens.Remove(tokenRecord.EmployeeRefreshTokens
                                                  .FirstOrDefault(p => p.RefreshToken.Equals(refreshToken)));
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("У пользователя {0} нет токена обновления", tokenRecord.Login);
            }


            return false;
        }
    }
}
