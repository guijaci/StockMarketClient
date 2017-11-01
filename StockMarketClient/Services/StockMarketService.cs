using StockMarketClient.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace StockMarketClient.Services
{
    public class StockMarketService : WebService
    {
        protected string CrateBuyStockOrderPath     { get => "/stock/order/buy"; }
        protected string CrateSellStockOrderPath    { get => "/stock/order/sell"; }
        protected string ListStockOrderPath         { get => "/stock/order/list"; }
        protected string RetrieveEventsPath         { get => "/stock/events"; }

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

        public async Task<List<StockEventArgs>> RetrieveEventsRequest(Stockholder subscriber)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(RetrieveEventsPath, subscriber);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<StockEventArgs>>();
        }
    }
}
