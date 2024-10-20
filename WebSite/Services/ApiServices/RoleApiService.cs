using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public class RoleApiService : IApiService<RoleDTO>
    {
        private readonly HttpClient _httpClient;

        public RoleApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataServiceResult<RoleDTO>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/roles");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DataServiceResult<RoleDTO>>(responseObjects);
            }
            catch (Exception ex)
            {
                return new DataServiceResult<RoleDTO>(null, 0);
            }
        }

        public Task<ResponseModel<RoleDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataServiceResult<RoleDTO>> GetAsync(Dictionary<string, string> queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<string>> PostAsync(object data)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<string>> PutAsync(int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
