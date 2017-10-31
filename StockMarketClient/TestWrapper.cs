using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace StockMarketClient
{
    public class TestWrapper
    {
        public static async Task HttpTest()
        {
            Console.WriteLine("Inicializando requisição HTTP");
            HttpClient client = new HttpClient();
            InitializeTestRequest(client);
            Test testResult = await GetTestAsync(client, "/test");
            Console.WriteLine(testResult.test);
        }


        async static Task<Test> GetTestAsync(HttpClient client, string path)
        {
            Test test = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                test = await response.Content.ReadAsAsync<Test>();
            }
            return test;
        }

        public static void InitializeTestRequest(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:8080");  
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class Test
        {
            public string test { get; set; }
        }
    }
}
