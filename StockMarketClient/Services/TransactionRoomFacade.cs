using StockMarketClient.Models;
using System.Threading.Tasks;
using System;
using System.Timers;
using System.Linq;
using System.Collections.Generic;

namespace StockMarketClient.Services
{
    public class TransactionRoomFacade
    {
        private StockMarketService _stockMarketService;
        private Timer _eventPollTimer;

        public StockMarketService StockMarketService { get => _stockMarketService; set => _stockMarketService = value; }
        public Timer EventPollTimer { get => _eventPollTimer; set => _eventPollTimer = value; }

        public delegate void StockEventHandler(object sender, StockEventArgs stockEvent);
        public event StockEventHandler AddedStockEvent;
        public event StockEventHandler RemovedStockEvent;
        public event StockEventHandler UpdatedStockEvent;
        public event StockEventHandler TradedStockEvent;

        public void SetupEventPolling(Stockholder subscriber, long interval)
        {
            EventPollTimer.Elapsed += 
                (s, e) => PollStockEvents(subscriber);
            EventPollTimer.Interval = interval;
            EventPollTimer.AutoReset = true;
            EventPollTimer.Enabled = true;
        }

        public void PollStockEvents(Stockholder subscriber)
        {
            var request = Task.Run(async () => await StockMarketService.RetrieveEventsRequest(subscriber));
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

        public BuyStockOrder AddBuyStockOrder(BuyStockOrder stockOrder) {
            var request = Task.Run(async () => await StockMarketService.CreateBuyStockOrderRequest(stockOrder));
            request.Wait();
            return request.Result;
        }

        public async Task<BuyStockOrder> AddBuyStockOrderAsync(BuyStockOrder stockOrder) => 
            await StockMarketService.CreateBuyStockOrderRequest(stockOrder);

        public SellStockOrder AddSellStockOrder(SellStockOrder stockOrder)
        {
            var request = Task.Run(async () => await StockMarketService.CreateSellStockOrderRequest(stockOrder));
            request.Wait();
            return request.Result;
        }

        public async Task<SellStockOrder> AddSellStockOrderAsync(SellStockOrder stockOrder) => 
            await StockMarketService.CreateSellStockOrderRequest(stockOrder);
    }
}
