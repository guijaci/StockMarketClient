using StockMarketClient.Models;
using StockMarketClient.UI.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockMarketClient.UI
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private App app = ((App)Application.Current);
        private CreateStockOrderDialog createStockOrderDialog = new CreateStockOrderDialog();

        public string StockholderName { get => app.Stockholder?.Name; set => app.Stockholder.Name = value; }
        
        public MainWindow()
        {
            InitializeComponent();
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
            if(createStockOrderDialog.ShowDialog() == true)
            {
                Stocks stocks = createStockOrderDialog.Answer;
            }
        }

        private void SellStocksButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
