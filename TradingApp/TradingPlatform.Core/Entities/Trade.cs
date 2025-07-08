using System;

namespace TradingPlatform.Core.Entities
{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Symbol { get; set; } // e.g., BTC/USD
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime TradeDate { get; set; }
        public User User { get; set; }
    }
}