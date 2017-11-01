using StockMarketClient.UI.Utils;
using System.Windows;
using System.Windows.Controls;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para SubscribeDialog.xaml
    /// </summary>
    public partial class SubscribeDialog : Window
    {
        public enum EEventType
        {
            TRANSACTION,
            ADDED_BUY,
            ADDED_SELL
        }

        private EEventType _selectedEvent = EEventType.TRANSACTION;

        public EEventType SelectedEvent
        {
            get => _selectedEvent;
            set => _selectedEvent = value;
        }

        public SubscribeDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = true;

        private void CancelButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = false;

        private void EnterpriseTextBox_TextChanged(object sender, TextChangedEventArgs e) => 
            okButton.IsEnabled = !string.IsNullOrWhiteSpace(enterpriseTextBox.Text);
    }
}
