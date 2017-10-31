using StockMarketClient.Builders.Services;
using StockMarketClient.Models;
using StockMarketClient.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        internal Stockholder Stockholder { get => _stockholder; set => _stockholder = value; }
        internal TransactionRoomFacade TransactionRoomService { get => _transactionRoomService; set => _transactionRoomService = value; }

        public App()
        {
            TransactionRoomService = new TransactionRoomFacade()
                .WithStockMarketService(new StockMarketService()
                    .WithClientService(WebService.DefaultClient("http://localhost:8080")));
        }
    }
}
