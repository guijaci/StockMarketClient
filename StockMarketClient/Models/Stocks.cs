using Newtonsoft.Json;

namespace StockMarketClient.Models
{
    class Stocks
    {
        private long _version = 0;
        private double _price = 0;
        private long _quantity = 0;
        private string _enterprise = "";

        [JsonProperty(PropertyName = "version")]
        public long Version { get => _version; private set => _version = value; }

        public Stocks() { }

        public Stocks(double price, long quantity, string enterprise)
        {
            _price = price;
            _quantity = quantity;
            _enterprise = enterprise;
        }

        [JsonProperty(PropertyName = "price")]
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                Version++;
            }
        }

        [JsonProperty(PropertyName = "quantity")]
        public long Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                Version++;
            }
        }

        [JsonProperty(PropertyName = "enterprise")]
        public string Enterprise
        {
            get => _enterprise;
            set
            {
                _enterprise = value;
                Version++;
            }
        }
    }
}
