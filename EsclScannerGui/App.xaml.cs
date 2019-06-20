using Avalonia;
using Avalonia.Markup.Xaml;

namespace EsclScannerGui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}