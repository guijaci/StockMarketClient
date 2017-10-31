using StockMarketClient.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace StockMarketClient.Services
{
    class StockMarketService : WebService
    {
        protected string CrateBuyStockOrderPath   { get => "/stock/order/buy"; }
        protected string CrateSellStockOrderPath  { get => "/stock/order/sell"; }
        protected string ListStockOrderPath       { get => "/stock/order/list"; }

        public async Task<BuyStockOrder> CreateBuyStockOrderRequest(BuyStockOrder stockOrder)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(CrateBuyStockOrderPath, stockOrder);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<BuyStockOrder>();
        }

        public async Task<SellStockOrder> CreateSellStockOrderRequest(SellStockOrder stockOrder)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(CrateSellStockOrderPath, stockOrder);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<SellStockOrder>();
        }
    }
}
