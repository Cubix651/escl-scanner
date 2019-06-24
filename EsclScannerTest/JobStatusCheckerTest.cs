using Escl.Connection;
using Escl.Jobs;
using FakeItEasy;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace EsclScannerTest
{
    public class JobStatusCheckerTest
    {
        static readonly string JOB_READY_RESPONSE_BODY = 
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!-- THIS DATA SUBJECT TO DISCLAIMER(S) INCLUDED WITH THE PRODUCT OF ORIGIN. -->
<scan:ScannerStatus xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" xsi:schemaLocation=""http://schemas.hp.com/imaging/escl/2011/05/03 eSCL.xsd"">
  <pwg:Version>2.63</pwg:Version>
  <pwg:State>Idle</pwg:State>
  <scan:Jobs>
    <scan:JobInfo>
      <pwg:JobUri>/eSCL/ScanJobs/pjcqbcg8-73aw-17j4-1007-yrbwnwyo</pwg:JobUri>
      <pwg:JobUuid>pjcqbcg8-73aw-17j4-1007-yrbwnwyo</pwg:JobUuid>
      <scan:Age>1597</scan:Age>
      <pwg:ImagesCompleted>1</pwg:ImagesCompleted>
      <pwg:ImagesToTransfer>1</pwg:ImagesToTransfer>
      <scan:TransferRetryCount>0</scan:TransferRetryCount>
      <pwg:JobState>Completed</pwg:JobState>
      <pwg:JobStateReasons>
        <pwg:JobStateReason>JobCompletedSuccessfully</pwg:JobStateReason>
      </pwg:JobStateReasons>
    </scan:JobInfo>
    <scan:JobInfo>
      <pwg:JobUri>/eSCL/ScanJobs/pjcqbcg8-1wkf-rxzr-1006-nno0gxg7</pwg:JobUri>
      <pwg:JobUuid>pjcqbcg8-1wkf-rxzr-1006-nno0gxg7</pwg:JobUuid>
      <scan:Age>1642</scan:Age>
      <pwg:ImagesCompleted>1</pwg:ImagesCompleted>
      <pwg:ImagesToTransfer>1</pwg:ImagesToTransfer>
      <scan:TransferRetryCount>0</scan:TransferRetryCount>
      <pwg:JobState>Aborted</pwg:JobState>
      <pwg:JobStateReasons>
        <pwg:JobStateReason>JobCanceledAtDevice</pwg:JobStateReason>
      </pwg:JobStateReasons>
    </scan:JobInfo>
  </scan:Jobs>
</scan:ScannerStatus>";

        static readonly string JOB_IN_PROGRESS_RESPONSE_BODY = 
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!-- THIS DATA SUBJECT TO DISCLAIMER(S) INCLUDED WITH THE PRODUCT OF ORIGIN. -->
<scan:ScannerStatus xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" xsi:schemaLocation=""http://schemas.hp.com/imaging/escl/2011/05/03 eSCL.xsd"">
  <pwg:Version>2.63</pwg:Version>
  <pwg:State>Processing</pwg:State>
  <scan:Jobs>
    <scan:JobInfo>
      <pwg:JobUri>/eSCL/ScanJobs/pjcqbcg8-73aw-17j4-1007-yrbwnwyo</pwg:JobUri>
      <pwg:JobUuid>pjcqbcg8-73aw-17j4-1007-yrbwnwyo</pwg:JobUuid>
      <scan:Age>1597</scan:Age>
      <pwg:ImagesCompleted>0</pwg:ImagesCompleted>
      <pwg:ImagesToTransfer>0</pwg:ImagesToTransfer>
      <scan:TransferRetryCount>0</scan:TransferRetryCount>
      <pwg:JobState>Completed</pwg:JobState>
      <pwg:JobStateReasons>
        <pwg:JobStateReason>JobCompletedSuccessfully</pwg:JobStateReason>
      </pwg:JobStateReasons>
    </scan:JobInfo>
    <scan:JobInfo>
      <pwg:JobUri>/eSCL/ScanJobs/pjcqbcg8-1wkf-rxzr-1006-nno0gxg7</pwg:JobUri>
      <pwg:JobUuid>pjcqbcg8-1wkf-rxzr-1006-nno0gxg7</pwg:JobUuid>
      <scan:Age>1642</scan:Age>
      <pwg:ImagesCompleted>1</pwg:ImagesCompleted>
      <pwg:ImagesToTransfer>1</pwg:ImagesToTransfer>
      <scan:TransferRetryCount>0</scan:TransferRetryCount>
      <pwg:JobState>Aborted</pwg:JobState>
      <pwg:JobStateReasons>
        <pwg:JobStateReason>JobCanceledAtDevice</pwg:JobStateReason>
      </pwg:JobStateReasons>
    </scan:JobInfo>
  </scan:Jobs>
</scan:ScannerStatus>";
        
        [Fact]
        public async Task job_ready()
        {
            var responseXml = new XmlDocument();
            responseXml.LoadXml(JOB_READY_RESPONSE_BODY);
            var client = A.Fake<IEsclClient>();
            A.CallTo(() => client.GetAsync("http://192.168.0.151/eSCL/ScannerStatus"))
                .Returns(Task.FromResult<IEsclResponse>(new EsclResponse(content: responseXml)));

            var jobStatusChecker = new JobStatusChecker(client, "192.168.0.151",
                                                      "/eSCL/ScanJobs/pjcqbcg8-73aw-17j4-1007-yrbwnwyo");
            bool ready = await jobStatusChecker.Ready();

            Assert.True(ready);
        }

        [Fact]
        public async Task job_in_progress()
        {
            var responseXml = new XmlDocument();
            responseXml.LoadXml(JOB_IN_PROGRESS_RESPONSE_BODY);
            var client = A.Fake<IEsclClient>();
            A.CallTo(() => client.GetAsync("http://192.168.0.151/eSCL/ScannerStatus"))
                .Returns(Task.FromResult<IEsclResponse>(new EsclResponse(content: responseXml)));

            var jobStatusChecker = new JobStatusChecker(client, "192.168.0.151",
                                                      "/eSCL/ScanJobs/pjcqbcg8-73aw-17j4-1007-yrbwnwyo");
            bool ready = await jobStatusChecker.Ready();

            Assert.False(ready);
        }
    }
}
