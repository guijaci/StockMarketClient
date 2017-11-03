using StockMarketClient.Builders.Services;
using StockMarketClient.Models;
using StockMarketClient.Services;
using System.Timers;
using System.Windows;

namespace StockMarketClient
{
    /// <summary>
    /// Interação lógica para App.xaml.
    /// Singleton do aplicativo. Realiza inicializações de serviços gerais e mantém alguns parâmetros.
    /// </summary>
    public partial class App : Application
    {
        private Stockholder _stockholder = null;
        private TransactionRoomFacade _transactionRoomService = null;

        /// <summary>
        /// Taxa de consulta de eventos no servidor
        /// </summary>
        private const int POLL_RATE = 5000;
        /// <summary>
        /// Endereço do serviço web que expõe APIs utilizadas
        /// </summary>
        private const string BACKEND_PATH = "http://localhost:8080";

        /// <summary>
        /// Acionista da sessão. Injetado após usuário inserir nome
        /// </summary>
        public Stockholder Stockholder { get => _stockholder; set => _stockholder = value; }
        /// <summary>
        /// Serviço de fachada para servidor remoto. Expões API do WebService como métodos comuns em C#
        /// </summary>
        public TransactionRoomFacade TransactionRoomService { get => _transactionRoomService; set => _transactionRoomService = value; }

        /// <summary>
        /// Inicializa serviços principais do aplicativo
        /// </summary>
        public App()
        {
            TransactionRoomService = new TransactionRoomFacade()
                .WithEventPollTimer(new Timer())
                .WithStockMarketService(new StockMarketService()
                    .WithClientService(WebService.DefaultClient(BACKEND_PATH)));
        }

        /// <summary>
        /// Inicializa polling de eventos no servidor
        /// </summary>
        public void StartEventPolling() => 
            TransactionRoomService.SetupEventPolling(Stockholder, POLL_RATE);
    }
}
