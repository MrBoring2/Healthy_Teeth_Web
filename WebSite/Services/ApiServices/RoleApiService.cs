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

        public async Task<ResponseModel<IEnumerable<RoleDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/roles");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new (response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<RoleDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public Task<ResponseModel<RoleDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<DataServiceResult<RoleDTO>>> GetAsync(Dictionary<string, string> queryParameters)
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
