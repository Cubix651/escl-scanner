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
    }
}