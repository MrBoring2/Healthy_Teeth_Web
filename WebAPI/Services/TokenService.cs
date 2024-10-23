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

        public async Task SaveRefreshToken(int userId, string token)
        {
            var user = _context.Accounts.Include(p => p.EmployeeRefreshToken).FirstOrDefault(p => p.EmployeeId == userId);
            if (user.EmployeeRefreshToken == null)
                user.EmployeeRefreshToken = new Entities.EmployeeRefreshToken();
            user.EmployeeRefreshToken.RefreshToken = token;
            user.EmployeeRefreshToken.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
        }

        public async Task<string> RetrieveLoginByRefreshToken(string refreshToken)
        {
            var tokenRecord = await _context.Accounts.Include(p => p.EmployeeRefreshToken).FirstOrDefaultAsync(p => p.EmployeeRefreshToken.RefreshToken.Equals(refreshToken) &&
                                        p.EmployeeRefreshToken.RefreshTokenExpiryDate > DateTime.UtcNow);
            return tokenRecord?.Login;
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            Account tokenRecord = new Account();
            try
            {
                tokenRecord = await _context.Accounts.Include(p => p.EmployeeRefreshToken).FirstOrDefaultAsync(p => p.EmployeeRefreshToken.RefreshToken.Equals(refreshToken));
                if (tokenRecord != null)
                {
                    _context.EmployeeRefreshTokens.Remove(tokenRecord.EmployeeRefreshToken);
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
