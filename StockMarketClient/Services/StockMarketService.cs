using StockMarketClient.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace StockMarketClient.Services
{
    /// <summary>
    /// Serviço para realizar chamadas remotas específicas para Backend do Mercado de Ações
    /// </summary>
    public class StockMarketService : WebService
    {
        /// <summary> Caminho para a API para adicionar ordem de compra de ações </summary>
        protected Uri AddBuyStockOrderPath      { get; } = new Uri("/stock/order/buy",          UriKind.Relative);
        /// <summary> Caminho para a API para adicionar ordem de venda de ações </summary>
        protected Uri AddSellStockOrderPath     { get; } = new Uri("/stock/order/sell",         UriKind.Relative);
        /// <summary> Caminho para a API para listar ordems de operação sobre ações </summary>
        protected Uri ListStockOrderPath        { get; } = new Uri("/stock/order/list",         UriKind.Relative);
        /// <summary> Caminho para a API para inscrição de eventos de ações </summary>
        protected Uri SubscribeToStockEventPath   { get; } = new Uri("/stock/event/subscribe",    UriKind.Relative);
        /// <summary> Caminho para a API para recuperar eventos de ações </summary>
        protected Uri RetrieveEventsPath        { get; } = new Uri("/stock/events",             UriKind.Relative);

        /// <summary>
        /// Realiza uma requisição de adição de ordem de compra de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de compra de ações para ser adicionada </param>
        /// <returns> A ordem de compra de ações adicionada </returns>
        public async Task<BuyStockOrder> AddBuyStockOrderRequest(BuyStockOrder stockOrder)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(AddBuyStockOrderPath, stockOrder);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<BuyStockOrder>();
        }

        /// <summary>
        /// Realiza uma requisição de adição de ordem de venda de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de venda de ações para ser adicionada </param>
        /// <returns> A ordem de venda de ações adicionada </returns>
        public async Task<SellStockOrder> AddSellStockOrderRequest(SellStockOrder stockOrder)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(AddSellStockOrderPath, stockOrder);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<SellStockOrder>();
        }

        /// <summary>
        /// Realiza uma requisição de inscrição de eventos de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante </param>
        /// <param name="enterprise"> Empresa da qual se deseja receber eventos </param>
        /// <param name="eventType"> Tipo de evento desejado </param>
        /// <param name="isBuying"> Se é um evento relacionado à compra de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param>
        /// <param name="isSelling"> Se é um evento relacionado à venda de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param> 
        /// <returns> O assinante dos eventos </returns>
        public async Task<Stockholder> SubscribeToStockEventRequest(Stockholder subscriber, string enterprise, StockEventArgs.EStockEventType eventType, bool? isBuying, bool? isSelling)
        {
            //Montagem da query de URI
            IDictionary<string, string> queryMap = new Dictionary<string, string>();
            queryMap.Add("eventType", eventType.ToString());
            queryMap.Add("enterprise", enterprise);
            if (isBuying != null)
                queryMap.Add("isBuying", isBuying.ToString());
            if (isSelling != null)
                queryMap.Add("isSelling", isSelling.ToString());
            FormUrlEncodedContent query = new FormUrlEncodedContent(queryMap);
            //Concatenação da query de URI com o caminho da API
            string requestUri = string.Format("{0}?{1}", SubscribeToStockEventPath, await query.ReadAsStringAsync());
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(requestUri, subscriber);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Stockholder>();
        }

        /// <summary>
        /// Realiza uma requisição dos últimos eventos emitidos para o assinante pelo Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante dos eventos </param>
        /// <returns> Lista de eventos <see cref="Stockholder"/> emitidos pelo serviço remoto </returns>
        public async Task<List<StockEventArgs>> RetrieveEventRequest(Stockholder subscriber)
        {
            HttpResponseMessage response = await ClientService.PostAsJsonAsync(RetrieveEventsPath, subscriber);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<StockEventArgs>>();
        }
    }
}
