using Escl.Connection;
using Escl.Requests;
using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class EsclJobCreator
    {
        public static readonly string NEW_JOB_URI_PATTERN = "http://{0}/eSCL/ScanJobs";

        IEsclClient esclClient;
        string host;
        string endpoint;
        EsclScanRequestGenerator scanRequestGenerator;   

        public EsclJobCreator(IEsclClient esclClient,
                              string host,
                              EsclScanRequestGenerator scanRequestGenerator)
        {
            this.esclClient = esclClient;
            this.host = host;
            this.endpoint = string.Format(NEW_JOB_URI_PATTERN, host);
            this.scanRequestGenerator = scanRequestGenerator;
        }

        public async Task<EsclJob> CreateJob(ScanOptions options)
        {
            string request = scanRequestGenerator.Generate(options);
            var response = await esclClient.PostAsync(endpoint, request);
            string jobUri = response.Location.PathAndQuery;
            var statusChecker = new JobStatusChecker(esclClient, host, jobUri);
            return new EsclJob(statusChecker);
        }
        
    }
}