using System.Xml;

namespace Escl.Utils
{
    public static class NamespaceUtils
    {
        public static readonly string SCAN_NAMESPACE = "http://schemas.hp.com/imaging/escl/2011/05/03";
        public static readonly string PWG_NAMESPACE = "http://www.pwg.org/schemas/2010/12/sm";

        public static XmlNamespaceManager CreateNamespaceManager(XmlDocument xmlDocument)
        {
            var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("scan", SCAN_NAMESPACE);
            namespaceManager.AddNamespace("pwg", PWG_NAMESPACE);
            return namespaceManager;
        }
    }
}