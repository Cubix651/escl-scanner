using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EsclScannerGui.Views
{
    public class ScanView : UserControl
    {
        public ScanView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}