using Escl.Connection;
using Escl.Status;
using FakeItEasy;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace EsclScannerTest
{
    public class StatusProviderTest
    {
        static readonly string FULL_RESPONSE_BODY = 
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!-- THIS DATA SUBJECT TO DISCLAIMER(S) INCLUDED WITH THE PRODUCT OF ORIGIN. -->
<scan:ScannerStatus
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03""
    xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" 
    xsi:schemaLocation=""http://schemas.hp.com/imaging/escl/2011/05/03 eSCL.xsd"">
        <pwg:Version>2.63</pwg:Version>
        <pwg:State>Idle</pwg:State>
</scan:ScannerStatus>";
        
        [Fact]
        public async Task successful_response_with_idle_status()
        {
            var responseXml = new XmlDocument();
            responseXml.LoadXml(FULL_RESPONSE_BODY);
            var client = A.Fake<IEsclClient>();
            A.CallTo(() => client.GetAsync("http://192.168.0.151/eSCL/ScannerStatus"))
                .Returns(Task.FromResult<IEsclResponse>(new EsclResponse(content: responseXml)));

            var statusProvider = new StatusProvider(client, "192.168.0.151");
            var status = await statusProvider.GetStatus();

            Assert.NotNull(status);
            Assert.Equal("Idle", status.Value.State);
            Assert.Equal("2.63", status.Value.Version);
        }
    }
}
