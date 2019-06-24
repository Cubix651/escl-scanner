using Escl.Utils;
using Escl.Connection;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace Escl.Capabilities
{
    public class CapabilitiesProvider
    {
        public static readonly string CAPABILITIES_URI_PATTERN = "http://{0}/eSCL/ScannerCapabilities";

        private IEsclClient esclClient;
        private string endpoint;

        public CapabilitiesProvider(IEsclClient esclClient, string host)
        {
            this.esclClient = esclClient;
            this.endpoint = String.Format(CAPABILITIES_URI_PATTERN, host);
        }

        private class Extractor
        {
            XmlDocument xml;
            XmlNamespaceManager namespaceManager;

            public Extractor(XmlDocument xml, XmlNamespaceManager namespaceManager)
            {
                this.xml = xml;
                this.namespaceManager = namespaceManager;
            }

            public string Extract(string xpath)
            {
                var node = xml.SelectSingleNode(xpath, namespaceManager);
                return node?.InnerText;
            }

            public int ExtractInt(string xpath, int defaultValue = 0)
            {
                var node = xml.SelectSingleNode(xpath, namespaceManager);
                var valueString = node?.InnerText;
                return valueString.ParseIntOrDefault(defaultValue);
            }

            public IEnumerable<XmlNode> ExtractNodes(string xpath)
            {
                return xml.SelectNodes(xpath, namespaceManager).Cast<XmlNode>();
            }
            
            public List<string> ExtractList(string xpath)
            {
                return ExtractNodes(xpath)
                    .Select(node => node.InnerText)
                    .ToList();
            }
        }

        public async Task<CapabilitiesInfo> GetCapabilities()
        {
            var response = await esclClient.GetAsync(endpoint);
            var xml = response.Content;
            var namespaceManager = NamespaceUtils.CreateNamespaceManager(xml);
            var extractor = new Extractor(xml, namespaceManager);
            
            return new CapabilitiesInfo
            {
                Model = extractor.Extract("/scan:ScannerCapabilities/pwg:MakeAndModel"),
                MinWidth = extractor.ExtractInt("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:MinWidth"),
                MaxWidth = extractor.ExtractInt("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:MaxWidth"),
                MinHeight = extractor.ExtractInt("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:MinHeight"),
                MaxHeight = extractor.ExtractInt("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:MaxHeight"),
                DocumentFormatExtensions = extractor.ExtractList("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:SettingProfiles/scan:SettingProfile/scan:DocumentFormats/scan:DocumentFormatExt"),
                Resolutions = 
                    extractor.ExtractNodes("/scan:ScannerCapabilities/scan:Platen/scan:PlatenInputCaps/scan:SettingProfiles/scan:SettingProfile/scan:SupportedResolutions/scan:DiscreteResolutions/scan:DiscreteResolution")
                        .Select(node => new Resolution {
                            X = node.SelectSingleNode("./scan:XResolution", namespaceManager).InnerText.ParseIntOrDefault(0),
                            Y = node.SelectSingleNode("./scan:YResolution", namespaceManager).InnerText.ParseIntOrDefault(0)
                            }).ToList()
            };
        }
    }
}
