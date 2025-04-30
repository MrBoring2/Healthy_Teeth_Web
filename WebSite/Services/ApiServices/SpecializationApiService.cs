using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface ISpecializationApiService
    {
        Task<ResponseModel<IEnumerable<SpecializationDTO>>> GetAsync();
    }
    public class SpecializationApiService : ISpecializationApiService
    {
        private readonly HttpClient _httpClient;

        public SpecializationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<IEnumerable<SpecializationDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/specializations");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<SpecializationDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }
    }
}
