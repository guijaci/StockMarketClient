using StockMarketClient.Models;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Generic;

namespace StockMarketClient.Services
{
    /// <summary>
    /// Fachada para requisições do Serviço Web de Backend
    /// </summary>
    public class TransactionRoomFacade
    {
        private StockMarketService _stockMarketService;
        private Timer _eventPollTimer;

        /// <summary>
        /// Serviço para realizar chamadas ao Serviço Web de Backend
        /// </summary>
        public StockMarketService StockMarketService { get => _stockMarketService; set => _stockMarketService = value; }
        /// <summary>
        /// Temporizador de consulta de eventos
        /// </summary>
        public Timer EventPollTimer { get => _eventPollTimer; set => _eventPollTimer = value; }

        /// <summary>
        /// Delegado para eventos redirecionados provenientes do Serviço Web de Backend
        /// </summary>
        /// <param name="sender"> Nome do objeto emissor do evento (neste caso, será sempre <see cref="TransactionRoomFacade"/>) </param>
        /// <param name="stockEvent"> Argumentos do evento emitido </param>
        public delegate void StockEventHandler(object sender, StockEventArgs stockEvent);
        /// <summary> Eventos de ordens de operação sobre ação adicionadas no Serviço Web de Backend </summary>
        public event StockEventHandler AddedStockEvent;
        /// <summary> Eventos de ordens de operação sobre ação removidas no Serviço Web de Backend </summary>
        public event StockEventHandler RemovedStockEvent;
        /// <summary> Eventos de ordens de operação sobre ação atualizadas no Serviço Web de Backend </summary>
        public event StockEventHandler UpdatedStockEvent;
        /// <summary> Eventos de ordens de operação sobre ação transacionadas no Serviço Web de Backend </summary>
        public event StockEventHandler TradedStockEvent;

        /// <summary>
        /// Configura e inicializa temporizador para realizar consulta de eventos no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante dos eventos consultados </param>
        /// <param name="interval"> intervalo de tempo entre as consultas em milisegundos </param>
        public void SetupEventPolling(Stockholder subscriber, long interval)
        {
            EventPollTimer.Elapsed += 
                (s, e) => PollStockEvents(subscriber);
            EventPollTimer.Interval = interval;
            EventPollTimer.AutoReset = true;
            EventPollTimer.Enabled = true;
        }

        /// <summary>
        /// Callback de timer para consulta de eventos no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante dos eventos consultados </param>
        public void PollStockEvents(Stockholder subscriber)
        {
            var request = Task.Run(() => PollStockEventsAsync(subscriber));
            request.Wait();
            List<StockEventArgs> events = request.Result;
            events.ForEach(s => {
                switch (s.EventType)
                {
                    case StockEventArgs.EStockEventType.ADDED:
                        AddedStockEvent?.Invoke(this, s);
                        break;
                    case StockEventArgs.EStockEventType.REMOVED:
                        RemovedStockEvent?.Invoke(this, s);
                        break;
                    case StockEventArgs.EStockEventType.UPDATED:
                        UpdatedStockEvent?.Invoke(this, s);
                        break;
                    case StockEventArgs.EStockEventType.TRADED:
                        TradedStockEvent?.Invoke(this, s);
                        break;
                }
            });
        }

        /// <summary>
        /// Realiza consulta de eventos de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante dos eventos consultados </param>
        /// <returns> Lista de eventos gerados pelo Serviço Web de Backend e consumidos durante requisição </returns>
        public async Task<List<StockEventArgs>> PollStockEventsAsync(Stockholder subscriber) =>
            await StockMarketService.RetrieveEventRequest(subscriber);

        /// <summary>
        /// Realiza uma requisição de adição de ordens de compra de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de compra de ações para ser adicionado </param>
        /// <returns> Ordem de compra de ações adicionada </returns>
        public BuyStockOrder AddBuyStockOrder(BuyStockOrder stockOrder) {
            var request = Task.Run(() => AddBuyStockOrderAsync(stockOrder));
            request.Wait();
            return request.Result;
        }

        /// <summary>
        /// Realiza uma requisição assíncrona de adição de ordens de compra de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de compra de ações para ser adicionado </param>
        /// <returns> Ordem de compra de ações adicionada </returns>
        public async Task<BuyStockOrder> AddBuyStockOrderAsync(BuyStockOrder stockOrder) => 
            await StockMarketService.AddBuyStockOrderRequest(stockOrder);

        /// <summary>
        /// Realiza uma requisição de adição de ordens de venda de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de venda de ações para ser adicionado </param>
        /// <returns> Ordem de venda de ações adicionada </returns>
        public SellStockOrder AddSellStockOrder(SellStockOrder stockOrder)
        {
            var request = Task.Run(() => AddSellStockOrderAsync(stockOrder));
            request.Wait();
            return request.Result;
        }

        /// <summary>
        /// Realiza uma requisição assíncrona de adição de ordens de venda de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="stockOrder"> Ordem de venda de ações para ser adicionado </param>
        /// <returns> Ordem de venda de ações adicionada </returns>
        public async Task<SellStockOrder> AddSellStockOrderAsync(SellStockOrder stockOrder) => 
            await StockMarketService.AddSellStockOrderRequest(stockOrder);

        /// <summary>
        /// Realiza uma requisição de inscrição de eventos de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <see cref="Stockholder"/> assinante dos eventos consultados </param>
        /// <param name="enterprise"> Empresa da qual se deseja receber eventos </param>
        /// <param name="eventType"> Tipo de evento desejado </param>
        /// <param name="isBuying"> Se é um evento relacionado à compra de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param>
        /// <param name="isSelling"> Se é um evento relacionado à venda de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param> 
        /// <returns> O assinante dos eventos </returns>
        public Stockholder StockEventsSubscribe(Stockholder subscriber, string enterprise, StockEventArgs.EStockEventType eventType, bool? isBuying = null, bool? isSelling = null)
        {
            var request = Task.Run(() => StockEventsSubscribeAsync(subscriber, enterprise, eventType, isBuying, isSelling));
            request.Wait();
            return request.Result;
        }

        /// <summary>
        /// Realiza uma requisição assíncrona de inscrição de eventos de ações no Serviço Web de Backend
        /// </summary>
        /// <param name="subscriber"> <c>Stockholder</c> assinante dos eventos consultados </param>
        /// <param name="enterprise"> Empresa da qual se deseja receber eventos </param>
        /// <param name="eventType"> Tipo de evento desejado </param>
        /// <param name="isBuying"> Se é um evento relacionado à compra de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param>
        /// <param name="isSelling"> Se é um evento relacionado à venda de ações(pode ser diferente de nulo apenas se o <paramref name="eventType"/> for <see cref="StockEventArgs.EStockEventType.ADDED"/>) </param> 
        /// <returns> O assinante dos eventos </returns>
        public async Task<Stockholder> StockEventsSubscribeAsync(Stockholder subscriber, string enterprise, StockEventArgs.EStockEventType eventType, bool? isBuying = null, bool? isSelling = null) =>
            await StockMarketService.SubscribeToStockEventRequest(subscriber, enterprise, eventType, isBuying, isSelling);
    }
}
