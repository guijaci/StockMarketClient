using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StockMarketClient.Services
{
    public class WebService
    {
        protected string TestPostPath { get => "/post/test"; }
        protected string TestGetPath { get => "/get/test"; }

        private HttpClient _clientService;

        public HttpClient ClientService { get => _clientService; set => _clientService = value; }

        public static HttpClient DefaultClient(string webservicePath)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(webservicePath)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }

}
