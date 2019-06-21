using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EsclScanner
{
    public class StatusProvider
    {
        public static readonly string STATUS_URI_PATTERN = "http://{0}/eSCL/ScannerStatus";

        private IEsclClient esclClient;
        private string endpoint;

        public StatusProvider(IEsclClient esclClient, string host)
        {
            this.esclClient = esclClient;
            this.endpoint = String.Format(STATUS_URI_PATTERN, host);
        }

        public async Task<EsclStatus?> GetStatus()
        {
            var response = await esclClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return null;
            var content = response.Content;
            return new EsclStatus{
                State = extractMarkupContent(content, "State"),
                Version = extractMarkupContent(content, "Version"),
            };
        }

        private string extractMarkupContent(string xml, string markup)
        {
            int startPosition = xml.IndexOf(markup);
            int from = xml.IndexOf(">", startPosition) + 1;
            int to = xml.IndexOf("<", from);
            return xml.Substring(from, to-from);
        }
    }
}
