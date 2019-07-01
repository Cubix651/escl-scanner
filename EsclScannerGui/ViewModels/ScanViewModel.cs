using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Escl;
using Escl.Capabilities;
using Escl.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ReactiveUI;

namespace EsclScannerGui.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {
        Scanner scanner;
        Bitmap previewImage;
        public string OutputPath {get; set;}
        public string DocumentFormatExt {get; set;}
        public Resolution Resolution {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public int XOffset {get; set;}
        public int YOffset {get; set;}
        public Bitmap PreviewImage {
            get => previewImage;
            set => this.RaiseAndSetIfChanged(ref previewImage, value);
        }

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
            OutputPath = "scan.jpg";
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

        public async Task PreviewAsync()
        {
            var options = new ScanOptions
            {
                DocumentFormatExt = "image/jpeg",
                Resolution = Capabilities.Resolutions[0],
                Width = Capabilities.MaxWidth,
                Height = Capabilities.MaxHeight,
                XOffset = 0,
                YOffset = 0
            };
            using (var previewStream = await scanner.GetPreviewAsync(options))
            {
                PreviewImage = new Bitmap(previewStream);
            }
        }
    }
}