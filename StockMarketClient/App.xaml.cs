using StockMarketClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StockMarketClient
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Stockholder _stockholder = null;

        internal Stockholder Stockholder { get => _stockholder; set => _stockholder = value; }
    }
}
