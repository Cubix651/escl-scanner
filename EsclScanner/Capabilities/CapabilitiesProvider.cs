using Escl.Utils;
using Escl.Connection;
using System;
using System.Threading.Tasks;

namespace Escl.Capabilities
{
    public class CapabilitiesProvider
    {
        public static readonly string CAPABILITIES_URI_PATTERN = "http://{0}/eSCL/ScannerCapabilities";

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
            var xml = response.Content;
            var namespaceManager = NamespaceUtils.CreateNamespaceManager(xml);
            var modelNode = xml.SelectSingleNode("/scan:ScannerCapabilities/pwg:MakeAndModel",
                                                 namespaceManager);
            string model = modelNode?.InnerText;
            return new CapabilitiesInfo { Model = model };
        }
    }
}
