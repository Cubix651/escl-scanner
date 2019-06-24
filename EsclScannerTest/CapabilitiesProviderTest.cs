using Xunit;
using FakeItEasy;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Escl.Connection;
using Escl.Capabilities;
using Escl.Utils;

namespace EsclScannerTest
{
    public class CapabilitiesProviderTest
    {
        static readonly string FULL_RESPONSE_BODY =
@"<?xml version=""1.0"" encoding=""UTF-8""?>     
<!-- THIS DATA SUBJECT TO DISCLAIMER(S) INCLUDED WITH THE PRODUCT OF ORIGIN. -->
<scan:ScannerCapabilities xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" xsi:schemaLocation=""http://schemas.hp.com/imaging/escl/2011/05/03 eSCL.xsd"">
  <pwg:Version>2.63</pwg:Version>
  <pwg:MakeAndModel>HP LaserJet MFP M28w</pwg:MakeAndModel>
  <pwg:SerialNumber>VNC3K22655</pwg:SerialNumber>
  <scan:UUID>564E4333-4B32-3236-3535-C8D9D2B6E3F9</scan:UUID>
  <scan:AdminURI>http://NPIB6E3F9.local.</scan:AdminURI>
  <scan:IconURI>http://NPIB6E3F9.local./ipp/images/printer.png</scan:IconURI>
  <scan:Platen>
    <scan:PlatenInputCaps>
      <scan:MinWidth>300</scan:MinWidth>
      <scan:MaxWidth>2550</scan:MaxWidth>
      <scan:MinHeight>300</scan:MinHeight>
      <scan:MaxHeight>3508</scan:MaxHeight>
      <scan:MaxScanRegions>1</scan:MaxScanRegions>
      <scan:SettingProfiles>
        <scan:SettingProfile>
          <scan:ColorModes>
            <scan:ColorMode>RGB24</scan:ColorMode>
            <scan:ColorMode>Grayscale8</scan:ColorMode>
          </scan:ColorModes>
          <scan:ContentTypes>
            <pwg:ContentType>Photo</pwg:ContentType>
            <pwg:ContentType>Text</pwg:ContentType>
            <pwg:ContentType>TextAndPhoto</pwg:ContentType>
          </scan:ContentTypes>
          <scan:DocumentFormats>
            <pwg:DocumentFormat>image/jpeg</pwg:DocumentFormat>
            <pwg:DocumentFormat>application/pdf</pwg:DocumentFormat>
            <pwg:DocumentFormat>application/octet-stream</pwg:DocumentFormat>
            <scan:DocumentFormatExt>image/jpeg</scan:DocumentFormatExt>
            <scan:DocumentFormatExt>application/pdf</scan:DocumentFormatExt>
            <scan:DocumentFormatExt>application/octet-stream</scan:DocumentFormatExt>
          </scan:DocumentFormats>
          <scan:SupportedResolutions>
            <scan:DiscreteResolutions>
              <scan:DiscreteResolution>
                <scan:XResolution>200</scan:XResolution>
                <scan:YResolution>200</scan:YResolution>
              </scan:DiscreteResolution>
              <scan:DiscreteResolution>
                <scan:XResolution>300</scan:XResolution>
                <scan:YResolution>300</scan:YResolution>
              </scan:DiscreteResolution>
              <scan:DiscreteResolution>
                <scan:XResolution>600</scan:XResolution>
                <scan:YResolution>600</scan:YResolution>
              </scan:DiscreteResolution>
            </scan:DiscreteResolutions>
          </scan:SupportedResolutions>
          <scan:ColorSpaces>
            <scan:ColorSpace>sRGB</scan:ColorSpace>
          </scan:ColorSpaces>
        </scan:SettingProfile>
      </scan:SettingProfiles>
      <scan:SupportedIntents>
        <scan:Intent>Document</scan:Intent>
        <scan:Intent>Photo</scan:Intent>
        <scan:Intent>Preview</scan:Intent>
        <scan:Intent>TextAndGraphic</scan:Intent>
      </scan:SupportedIntents>
      <scan:MaxOpticalXResolution>600</scan:MaxOpticalXResolution>
      <scan:MaxOpticalYResolution>600</scan:MaxOpticalYResolution>
    </scan:PlatenInputCaps>
  </scan:Platen>
  <scan:eSCLConfigCap>
    <scan:StateSupport>
      <scan:State>disabled</scan:State>
      <scan:State>enabled</scan:State>
    </scan:StateSupport>
    <scan:ScannerAdminCredentialsSupport>true</scan:ScannerAdminCredentialsSupport>
  </scan:eSCLConfigCap>
</scan:ScannerCapabilities>";

