using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketClient.Models
{
    class Stockholder
    {
        private long _version = 0;
        private string _name = "";
        private Guid _id = Guid.NewGuid();

        public Stockholder() { }

        public Stockholder(string name)
        {
            Name = name;
        }

        public long Version { get => _version; private set => _version = value; }
        public Guid Id { get => _id; private set => _id = value; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Version++;
            }
        }
    }
}
