using System.Threading.Tasks;

namespace Escl.Jobs
{
    public class JobMonitor
    {
        static readonly int DELAY = 2000;
        private JobStatusChecker statusChecker;

        public JobMonitor(JobStatusChecker statusChecker)
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