using StockMarketClient.Models;
using StockMarketClient.Builders.Models;
using System.Windows;
using System.Windows.Controls;

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
                enterpriseTextBox.Text = value?.Enterprise;
                priceSpinner.Value = value?.Price;
                quantitySpinner.Value = value?.Quantity;
            }
        }

        private bool IsValid { get => !string.IsNullOrWhiteSpace(enterpriseTextBox.Text) && priceSpinner.Value >= 0.01 && quantitySpinner.Value > 0; }

        public CreateStockOrderDialog()
        {
            InitializeComponent();
            priceSpinner.ValueChanged += PriceSpinner_ValueChanged;
            quantitySpinner.ValueChanged += QuantitySpinner_ValueChanged;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = false;

        private void EnterpriseTextBox_TextChanged(object sender, TextChangedEventArgs e) => 
            okButton.IsEnabled = IsValid;

        private void PriceSpinner_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => 
            okButton.IsEnabled = IsValid;

        private void QuantitySpinner_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => 
            okButton.IsEnabled = IsValid;
    }
}
