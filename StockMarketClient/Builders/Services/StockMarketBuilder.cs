using StockMarketClient.Services;
using System.Net.Http;

namespace StockMarketClient.Builders.Services
{
    static class StockMarketBuilder
    {
        public static StockMarketService WithClientService(this StockMarketService stockMarket, HttpClient clientService)
        {
            stockMarket.ClientService = clientService;
            return stockMarket;
        }
    }
}
