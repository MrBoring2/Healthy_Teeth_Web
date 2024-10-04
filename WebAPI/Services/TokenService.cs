﻿using Data;
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
            var user = _context.Accounts.FirstOrDefault(p => p.EmploeeId == userId);
            user.RefreshToken = token;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
        }

        public async Task<string> RetrieveLoginByRefreshToken(string refreshToken)
        {
            var tokenRecord = await _context.Accounts.FirstOrDefaultAsync(p => p.RefreshToken.Equals(refreshToken) &&
                                        p.RefreshTokenExpiryDate > DateTime.UtcNow);
            return tokenRecord?.Login;
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var tokenRecord = await _context.Accounts.FirstOrDefaultAsync(p=>p.RefreshToken.Equals(refreshToken));
            if(tokenRecord != null)
            {
                tokenRecord.RefreshToken = null;
                tokenRecord.RefreshTokenExpiryDate = null;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
