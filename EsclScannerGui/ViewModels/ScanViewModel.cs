using Avalonia.Controls;
using Escl;
using Escl.Capabilities;
using Escl.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EsclScannerGui.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {
        Scanner scanner;
        public string OutputPath {get; set;}
        public string DocumentFormatExt {get; set;}
        public Resolution Resolution {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public int XOffset {get; set;}
        public int YOffset {get; set;}

        public CapabilitiesInfo Capabilities {get;}

        public ScanViewModel(Scanner scanner,
                             CapabilitiesInfo capabilities)
        {
            this.scanner = scanner;
            Capabilities = capabilities;
            DocumentFormatExt = Capabilities.DocumentFormatExtensions[0];
            Resolution = Capabilities.Resolutions[0];
            Width = capabilities.MaxWidth;
            Height = capabilities.MaxHeight;
        }        

        public async Task Scan()
        {
            var options = new ScanOptions
            {
                OutputPath = OutputPath,
                DocumentFormatExt = DocumentFormatExt,
                Resolution = Resolution,
                Width = Width,
                Height = Height,
                XOffset = XOffset,
                YOffset = YOffset
            };
            await scanner.Scan(options);
        }
    }
}