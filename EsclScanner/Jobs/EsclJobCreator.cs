using Escl.Connection;
using Escl.Requests;
using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class EsclJobCreator
    {
        public static readonly string NEW_JOB_URI_PATTERN = "http://{0}/eSCL/ScanJobs";

        IEsclClient esclClient;
        string endpoint;
        EsclScanRequestGenerator scanRequestGenerator;   

        public EsclJobCreator(IEsclClient esclClient,
                              string host,
                              EsclScanRequestGenerator scanRequestGenerator)
        {
            this.esclClient = esclClient;
            this.endpoint = string.Format(NEW_JOB_URI_PATTERN, host);
        }

        public async Task<string> CreateJob()
        {
            string request = scanRequestGenerator.Generate();
            var response = await esclClient.PostAsync(endpoint, request);
            return response.LocationHeader.PathAndQuery;
        }
        
    }
}