using Escl.Utils;
using Escl.Connection;
using System;
using System.Threading.Tasks;
using System.Xml;

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

        private class Extractor
        {
            XmlDocument xml;
            XmlNamespaceManager namespaceManager;

            public Extractor(XmlDocument xml, XmlNamespaceManager namespaceManager)
            {
                this.xml = xml;
                this.namespaceManager = namespaceManager;
            }

            public string Extract(string xpath)
            {
                var node = xml.SelectSingleNode(xpath, namespaceManager);
                return node?.InnerText;
            }

        }

        public async Task<CapabilitiesInfo> GetCapabilities()
        {
            var response = await esclClient.GetAsync(endpoint);
            var xml = response.Content;
            var namespaceManager = NamespaceUtils.CreateNamespaceManager(xml);
            var extractor = new Extractor(xml, namespaceManager);
            
            return new CapabilitiesInfo
            {
                Model = extractor.Extract("/scan:ScannerCapabilities/pwg:MakeAndModel")
            };
        }
    }
}
