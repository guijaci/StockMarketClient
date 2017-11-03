using StockMarketClient.Services;
using System.Net.Http;

namespace StockMarketClient.Builders.Services
{
    /// <summary>
    /// Classe extende <see cref="StockMarketService"/> para permitir construção encadeada de objeto
    /// </summary>
    static class StockMarketBuilder
    {
        public static StockMarketService WithClientService(this StockMarketService stockMarket, HttpClient clientService)
        {
            stockMarket.ClientService = clientService;
            return stockMarket;
        }
    }
}
