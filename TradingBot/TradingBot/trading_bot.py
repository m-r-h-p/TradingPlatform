import ccxt
from api_client import ApiClient
import pandas as pd
import time

class TradingBot:
    def __init__(self, api_client, exchange_id, api_key, api_secret):
        self.api_client = api_client
        self.exchange = getattr(ccxt, exchange_id)({
            'apiKey': api_key,
            'secret': api_secret,
        })

    def fetch_market_data(self, symbol, timeframe ):
        ohlcv = self.exchange.fetch_ohlcv(symbol, timeframe)
        df = pd.DataFrame(ohlcv, columns=['timestamp', 'open', 'high', 'low', 'close', 'volume'])
        df['timestamp'] = pd.to_datetime(df['timestamp'], unit='ms')
        return df

    def trading_strategy(self, symbol, timeframe):
        df = self.fetch_market_data(symbol, timeframe)
        # Simple Moving Average Strategy
        df['sma20'] = df['close'].rolling(window=20).mean()
        df['sma50'] = df['close'].rolling(window=50).mean()

        if df['sma20'].iloc[-1] > df['sma50'].iloc[-1]:
            # Buy signal
            print(f"Buy {symbol}")
            # Place order on exchange
            # self.exchange.create_market_buy_order(symbol, amount)
        elif df['sma20'].iloc[-1] < df['sma50'].iloc[-1]:
            print(f"Sell {symbol}")
            # Place sell order
            # Save trade to database
            # Call api_client to save trade

    def run(self, symbol, timeframe):
        while True:
            self.trading_strategy(symbol, timeframe)
            time.sleep(60)  # Wait 1 minute

if __name__ == "__main__":
    api_client = ApiClient("https://localhost:5001", "user@example.com", "password123")
    api_client.login()
    bot = TradingBot(api_client, "binance", "your-api-key", "your-api-secret")
    bot.run("BTC/USDT", "1h")