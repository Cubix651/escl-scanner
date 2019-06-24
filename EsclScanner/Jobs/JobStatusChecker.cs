using Escl.Connection;
using Escl.Utils;
using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class JobStatusChecker
    {
        public static readonly string STATUS_URI_PATTERN = "http://{0}/eSCL/ScannerStatus";

        IEsclClient esclClient;
        string endpoint;
        public string Uri {get;}

        public JobStatusChecker(IEsclClient esclClient, string host, string jobUri)
        {
            this.esclClient = esclClient;
            this.endpoint = string.Format(STATUS_URI_PATTERN, host);
            this.Uri = jobUri;
        }

        public async Task<bool> Ready()
        {
            var response = await esclClient.GetAsync(endpoint);
            var xmlDocument = response.Content;
            var namespaceManager = NamespaceUtils.CreateNamespaceManager(xmlDocument);
            var jobInfoNode = xmlDocument.SelectSingleNode(
                $"/scan:ScannerStatus/scan:Jobs/scan:JobInfo[pwg:JobUri = '{Uri}']",
                namespaceManager);
            var imagesToTransferNode = jobInfoNode.SelectSingleNode("./pwg:ImagesToTransfer", namespaceManager);
            if (int.Parse(imagesToTransferNode.InnerText) > 0)
                return true;
            return false;
        }
    }
}