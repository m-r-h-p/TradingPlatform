using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.Core.DTOs;

namespace TradingPlatform.Application.Services.Interfaces
{
    public interface IJwtService
    {
        Task<TokenDto> GenerateTokenAsync(string userId, string email);
        Task<string> GenerateRefreshTokenAsync(string userId);
        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
    }
}
