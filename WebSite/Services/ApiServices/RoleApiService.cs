using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IRoleApiService
    {
        Task<ResponseModel<IEnumerable<RoleDTO>>> GetAsync();
    }
    public class RoleApiService : IRoleApiService
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
      
    }
}