        static readonly string EMPTY_RESPONSE_BODY = 
@"<?xml version=""1.0"" encoding=""UTF-8""?>     
<scan:ScannerCapabilities xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" xsi:schemaLocation=""http://schemas.hp.com/imaging/escl/2011/05/03 eSCL.xsd"">
</scan:ScannerCapabilities>";

        private async Task<CapabilitiesInfo> GetCapabilities(string xmlResponse)
        {
            var responseXml = new XmlDocument();
            responseXml.LoadXml(xmlResponse);
            var client = A.Fake<IEsclClient>();
            A.CallTo(() => client.GetAsync("http://192.168.0.151/eSCL/ScannerCapabilities"))
                .Returns(Task.FromResult<IEsclResponse>(new EsclResponse(content: responseXml)));

            var capabilitiesProvider = new CapabilitiesProvider(client, "192.168.0.151");
            var capabilities = await capabilitiesProvider.GetCapabilities();
            return capabilities;
        }

        [Fact]
        public async Task response_with_model_name()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            Assert.Equal("HP LaserJet MFP M28w", capabilities.Model);
        }
        
        [Fact]
        public async Task response_without_model_name()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Null(capabilities.Model);
        }

        [Fact]
        public async Task response_with_min_width()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            Assert.Equal(300, capabilities.MinWidth);
        }
        
        [Fact]
        public async Task response_without_min_width()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Equal(0, capabilities.MinWidth);
        }

        [Fact]
        public async Task response_with_min_height()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            Assert.Equal(300, capabilities.MinHeight);
        }
        
        [Fact]
        public async Task response_without_min_height()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Equal(0, capabilities.MinHeight);
        }

        [Fact]
        public async Task response_with_max_width()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            Assert.Equal(2550, capabilities.MaxWidth);
        }
        
        [Fact]
        public async Task response_without_max_width()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Equal(0, capabilities.MaxWidth);
        }

        [Fact]
        public async Task response_with_max_height()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            Assert.Equal(3508, capabilities.MaxHeight);
        }
        
        [Fact]
        public async Task response_without_max_height()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Equal(0, capabilities.MaxHeight);
        }

        [Fact]
        public async Task response_with_document_format_ext()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);
            var formats = capabilities.DocumentFormatExtensions;
            formats.Sort();

            Assert.Equal(
                new List<string> {"application/octet-stream","application/pdf","image/jpeg"},
                formats);
        }
        
        [Fact]
        public async Task response_without_document_format_ext()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Empty(capabilities.DocumentFormatExtensions);
        }

        [Fact]
        public async Task response_with_resolutions()
        {
            var capabilities = await GetCapabilities(FULL_RESPONSE_BODY);

            Assert.Equal(
                new List<Resolution> {
                    new Resolution {X = 200, Y = 200},
                    new Resolution {X = 300, Y = 300},
                    new Resolution {X = 600, Y = 600},
                },
                capabilities.Resolutions);
        }
        
        [Fact]
        public async Task response_without_resolutions()
        {
            var capabilities = await GetCapabilities(EMPTY_RESPONSE_BODY);
            Assert.Empty(capabilities.Resolutions);
        }
    }
}
