using Shared.Models;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IApiService<T> where T : class
    {
        Task<DataServiceResult<T>> GetAsync();
        Task<DataServiceResult<T>> GetAsync(Dictionary<string, string> queryParameters);
        Task<ResponseModel<T>> GetAsync(int id);
        Task<ResponseModel<string>> PostAsync(object data);
        Task<ResponseModel<string>> PutAsync(int id, object data);

    }
}
