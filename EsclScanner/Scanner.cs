using System.Threading.Tasks;
using Escl.Connection;
using Escl.Jobs;
using Escl.Requests;
using Escl.Status;

namespace Escl
{
    public class Scanner
    {
        EsclClient esclClient;
        EsclScanRequestGenerator requestGenerator;
        EsclJobCreator jobCreator;
        public string Host {get;}
        public StatusProvider StatusProvider {get;}

        public Scanner(string host)
        {
            this.esclClient = new EsclClient();
            this.Host = host;
            this.StatusProvider = new StatusProvider(esclClient, host);
            this.requestGenerator = new EsclScanRequestGenerator();
            this.jobCreator = new EsclJobCreator(esclClient, host, requestGenerator);
        }

        public async Task Scan(ScanOptions options)
        {
            var job = await jobCreator.CreateJob();
            await job.MonitorAsync();
            var fileFetcher = new EsclFileFetcher(esclClient, Host, job.Uri);
            await fileFetcher.SaveToFile(options.OutputPath);
        }
    }
}