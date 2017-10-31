using StockMarketClient.Models;

namespace StockMarketClient.Builders.Models
{
    static class StockOrderBuilder
    {
        public static StockOrder WithOrderPlacer(this StockOrder stockOrder, Stockholder orderPlacer)
        {
            stockOrder.OrderPlacer = orderPlacer;
            return stockOrder;
        }

        public static StockOrder WithStocks(this StockOrder stockOrder, Stocks stocks)
        {
            stockOrder.Stocks = stocks;
            return stockOrder;
        }
    }
}
