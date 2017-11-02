using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockMarketClient.Services.Utils
{
    public static class HttpExtensions
    {
        public static Uri WithQuery (this Uri uri, FormUrlEncodedContent query)
        {
            UriBuilder builder = new UriBuilder(uri);
            var task = Task.Run(() => query.ReadAsStringAsync());
            task.Wait();
            builder.Query = task.Result;
            return builder.Uri;
        }
    }
}
