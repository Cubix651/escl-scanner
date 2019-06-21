using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EsclScanner
{
    public class StatusProvider
    {
        public static readonly string STATUS_URI_PATTERN = "http://{0}/eSCL/ScannerStatus";
        public static readonly string UNSUCCESS_STATUS_CODE = "Unsuccess status code";

        private IEsclClient esclClient;
        private string endpoint;

        public StatusProvider(IEsclClient esclClient, string host)
        {
            this.esclClient = esclClient;
            this.endpoint = String.Format(STATUS_URI_PATTERN, host);
        }

        public async Task<string> GetStatus()
        {
            var response = await esclClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return UNSUCCESS_STATUS_CODE;
            var content = response.Content;
            return extractStateMarkupContent(content);
        }

        private string extractStateMarkupContent(string xml)
        {
            int startPosition = xml.IndexOf("State");
            int from = xml.IndexOf(">", startPosition) + 1;
            int to = xml.IndexOf("<", from);
            return xml.Substring(from, to-from);
        }
    }
}
