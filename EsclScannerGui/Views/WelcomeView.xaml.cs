using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EsclScannerGui.Views
{
    public class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}