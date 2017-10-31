using StockMarketClient.Models;
using System.Threading.Tasks;
using System;

namespace StockMarketClient.Services
{
    class TransactionRoomFacade
    {
        private StockMarketService _stockMarketService;

        internal StockMarketService StockMarketService { get => _stockMarketService; set => _stockMarketService = value; }

        public BuyStockOrder AddBuyStockOrder(BuyStockOrder stockOrder) {
            var request = Task.Run(async () => await StockMarketService.CreateBuyStockOrderRequest(stockOrder));
            request.Wait();
            return request.Result;
        }

        public async Task<BuyStockOrder> AddBuyStockOrderAsync(BuyStockOrder stockOrder)
        {
            return await StockMarketService.CreateBuyStockOrderRequest(stockOrder);
        }

        public SellStockOrder AddSellStockOrder(SellStockOrder stockOrder)
        {
            var request = Task.Run(async () => await StockMarketService.CreateSellStockOrderRequest(stockOrder));
            request.Wait();
            return request.Result;
        }

        public async Task<SellStockOrder> AddSellStockOrderAsync(SellStockOrder stockOrder)
        {
            return await StockMarketService.CreateSellStockOrderRequest(stockOrder);
        }
    }
}
