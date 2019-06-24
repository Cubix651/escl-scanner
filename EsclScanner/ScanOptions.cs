using Escl.Utils;

namespace Escl
{
    public struct ScanOptions
    {
        public string OutputPath {get; set;}
        public string DocumentFormatExt {get; set;}
        public Resolution Resolution {get; set;}
        public int Width {get; set;}
        public int Height {get; set;}
        public int XOffset {get; set;}
        public int YOffset {get; set;}
    }
}