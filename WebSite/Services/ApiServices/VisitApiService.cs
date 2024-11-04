using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using System.Net.Mime;
using System.Text;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IVisitApiService
    {
        Task<ResponseModel<DataServiceResult<PatientDTO>>> GetAsync(Dictionary<string, string> queryParameters);
        Task<ResponseModel<ServiceDTO>> GetAsync(int id);
        Task<ResponseModel<string>> PostAsync(object data);
        Task<ResponseModel<string>> PutAsync(int id, object data);
    }
    public class VisitApiService : IVisitApiService
    {
        private readonly HttpClient _httpClient;

        public VisitApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<ResponseModel<DataServiceResult<PatientDTO>>> GetAsync(Dictionary<string, string> queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<ServiceDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> PostAsync(object data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync("api/visits", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(response.StatusCode, ex.Message);
            }
        }

        public Task<ResponseModel<string>> PutAsync(int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
