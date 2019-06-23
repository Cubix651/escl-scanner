using System;
using System.Threading.Tasks;
using System.Xml;
using Escl.Connection;
using Escl.Utils;

namespace Escl.Status
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

        public async Task<EsclStatus> GetStatus()
        {
            var response = await esclClient.GetAsync(endpoint);
            var xmlDocument = response.Content;
            var namespaceManager = NamespaceUtils.CreateNamespaceManager(xmlDocument);
            var versionNode = xmlDocument.SelectSingleNode("/scan:ScannerStatus/pwg:Version", namespaceManager);
            var stateNode = xmlDocument.SelectSingleNode("/scan:ScannerStatus/pwg:State", namespaceManager);

            return new EsclStatus{
                State = stateNode.InnerText,
                Version = versionNode.InnerText
            };
        }
    }
}
