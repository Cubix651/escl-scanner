using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class EsclJob
    {
        static readonly int DELAY = 2000;
        private JobStatusChecker statusChecker;

        public string Uri => statusChecker.Uri;

        public EsclJob(JobStatusChecker statusChecker)
        {
            this.statusChecker = statusChecker;
        }
        public async Task MonitorAsync()
        {
            while(!await statusChecker.Ready())
            {
                await Task.Delay(DELAY);
            }
        }


    }
}