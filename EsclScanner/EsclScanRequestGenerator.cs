namespace EsclScanner
{
    public class EsclScanRequestGenerator
    {
        public string Generate()
        {
            return @"<?xml version='1.0' encoding='utf-8'?>
<escl:ScanSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
    xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"" 
    xmlns:escl=""http://schemas.hp.com/imaging/escl/2011/05/03"">
    <pwg:Version>2.63</pwg:Version>
    <pwg:ScanRegions pwg:MustHonor=""false"">
        <pwg:ScanRegion>
            <pwg:ContentRegionUnits>escl:ThreeHundredthsOfInches</pwg:ContentRegionUnits>
            <pwg:XOffset>0</pwg:XOffset>
            <pwg:YOffset>0</pwg:YOffset>
            <pwg:Width>2550</pwg:Width>
            <pwg:Height>3508</pwg:Height>
        </pwg:ScanRegion>
    </pwg:ScanRegions>
    <escl:DocumentFormatExt>application/pdf</escl:DocumentFormatExt>
    <pwg:InputSource>Platen</pwg:InputSource>
    <escl:XResolution>200</escl:XResolution>
    <escl:YResolution>200</escl:YResolution>
    <escl:ColorMode>RGB24</escl:ColorMode>
</escl:ScanSettings>";
        }
    }
}