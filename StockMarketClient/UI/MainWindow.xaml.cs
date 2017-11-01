using StockMarketClient.Builders.Models;
using StockMarketClient.Models;
using StockMarketClient.UI.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

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

        public string StockholderName { get => app.Stockholder?.Name; set => app.Stockholder.Name = value; }
        
        public MainWindow()
        {
            InitializeComponent();
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

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(TestWrapper.HttpTest).Wait();
            }
            catch(Exception ex)
            {

            }
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

            }
            else
            {

            }
        }
    }
}
