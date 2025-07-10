using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TradingPlatform.Application.Services.Interfaces;
using TradingPlatform.Core.DTOs;

namespace TradingPlatform.Application.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

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
            // Store refresh token in database (simplified for now)
            return Guid.NewGuid().ToString();
        }


        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            return true;
        }
    }
}
