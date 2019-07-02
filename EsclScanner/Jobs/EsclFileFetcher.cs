using Escl.Connection;
using System.IO;
using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class EsclFileFetcher
    {
        public static readonly string RESULT_URI_PATTERN = "http://{0}{1}/NextDocument";
        IEsclClient esclClient;
        string endpoint;

        public EsclFileFetcher(IEsclClient esclClient, string host, string jobUri)
        {
            this.esclClient = esclClient;
            this.endpoint = string.Format(RESULT_URI_PATTERN, host, jobUri);
        }

        public async Task<Stream> GetStreamAsync()
        {
            return await esclClient.GetStreamAsync(endpoint);
        }

        public async Task SaveToFile(string path)
        {
            using(var file = new FileStream(path, FileMode.Create))
            {
                using (var resultStream = await GetStreamAsync())
                {
                    resultStream.CopyTo(file);
                }
            }
        }
    }
}