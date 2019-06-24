using System.Collections.Generic;
using Escl.Utils;

namespace Escl.Capabilities
{
    public struct CapabilitiesInfo
    {
        public string Model {get; set;}
        public int MinWidth {get; set;}
        public int MaxWidth {get; set;}
        public int MinHeight {get; set;}
        public int MaxHeight {get; set;}
        public List<string> DocumentFormatExtensions {get; set;}
        public List<Resolution> Resolutions {get; set;}
    }
}