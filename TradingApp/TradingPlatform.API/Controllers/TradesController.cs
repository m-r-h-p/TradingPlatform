using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Application.Queries;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TradingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TradesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TradesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrades()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetTradesQuery { UserId = Guid.Parse(userId) };
            var trades = await _mediator.Send(query);
            return Ok(trades);
        }
    }
}