using System;

namespace Escl.Connection
{
    public interface IEsclResponse
    {
         bool IsSuccessStatusCode {get;}
         string Content {get;}
         Uri LocationHeader {get;}
    }
}