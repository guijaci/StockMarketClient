using StockMarketClient.Builders.Models;
using StockMarketClient.Models;
using StockMarketClient.UI.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace StockMarketClient.UI
{
    /// <summary> 
    /// Interação lógica para MainWindow.xaml. 
    /// Controla janela principal que concentra as principais operações do sistema
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Singleton da aplicação geral, armazena principais recursos da aplicação.
        /// </summary>
        private App app = ((App)Application.Current);
        /// <summary>
        /// Último objeto de ação criado, para ser injetado na próxima janela de diálogo aberta
        /// </summary>
        private Stocks lastCreatedStocks = new Stocks()
            .WithEnterprise("")
            .WithPrice(1)
            .WithQuantity(1);

        private ObservableCollection<LineElement> _orderList = new ObservableCollection<LineElement>();
        private ObservableCollection<string> _eventList = new ObservableCollection<string>();

        /// <summary> 
        /// Nome do acionista, que é mostrado na janela principal 
        /// </summary>
        public string StockholderName { get => app.Stockholder?.Name; set => app.Stockholder.Name = value; }

        /// <summary> 
        /// Lista de elementos da tabela de ordens de ação 
        /// </summary>        
        public ObservableCollection<LineElement> OrderList { get => _orderList; set => _orderList = value; }
        /// <summary> 
        /// Lista de elementos de eventos gerados 
        /// </summary>
        public ObservableCollection<string> EventList { get => _eventList; set => _eventList = value; }

        /// <summary> 
        /// Construtor principal, inicialize componentes de view e realiza inscrição nos eventos do acionista 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            app.TransactionRoomService.AddedStockEvent += (sender, args) => app.Dispatcher.Invoke(() =>
                OrderList.Add(new LineElement(args.NewOrder)));

            app.TransactionRoomService.RemovedStockEvent += (sender, args) => app.Dispatcher.Invoke(() =>
                OrderList.Where(elem => elem?.StockOrder?.Id == args?.PrevOrder?.Id).ToList().ForEach(elem => OrderList.Remove(elem)));

            app.TransactionRoomService.UpdatedStockEvent += (sender, args) => app.Dispatcher.Invoke(() =>
                OrderList.Where(elem => elem?.StockOrder?.Id == args?.PrevOrder?.Id).ToList().ForEach(elem => elem.StockOrder = args.NewOrder));

            app.TransactionRoomService.AddedStockEvent += (sender, args) => InsertEventMessage(args);

            app.TransactionRoomService.TradedStockEvent += (sender, args) => InsertEventMessage(args);
        }

        /// <summary> 
        /// Adiciona novo item na lista de eventos capturados 
        /// </summary>
        /// <param name="args"> Evento para adicionar à lista </param>
        private void InsertEventMessage(StockEventArgs args)
        {
            app.Dispatcher.Invoke(() => EventList.Insert(0, CreateEventMessage(args)));
        }

        /// <summary> 
        /// Inicia janela de dialog para criar novas ações 
        /// </summary>
        /// <param name="onSucces"> Callback caso janela de dialogo tenha sido finalizada com sucesso, recebe como parâmetro ação gerada </param>
        private void ShowCreateStockDialog(Action<Stocks> onSucces)
        {
            ShowCreateStockDialog(onSucces, () => { });
        }

        /// <summary> 
        /// Inicia janela de dialog para criar novas ações 
        /// </summary>
        /// <param name="onSucces"> Callback caso janela de dialogo tenha sido finalizada com sucesso, recebe como parâmetro ação gerada </param>
        /// <param name="onFailure"> Callback caso janela de dialogo tenha sido cancelada </param>
        private void ShowCreateStockDialog(Action<Stocks> onSucces, Action onFailure)
        {
            CreateStockOrderDialog createStockOrderDialog = new CreateStockOrderDialog
            {
                Answer = lastCreatedStocks
            };

            if (createStockOrderDialog.ShowDialog() == true)
            {
                Stocks stocks = createStockOrderDialog.Answer;
                lastCreatedStocks = stocks;
                onSucces(stocks);
            }
            else
                onFailure();
        }

        /// <summary> 
        /// Cria uma mensagen correspondente para cada tipo de evento para ser exibido na lista de eventos 
        /// </summary>
        /// <param name="stockEvent">  Evento de para ser exibido </param>
        /// <returns> Mensagem correspondente ao evento </returns>
        private string CreateEventMessage(StockEventArgs stockEvent)
        {
            StockOrder order1;
            StockOrder order2;
            Stocks traded;
            switch (stockEvent.EventType)
            {
                case StockEventArgs.EStockEventType.ADDED:
                    order1 = stockEvent?.NewOrder;
                    return string.Format("{0} wants to {1} {2} stock{3} from {4} for {5:c}",
                        order1.OrderPlacer?.Name,
                        order1?.IsBuying == true ? "buy" : order1?.IsSelling == true ? "sell" : "do something with",
                        order1?.Stocks?.Quantity,
                        order1?.Stocks?.Quantity > 0 ? "s" : "",
                        order1?.Stocks?.Enterprise,
                        order1?.Stocks?.Price);
                case StockEventArgs.EStockEventType.TRADED:
                    order1 = stockEvent?.BuyOrder;
                    order2 = stockEvent?.SellOrder;
                    traded = stockEvent?.TradedStock;
                    return string.Format("{0} bought from {1} {2} {3} stocks for {4:c}",
                        order1?.OrderPlacer?.Name,
                        order2?.OrderPlacer?.Name,
                        traded?.Quantity,
                        traded?.Enterprise,
                        traded?.Price);
            }
            return null;
        }

        /// <summary> 
        /// Callback para botao "Buy Stocks" pressionado. 
        /// Inicializa janela de dialogo para compra de ações 
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void BuyStocksButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateStockDialog(stocks =>
            {
                StockOrder stockOrder = app.TransactionRoomService.AddBuyStockOrder( 
                    new BuyStockOrder()
                    .WithOrderPlacer(app.Stockholder)
                    .WithStocks(stocks) as BuyStockOrder);
                Console.WriteLine(string.Format("ID: {0}", stockOrder?.Id));
            });
        }

        /// <summary> 
        /// Callback para botao "Sell Stocks" pressionado. 
        /// Inicializa janela de dialogo para venda de ações 
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void SellStocksButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCreateStockDialog(stocks =>
            {
                StockOrder stockOrder = app.TransactionRoomService.AddSellStockOrder(
                    new SellStockOrder()
                    .WithOrderPlacer(app.Stockholder)
                    .WithStocks(stocks) as SellStockOrder);
                Console.WriteLine(string.Format("ID: {0}", stockOrder?.Id));
            });
        }

        /// <summary> 
        /// Callback para botao "Subscribe" pressionado. 
        /// Inicializa janela de dialogo para inscrição em eventos 
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            SubscribeDialog subscribeDialog = new SubscribeDialog();
            if(subscribeDialog.ShowDialog() == true)
            {
                var options = subscribeDialog.Answer;
                StockEventArgs.EStockEventType eventType;
                switch (options.EventType)
                {
                    case SubscribeDialog.ERadioEvent.TRANSACTION:
                        eventType = StockEventArgs.EStockEventType.TRADED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType);
                        break;
                    case SubscribeDialog.ERadioEvent.ADDED_BUY:
                        eventType = StockEventArgs.EStockEventType.ADDED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType, true, null);
                        break;
                    case SubscribeDialog.ERadioEvent.ADDED_SELL:
                        eventType = StockEventArgs.EStockEventType.ADDED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType, null, true);
                        break;
                }
            }
        }

        /// <summary>
        /// Classe envelope para exibir uma ordem de ação como linhas de uma tabela
        /// </summary>
        public class LineElement : INotifyPropertyChanged
        {
            private StockOrder _stockOrder;

            /// <summary> 
            /// Evento para propriedades internas alteradas 
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary> 
            /// Ordem de ação envelopado por <see cref="LineElement"/>
            /// </summary>
            /// <value> Operações set lançam eventos <see cref="PropertyChangedEventHandler"/> para que observadores atualizem</value>
            public StockOrder StockOrder
            {
                get => _stockOrder;
                set
                {
                    _stockOrder = value;
                    NotifyPropertyChanged("Order");
                    NotifyPropertyChanged("Enterprise");
                    NotifyPropertyChanged("Price");
                    NotifyPropertyChanged("Quantity");
                }
            }

            /// <summary> 
            /// Construtor padrão, insere ordem para ser envelopada 
            /// </summary>
            /// <param name="order"> Ordem que será envelopada </param>
            public LineElement(StockOrder order) => StockOrder = order;

            /// <summary> 
            /// Campo da coluna <see cref="Order"/>
            /// </summary>
            /// <value> Retorna por get "BUYING" ou "SELLING", dependo do tipo de ordem (compra ou venda) </value>
            public string Order => StockOrder?.IsBuying == true ? "BUYING" : StockOrder?.IsSelling == true ? "SELLING" : "ORDER";
            /// <summary> 
            /// Campo da coluna <see cref="Enterprise"/>
            /// </summary>
            /// <value> Retorna por get nome da empresa referente às ações desejadas </value>
            public string Enterprise => StockOrder?.Stocks?.Enterprise;
            /// <summary> 
            /// Campo da coluna <see cref="Price"/>
            /// </summary>
            /// <value> Retorna por get o preço da ação interessada desejado na transação </value>
            public string Price => string.Format("{0:c}", StockOrder?.Stocks?.Price ?? 0);
            /// <summary> 
            /// Campo da coluna <see cref="Quantity"/>
            /// </summary>
            /// <value> Retorna por get a quantidade minima desejada para compra destas ações </value>
            public string Quantity => string.Format("{0}", StockOrder?.Stocks?.Quantity ?? 0);

            /// <summary> 
            /// Notifica que uma propriedade foi alterada 
            /// </summary>
            /// <param name="info"> Nome propriedade alterada </param>
            private void NotifyPropertyChanged(String info) => 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

        }
    }
}
