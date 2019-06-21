using System;

namespace EsclScanner
{
    public interface IEsclResponse
    {
         bool IsSuccessStatusCode {get;}
         string Content {get;}
    }
}