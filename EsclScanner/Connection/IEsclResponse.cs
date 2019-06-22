using System;
using System.Xml;

namespace Escl.Connection
{
    public interface IEsclResponse
    {
         XmlDocument Content {get;}
         Uri Location {get;}
    }
}