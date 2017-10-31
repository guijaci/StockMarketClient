using StockMarketClient.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para StockholderNameDialog.xaml
    /// </summary>
    public partial class StockholderNameDialog : Window
    {
        private App app = ((App)Application.Current);

        public StockholderNameDialog() => 
            InitializeComponent();

        private void CancelButton_Click(object sender, RoutedEventArgs e) => 
            Application.Current.Shutdown();

        private void OkButton_Click(object sender, RoutedEventArgs e) => 
            LoadMainWindow();

        private void StockholderNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => 
            okButton.IsEnabled = !string.IsNullOrWhiteSpace(stockholderNameTextBox.Text);

        private void LoadMainWindow()
        {
            if (!string.IsNullOrWhiteSpace(stockholderNameTextBox.Text))
            {
                Stockholder stockholder = new Stockholder(stockholderNameTextBox.Text);
                app.Stockholder = stockholder;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Activate();
                    mainWindow.Show();
                    Close();
                }));
            }
        }
    }
}
