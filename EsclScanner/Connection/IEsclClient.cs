using System.IO;
using System.Threading.Tasks;

namespace Escl.Connection
{
    public interface IEsclClient
    {
        Task<IEsclResponse> GetAsync(string uri);
        Task<Stream> GetStreamAsync(string uri);
        Task<IEsclResponse> PostAsync(string uri, string body);
    }
}