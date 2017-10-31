namespace StockMarketClient.Models.Builders
{
    static class StocksBuilder
    {
        public static Stocks WithPrice(this Stocks stocks, double price)
        {
            stocks.Price = price;
            return stocks;
        }

        public static Stocks WithEnterprise(this Stocks stocks, string enterprise)
        {
            stocks.Enterprise = enterprise;
            return stocks;
        }

        public static Stocks WithQuantity(this Stocks stocks, long quantity)
        {
            stocks.Quantity = quantity;
            return stocks;
        }
    }
}
