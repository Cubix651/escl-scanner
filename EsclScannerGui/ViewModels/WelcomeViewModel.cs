using System.Threading.Tasks;
using Escl;

namespace EsclScannerGui.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        MainWindowViewModel mainWindowViewModel;
        public string WelcomeMessage => "Welcome to eSCL Scan Tool!";
        public string Host {get;set;}

        public WelcomeViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public async Task StartTool()
        {
            var scanner = new Scanner(Host);
            var capabilities = await scanner.GetCapabilities();
            mainWindowViewModel.Content = new ScanViewModel(scanner, capabilities);
        }
    }
}