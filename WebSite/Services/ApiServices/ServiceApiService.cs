using Newtonsoft.Json;
using Radzen;
using Shared.DTO;
using Shared.Models;
using System.Net.Mime;
using System.Text;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public class ServiceApiService : IApiService<ServiceDTO>
    {
        private readonly HttpClient _httpClient;

        public ServiceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataServiceResult<ServiceDTO>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/services");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DataServiceResult<ServiceDTO>>(responseObjects);
            }
            catch (Exception ex)
            {
                return new DataServiceResult<ServiceDTO>(null, 0);
            }
        }

        public Task<ResponseModel<ServiceDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DataServiceResult<ServiceDTO>> GetAsync(Dictionary<string, string> queryParameters)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> PostAsync(object data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync("api/services", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObjects);
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
