using StockMarketClient.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para StockholderNameDialog.xaml. 
    /// Controla janela de diálogo para escolha do nome do <see cref="Stockholder"/> da sessão
    /// </summary>
    public partial class StockholderNameDialog : Window
    {
        /// <summary>
        /// Singleton da aplicação geral, armazena principais recursos da aplicação.
        /// </summary>
        private App app = ((App)Application.Current);

        /// <summary> 
        /// Inicializa componentes view da janela 
        /// </summary>
        public StockholderNameDialog() => 
            InitializeComponent();

        /// <summary> 
        /// Callback do botão "Ok" pressionado. Encerra com sucesso a janela de diálogo
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void OkButton_Click(object sender, RoutedEventArgs e) => 
            LoadMainWindow();

        /// <summary>
        /// Callback do botão "Cancel" pressionado. Cancela a janela de diálogo
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void CancelButton_Click(object sender, RoutedEventArgs e) =>
            Application.Current.Shutdown();

        /// <summary>
        /// Callback de texto da caixa de inserção do nome do acionista alterado. Verifica se o texto é nulo para habilitar/desabilitar botão "Ok"
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e">Argumentos do evento de texto da caixa de inserção alterado </param>
        private void StockholderNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => 
            okButton.IsEnabled = !string.IsNullOrWhiteSpace(stockholderNameTextBox.Text);

        /// <summary>
        /// Carrega janela principal após usuário inserir o nome corretamente
        /// </summary>
        private void LoadMainWindow()
        {
            if (!string.IsNullOrWhiteSpace(stockholderNameTextBox.Text))
            {
                Stockholder stockholder = new Stockholder(stockholderNameTextBox.Text);
                app.Stockholder = stockholder;
                app.StartEventPolling();

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
