using System;
using System.Xml;

namespace Escl.Connection
{
    public struct EsclResponse : IEsclResponse
    {
        public EsclResponse(XmlDocument content = null, Uri location = null)
        {
            this.Content = content;
            this.Location = location;
        }

        public XmlDocument Content {get; }

        public Uri Location {get; }
        
    }
}