using Newtonsoft.Json;
using Radzen;
using Shared.DTO;
using Shared.Models;
using System.Net;
using System.Net.Mime;
using System.Text;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IServiceApiService
    {
        Task<ResponseModel<IEnumerable<ServiceDTO>>> GetAsync();
        Task<ResponseModel<IEnumerable<ServiceDTO>>> GetForSpesializationAsync(int specializationId);
        Task<ResponseModel<DataServiceResult<ServiceDTO>>> GetAsync(Dictionary<string, string> queryParameters);
        Task<ResponseModel<ServiceDTO>> GetAsync(int id);
        Task<ResponseModel<string>> DeleteAsync(int id);
        Task<ResponseModel<string>> PostAsync(ServiceDTO data);
        Task<ResponseModel<string>> PutAsync(int id, ServiceDTO data);
    }
    public class ServiceApiService : IServiceApiService
    {
        private readonly HttpClient _httpClient;

        public ServiceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseModel<IEnumerable<ServiceDTO>>> GetForSpesializationAsync(int specializationId)
        {
            var response = await _httpClient.GetAsync($"api/Services/GetForSpecialization/{specializationId}");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<ServiceDTO>>(responseObjects), "Не удалось получить данные");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }
        public async Task<ResponseModel<IEnumerable<ServiceDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/services");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<ServiceDTO>>(responseObjects), "Не удалось получить данные");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<ServiceDTO>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/services/{id}");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return new ResponseModel<ServiceDTO>(System.Net.HttpStatusCode.BadRequest, null, responseObjects);
                }

                return new(response.StatusCode, JsonConvert.DeserializeObject<ServiceDTO>(responseObjects));
            }
            catch (Exception ex)
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel<ServiceDTO>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }

        public async Task<ResponseModel<DataServiceResult<ServiceDTO>>> GetAsync(Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters
              .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.GetAsync($"api/services?{queryString}");
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<DataServiceResult<ServiceDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<string>> PostAsync(ServiceDTO data)
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

        public async Task<ResponseModel<string>> PutAsync(int id, ServiceDTO data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PutAsync($"api/services/{id}", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(response.StatusCode, ex.Message);
            }
        }
        public async Task<ResponseModel<string>> DeleteAsync(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.DeleteAsync($"api/services/{id}");
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
