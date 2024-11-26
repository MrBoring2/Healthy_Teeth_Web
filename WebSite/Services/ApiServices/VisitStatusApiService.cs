using Newtonsoft.Json;
using Shared.DTO;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IVisitStatusApiService
    {
        Task<ResponseModel<IEnumerable<VisitStatusDTO>>> GetAsync();
    }
    public class VisitStatusApiService : IVisitStatusApiService
    {
        private readonly HttpClient _httpClient;

        public VisitStatusApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseModel<IEnumerable<VisitStatusDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/VisitStatus");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<VisitStatusDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }
    }
}
