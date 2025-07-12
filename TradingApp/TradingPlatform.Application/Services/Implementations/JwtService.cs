using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingPlatform.Application.Services.Interfaces;
using TradingPlatform.Core.DTOs;
using TradingPlatform.Core.Entities;
using TradingPlatform.Infrastructure.Data;

namespace TradingPlatform.Application.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<TokenDto> GenerateTokenAsync(string userId, string email)
        {
            var claims = new[]
              {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = await GenerateRefreshTokenAsync(userId);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = token.ValidTo
            };
        }
        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // ریفرش توکن 7 روزه
                IsRevoked = false
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }
        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken);

            if (token == null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }
    }
}
