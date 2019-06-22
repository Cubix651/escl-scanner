using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Escl.Connection
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
            var xml = await readResponseAsync(response);
            return new EsclResponse(content: xml);
        }

        public async Task<IEsclResponse> PostAsync(string uri, string body)
        {
            var content = new StringContent(body);
            var response = await httpClient.PostAsync(uri, content);
            var xml = await readResponseAsync(response);
            return new EsclResponse(content: xml, location: response.Headers.Location);
        }

        private async Task<XmlDocument> readResponseAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return null;
            var xmlStreamTask = response.Content.ReadAsStreamAsync();
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(await xmlStreamTask);
            return xmlDocument;
        }
    }
}