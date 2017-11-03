using System.Windows;
using System.Windows.Controls;

namespace StockMarketClient.UI.Dialogs
{
    /// <summary>
    /// Lógica interna para SubscribeDialog.xaml. 
    /// Controla janela de dialogo para inscrição em eventos do servidor
    /// </summary>
    public partial class SubscribeDialog : Window
    {
        /// <summary> 
        /// Possíveis items selecionados pelo <see cref="RadioButton"/> de eventos 
        /// </summary>
        public enum ERadioEvent
        {
            TRANSACTION,
            ADDED_BUY,
            ADDED_SELL
        }

        /// <summary> 
        /// Encapsula as opções de inscrição configuradas pelo usuário nesta janela de diálogo 
        /// </summary>
        /// <value> Operação get retorna novo objeto de configuração contendo opções de inscrição do usuário </value>
        public SubscriptionOptions Answer => 
            new SubscriptionOptions(enterpriseTextBox.Text, SelectedEvent);
        
        private ERadioEvent _selectedEvent = ERadioEvent.TRANSACTION;

        /// <summary>
        /// Representa o <see cref="RadioButton"/> de eventos selecionado pelo usuário
        /// </summary>
        public ERadioEvent SelectedEvent
        {
            get => _selectedEvent;
            set => _selectedEvent = value;
        }

        /// <summary> 
        /// Inicializa componentes view da janela 
        /// </summary>
        public SubscribeDialog() => 
            InitializeComponent();

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
            okButton.IsEnabled = !string.IsNullOrWhiteSpace(enterpriseTextBox.Text);

        /// <summary>
        /// Classe envelope para opções de inscrição depois de finalizadas
        /// </summary>
        public class SubscriptionOptions
        {
            private string _enterprise;
            private ERadioEvent _eventType;
            
            /// <summary>
            /// Construtor padrão. Envelopa valores de opção de inscrição.
            /// </summary>
            /// <param name="enterprise"> empresa para se realizar inscrição </param>
            /// <param name="eventType"> tipo de evento para se realizar inscrição </param>
            public SubscriptionOptions(string enterprise, ERadioEvent eventType)
            {
                Enterprise = enterprise;
                EventType = eventType;
            }

            /// <summary>
            /// Empresa para realizar inscrição
            /// </summary>
            public string Enterprise { get => _enterprise; set => _enterprise = value; }
            /// <summary>
            /// Tipo de evento para se realizar inscrição
            /// </summary>
            public ERadioEvent EventType { get => _eventType; set => _eventType = value; }
        }
    }
}
