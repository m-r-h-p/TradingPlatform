using MediatR;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Core.Entities;
using TradingPlatform.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TradingPlatform.Application.Queries
{
    public class GetTradesQueryHandler : IRequestHandler<GetTradesQuery, List<Trade>>
    {
        private readonly ApplicationDbContext _context;

        public GetTradesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trade>> Handle(GetTradesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Trades
                .Where(t => t.UserId == request.UserId)
                .ToListAsync(cancellationToken);
        }
    }
}