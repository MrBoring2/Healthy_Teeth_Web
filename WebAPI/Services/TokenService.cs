using Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class TokenService
    {
        public readonly HealthyTeethDbContext _context;

        public TokenService(HealthyTeethDbContext context)
        {
            _context = context;
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
            var tokenRecord = await _context.Accounts.Include(p => p.EmployeeRefreshToken).FirstOrDefaultAsync(p => p.EmployeeRefreshToken.RefreshToken.Equals(refreshToken));
            if (tokenRecord != null)
            {
                tokenRecord.EmployeeRefreshToken.RefreshToken = null;
                tokenRecord.EmployeeRefreshToken.RefreshTokenExpiryDate = null;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
