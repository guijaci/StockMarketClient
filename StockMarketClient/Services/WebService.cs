using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StockMarketClient.Services
{
    /// <summary>
    /// Serviço base para serviços web. Encapsula um cliente e pode ser extendido para se implementar facilmente requisições à API's. 
    /// </summary>
    public class WebService
    {
        private HttpClient _clientService;

        /// <summary>
        /// Serviço de cliente para injeção. Utilizado para realizar chamadas remotas de API
        /// </summary>
        public HttpClient ClientService { get => _clientService; set => _clientService = value; }

        /// <summary>
        /// Construi um cliente padrão para ser utilizado nesta classe.
        /// </summary>
        /// <param name="webservicePath"> URI para serviço web </param>
        /// <returns> Cliente inicializado para realizar chamadas remotas à URI de <paramref name="webservicePath"/> e codificar corpo da requisição em JSON </returns>
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
