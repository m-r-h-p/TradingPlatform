using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Application.Services.Interfaces;
using TradingPlatform.Core.DTOs;

namespace TradingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        public AuthController(UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized();
            }

            var token = await _jwtService.GenerateTokenAsync(user.Id, user.Email);
            return Ok(token);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto registerDto)
        {
            var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = await _jwtService.GenerateTokenAsync(user.Id, user.Email);
            return Ok(token);
        }
    }
}
