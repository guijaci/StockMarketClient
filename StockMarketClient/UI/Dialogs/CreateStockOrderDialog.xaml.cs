using StockMarketClient.Models;
using StockMarketClient.Builders.Models;
using System.Windows;
using System.Windows.Controls;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para CreateStockOrderDialog.xaml.
    /// Controla janela de diálogo para criar um objeto <c>Stocks</c>
    /// </summary>
    public partial class CreateStockOrderDialog : Window
    {
        /// <summary>
        /// Envelope para recuperar ação criada 
        /// </summary>
        /// <value> Operação get retorna novo <c>Stocks</c> criado com a janela de dialogo. 
        /// Operação set altera valores dos views com valores do <c>Stocks</c> correspondente passado </value>
        public Stocks Answer {
            get => new Stocks()
                .WithEnterprise(enterpriseTextBox.Text)
                .WithPrice(priceSpinner.Value ?? 0.01)
                .WithQuantity(quantitySpinner.Value ?? 1);
            set
            {
                enterpriseTextBox.Text = value?.Enterprise ?? "";
                priceSpinner.Value = value?.Price ?? 1;
                quantitySpinner.Value = value?.Quantity ?? 1;
            }
        }

        /// <summary>
        /// Envelope para consultar se é possível finalizar a janela de dialogo com sucesso (ou seja, os valores inseridos são consistentes)
        /// </summary>
        private bool IsValid => 
            !string.IsNullOrWhiteSpace(enterpriseTextBox.Text) && priceSpinner.Value >= 0.01 && quantitySpinner.Value > 0;

        /// <summary>
        /// Construtor para inicializar componentes view 
        /// </summary>
        public CreateStockOrderDialog()
        {
            InitializeComponent();
            //por alguma razão, realizar dinamic binding destes eventos não funciona
            priceSpinner.ValueChanged += PriceSpinner_ValueChanged;
            quantitySpinner.ValueChanged += QuantitySpinner_ValueChanged;
        }

        /// <summary> 
        /// Callback do botão "Ok" pressionado. Encerra com sucesso a janela de diálogo
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void OkButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = true;

        /// <summary>
        /// Callback do botão "Cancel" pressionado. Cancela a janela de diálogo
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e"> Argumentos do evento do botão pressionado </param>
        private void CancelButton_Click(object sender, RoutedEventArgs e) => 
            DialogResult = false;

        /// <summary>
        /// Callback de texto da caixa de inserção do nome da empresa alterado. Verifica se o texto é nulo para habilitar/desabilitar botão "Ok"
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e">Argumentos do evento de texto da caixa de inserção alterado </param>
        private void EnterpriseTextBox_TextChanged(object sender, TextChangedEventArgs e) => 
            okButton.IsEnabled = IsValid;

        /// <summary>
        /// Callback de texto da caixa de rolagem do preço das ações alterado. Verifica se o texto é nulo para habilitar/desabilitar botão "Ok"
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e">Argumentos do evento de texto da caixa de rolagem alterado </param>
        private void PriceSpinner_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => 
            okButton.IsEnabled = IsValid;

        /// <summary>
        /// Callback de texto da caixa de rolagem de quantidade de ações alterado. Verifica se o texto é nulo para habilitar/desabilitar botão "Ok"
        /// </summary>
        /// <param name="sender"> Objeto que gerou o evento </param>
        /// <param name="e">Argumentos do evento de texto da caixa de rolagem alterado </param>
        private void QuantitySpinner_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => 
            okButton.IsEnabled = IsValid;
    }
}
