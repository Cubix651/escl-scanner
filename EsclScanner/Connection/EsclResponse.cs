using System;
using System.Xml;

namespace Escl.Connection
{
    public struct EsclResponse : IEsclResponse
    {
        public EsclResponse(XmlDocument content, Uri location)
        {
            this.Content = content;
            this.Location = location;
        }
        public EsclResponse(XmlDocument content) : this (content, null)
        { }

        public XmlDocument Content {get; }

        public Uri Location {get; }
        
    }
}