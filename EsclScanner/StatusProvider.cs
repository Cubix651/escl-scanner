using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace EsclScanner
{
    public class StatusProvider
    {
        public static readonly string STATUS_URI_PATTERN = "http://{0}/eSCL/ScannerStatus";
        public static readonly string SCAN_NAMESPACE = "http://schemas.hp.com/imaging/escl/2011/05/03";
        public static readonly string PWG_NAMESPACE = "http://www.pwg.org/schemas/2010/12/sm";

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
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            var namespaceManager = createNamespaceManager(xmlDocument);
            var versionNode = xmlDocument.SelectSingleNode("/scan:ScannerStatus/pwg:Version", namespaceManager);
            var stateNode = xmlDocument.SelectSingleNode("/scan:ScannerStatus/pwg:State", namespaceManager);

            return new EsclStatus{
                State = stateNode.InnerText,
                Version = versionNode.InnerText
            };
        }

        private XmlNamespaceManager createNamespaceManager(XmlDocument xmlDocument)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("scan", SCAN_NAMESPACE);
            namespaceManager.AddNamespace("pwg", PWG_NAMESPACE);
            return namespaceManager;
        }
    }
}
