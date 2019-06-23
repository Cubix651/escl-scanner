using System.Threading.Tasks;
using Escl.Capabilities;
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
        StatusProvider statusProvider;
        CapabilitiesProvider capabilitiesProvider;
        public string Host {get;}

        public Scanner(string host)
        {
            this.esclClient = new EsclClient();
            this.Host = host;
            this.statusProvider = new StatusProvider(esclClient, host);
            this.capabilitiesProvider = new CapabilitiesProvider(esclClient, host);
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

        public async Task<EsclStatus> GetStatus()
        {
            return await statusProvider.GetStatus();
        }

        public async Task<CapabilitiesInfo> GetCapabilities()
        {
            return await capabilitiesProvider.GetCapabilities();
        }
    }
}