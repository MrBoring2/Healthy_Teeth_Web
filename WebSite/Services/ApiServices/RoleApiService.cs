using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public class RoleApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public RoleApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/roles");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel(response.StatusCode, responseObjects);
            }
            catch (Exception ex)
            {
                return new ResponseModel(response.StatusCode, null);
            }
        }

        public Task<ResponseModel> GetAsync(string[] filterNames, string[] filterValues)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> PostAsync(object data)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> PutAsync(int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
