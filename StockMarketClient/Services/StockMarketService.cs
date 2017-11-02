using StockMarketClient.Models;
using StockMarketClient.Services.Utils;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;

namespace StockMarketClient.Services
{
    public class StockMarketService : WebService
    {
        protected Uri CrateBuyStockOrderPath    { get; } = new Uri("/stock/order/buy",          UriKind.Relative);
        protected Uri CrateSellStockOrderPath   { get; } = new Uri("/stock/order/sell",         UriKind.Relative);
        protected Uri ListStockOrderPath        { get; } = new Uri("/stock/order/list",         UriKind.Relative);
        protected Uri StockEventSubscribePath   { get; } = new Uri("/stock/event/subscribe",    UriKind.Relative);
        protected Uri RetrieveEventsPath        { get; } = new Uri("/stock/events",             UriKind.Relative);

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

        public async Task<Stockholder> StockEventsSubscribeRequest(Stockholder subscriber, string enterprise, StockEventArgs.EStockEventType eventType, bool? isBuying, bool? isSelling)
        {
            IDictionary<string, string> queryMap = new Dictionary<string, string>();
            queryMap.Add("eventType", eventType.ToString());
            queryMap.Add("enterprise", enterprise);
            if (isBuying != null)
                queryMap.Add("isBuying", isBuying.ToString());
            if (isSelling != null)
                queryMap.Add("isSelling", isSelling.ToString());
            FormUrlEncodedContent query = new FormUrlEncodedContent(queryMap);
            string requestUri = string.Format("{0}?{1}", StockEventSubscribePath, await query.ReadAsStringAsync());
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(requestUri, subscriber);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Stockholder>();
        }

        public async Task<List<StockEventArgs>> RetrieveEventsRequest(Stockholder subscriber)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(RetrieveEventsPath, subscriber);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<StockEventArgs>>();
        }
    }
}
