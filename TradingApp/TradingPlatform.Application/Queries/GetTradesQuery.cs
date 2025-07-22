using MediatR;
using TradingPlatform.Core.Entities;
using System.Collections.Generic;

namespace TradingPlatform.Application.Queries
{
    public class GetTradesQuery : IRequest<List<Trade>>
    {
        public Guid UserId { get; set; }
    }
}