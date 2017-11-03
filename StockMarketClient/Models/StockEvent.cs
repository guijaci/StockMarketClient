using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketClient.Models
{
    /// <summary>
    /// Objeto encapsula informações sobre eventos relacionados à ações, ordems de operação sobre ações e suas transações no mercado de ações
    /// </summary>
    public class StockEventArgs
    {
        public enum EStockEventType
        {
            ADDED, REMOVED, UPDATED, TRADED
        }

        private EStockEventType _eventType;
        private StockOrder _newOrder;
        private StockOrder _prevOrder;
        private StockOrder _buyOrder;
        private StockOrder _sellOrder;
        private Stocks _tradedStock;

        [JsonProperty(PropertyName = "eventType")]
        public EStockEventType EventType { get => _eventType; set => _eventType = value; }
        [JsonProperty(PropertyName = "newOrder")]
        public StockOrder NewOrder { get => _newOrder; set => _newOrder = value; }
        [JsonProperty(PropertyName = "prevOrder")]
        public StockOrder PrevOrder { get => _prevOrder; set => _prevOrder = value; }
        [JsonProperty(PropertyName = "buyOrder")]
        public StockOrder BuyOrder { get => _buyOrder; set => _buyOrder = value; }
        [JsonProperty(PropertyName = "sellOrder")]
        public StockOrder SellOrder { get => _sellOrder; set => _sellOrder = value; }
        [JsonProperty(PropertyName = "tradedStock")]
        public Stocks TradedStock { get => _tradedStock; set => _tradedStock = value; }
    }
}
