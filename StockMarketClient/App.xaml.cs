using StockMarketClient.Builders.Services;
using StockMarketClient.Models;
using StockMarketClient.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace StockMarketClient
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Stockholder _stockholder = null;
        private TransactionRoomFacade _transactionRoomService = null;
        private const int POLL_RATE = 5000;

        public Stockholder Stockholder { get => _stockholder; set => _stockholder = value; }
        public TransactionRoomFacade TransactionRoomService { get => _transactionRoomService; set => _transactionRoomService = value; }

        public App()
        {
            TransactionRoomService = new TransactionRoomFacade()
                .WithEventPollTimer(new Timer())
                .WithStockMarketService(new StockMarketService()
                    .WithClientService(WebService.DefaultClient("http://localhost:8080")));
        }

        public void StartEventPolling() => 
            TransactionRoomService.SetupEventPolling(Stockholder, POLL_RATE);

    }
}
