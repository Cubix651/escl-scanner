using System.Threading.Tasks;
using System.Net.Http;

namespace EsclScanner
{
    public class EsclClient : IEsclClient
    {
        private HttpClient httpClient;

        public EsclClient()
        {
            this.httpClient = new HttpClient();
        }
        
        public async Task<IEsclResponse> GetAsync(string uri)
        {
            var response = await httpClient.GetAsync(uri);
            return new EsclResponse(response);
        }

        public async Task<IEsclResponse> PostAsync(string uri, string body)
        {
            var content = new StringContent(body);
            var response = await httpClient.PostAsync(uri, content);
            return new EsclResponse(response);
        }
    }
}