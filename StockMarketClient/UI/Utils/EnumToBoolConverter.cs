using System;
using System.Globalization;
using System.Windows.Data;

namespace StockMarketClient.UI.Utils
{
    /// <summary>
    /// Converte um enum para booleano e vice versa. 
    /// Auxilia na conversão de valores da opção de eventos para seleção de tipos de eventos durante janela de diálogo de inscrição
    /// </summary>
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return Binding.DoNothing;
        }
    }
}
