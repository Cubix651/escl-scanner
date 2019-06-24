namespace Escl.Requests
{
    public class EsclScanRequestGenerator
    {
        public string Generate(ScanOptions options)
        {
            return $@"<?xml version='1.0' encoding='utf-8'?>
<escl:ScanSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" 
    xmlns:escl=""http://schemas.hp.com/imaging/escl/2011/05/03"">
    <pwg:Version>2.63</pwg:Version>
    <pwg:ScanRegions pwg:MustHonor=""false"">
        <pwg:ScanRegion>
            <pwg:ContentRegionUnits>escl:ThreeHundredthsOfInches</pwg:ContentRegionUnits>
            <pwg:XOffset>{options.XOffset}</pwg:XOffset>
            <pwg:YOffset>{options.YOffset}</pwg:YOffset>
            <pwg:Width>{options.Width}</pwg:Width>
            <pwg:Height>{options.Height}</pwg:Height>
        </pwg:ScanRegion>
    </pwg:ScanRegions>
    <escl:DocumentFormatExt>{options.DocumentFormatExt}</escl:DocumentFormatExt>
    <pwg:InputSource>Platen</pwg:InputSource>
    <escl:XResolution>{options.Resolution.X}</escl:XResolution>
    <escl:YResolution>{options.Resolution.Y}</escl:YResolution>
    <escl:ColorMode>RGB24</escl:ColorMode>
</escl:ScanSettings>";
        }
    }
}