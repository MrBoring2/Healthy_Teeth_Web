using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IApiService
    {
        Task<ResponseModel> GetAsync();
        Task<ResponseModel> GetAsync(string[] filterNames, string[] filterValues);
        Task<ResponseModel> GetAsync(int id);
        Task<ResponseModel> PostAsync(object data);
        Task<ResponseModel> PutAsync(int id, object data);

    }
}
