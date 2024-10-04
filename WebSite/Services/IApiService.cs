using WebSite.Models;

namespace WebSite.Services
{
    public interface IApiService
    {
        Task<ResponseModel> GetAsync(string path);
        Task<ResponseModel> GetAsync(string path, int id);
        Task<ResponseModel> PostAsync(string path, object data);
        Task<ResponseModel> PutAsync(string path, int id, object data);

    }
}
