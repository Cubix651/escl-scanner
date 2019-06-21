using System;
using System.Threading.Tasks;

namespace EsclScanner
{
    public interface IEsclClient
    {
        Task<IEsclResponse> GetAsync(string uri);
    }
}