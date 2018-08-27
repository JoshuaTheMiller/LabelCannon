using System.Threading.Tasks;

namespace Service
{
    public interface IWebClient
    {
        Task<TResponse> PostAsync<TBody, TResponse>(string url, TBody body);
    }
}
