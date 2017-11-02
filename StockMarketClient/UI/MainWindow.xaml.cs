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
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private App app = ((App)Application.Current);
        private Stocks lastCreatedStocks = new Stocks()
            .WithEnterprise("")
            .WithPrice(1)
            .WithQuantity(1);

        private ObservableCollection<LineElement> _orderList = new ObservableCollection<LineElement>();
        private ObservableCollection<string> _eventList = new ObservableCollection<string>();

        public string StockholderName { get => app.Stockholder?.Name; set => app.Stockholder.Name = value; }
        public ObservableCollection<LineElement> OrderList { get => _orderList; set => _orderList = value; }
        public ObservableCollection<string> EventList { get => _eventList; set => _eventList = value; }

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

        private void InsertEventMessage(StockEventArgs args)
        {
            app.Dispatcher.Invoke(() => EventList.Insert(0, CreateEventMessage(args)));
        }

        private void ShowCreateStockDialog(Action<Stocks> onSucces)
        {
            ShowCreateStockDialog(onSucces, () => { });
        }

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

        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            SubscribeDialog subscribeDialog = new SubscribeDialog();
            if(subscribeDialog.ShowDialog() == true)
            {
                var options = subscribeDialog.Answer;
                StockEventArgs.EStockEventType eventType;
                switch (options.EventType)
                {
                    case SubscribeDialog.EEventType.TRANSACTION:
                        eventType = StockEventArgs.EStockEventType.TRADED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType);
                        break;
                    case SubscribeDialog.EEventType.ADDED_BUY:
                        eventType = StockEventArgs.EStockEventType.ADDED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType, true, null);
                        break;
                    case SubscribeDialog.EEventType.ADDED_SELL:
                        eventType = StockEventArgs.EStockEventType.ADDED;
                        app.TransactionRoomService.StockEventsSubscribe(app.Stockholder, options.Enterprise, eventType, null, true);
                        break;
                }
            }
        }

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
                        order1?.Stocks?.Quantity > 0 ? "s":"",
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

        public class LineElement : INotifyPropertyChanged
        {
            private StockOrder _stockOrder;

            public event PropertyChangedEventHandler PropertyChanged;

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

            public LineElement(StockOrder order) => StockOrder = order;

            public string Order => StockOrder?.IsBuying == true ? "BUYING" : StockOrder?.IsSelling == true ? "SELLING" : "ORDER";
            public string Enterprise => StockOrder?.Stocks?.Enterprise;
            public string Price => string.Format("{0:c}", StockOrder?.Stocks?.Price ?? 0);
            public string Quantity => string.Format("{0}", StockOrder?.Stocks?.Quantity ?? 0);

            private void NotifyPropertyChanged(String info) => 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

        }
    }
}
