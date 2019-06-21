using System;
using System.Threading.Tasks;
using System.Xml;
using Escl.Connection;

namespace Escl.Capabilities
{
    public class CapabilitiesProvider
    {
        public static readonly string CAPABILITIES_URI_PATTERN = "http://{0}/eSCL/ScannerStatus";

        private IEsclClient esclClient;
        private string endpoint;

        public CapabilitiesProvider(IEsclClient esclClient, string host)
        {
            this.esclClient = esclClient;
            this.endpoint = String.Format(CAPABILITIES_URI_PATTERN, host);
        }

        public async Task<CapabilitiesInfo?> GetCapabilities()
        {
            var response = await esclClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
                return null;
            var content = response.Content;
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            return null;
        }
    }
}
