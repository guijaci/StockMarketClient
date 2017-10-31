using StockMarketClient.Models;
using StockMarketClient.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para CreateStockOrderDialog.xaml
    /// </summary>
    public partial class CreateStockOrderDialog : Window
    {
        internal Stocks Answer {
            get => new Stocks()
                .WithEnterprise(enterpriseTextBox.Text)
                .WithPrice(priceSpinner.Value ?? 0.01)
                .WithQuantity(quantitySpinner.Value ?? 1);
            set
            {
                enterpriseTextBox.Text = value.Enterprise;
                priceSpinner.Value = value.Price;
                quantitySpinner.Value = value.Quantity;
            }
        }

        public CreateStockOrderDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
