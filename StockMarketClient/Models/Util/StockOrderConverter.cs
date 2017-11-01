using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarketClient.Models.Util
{
    class StockOrderConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = 
            new JsonSerializerSettings() { ContractResolver = new StockOrderSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType) => 
            typeof(StockOrder).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jo = JObject.Load(reader);
                if (jo["buying"]?.Value<bool>() == true)
                    return JsonConvert.DeserializeObject<BuyStockOrder>(jo.ToString(), SpecifiedSubclassConversion);
                if (jo["selling"]?.Value<bool>() == true)
                    return JsonConvert.DeserializeObject<SellStockOrder>(jo.ToString(), SpecifiedSubclassConversion);
            }
            throw new Exception();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => 
            throw new NotImplementedException();

        public override bool CanWrite => false;
    }
}
