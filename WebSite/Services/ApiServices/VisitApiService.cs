using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using System.Net.Mime;
using System.Text;
using WebSite.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebSite.Services.ApiServices
{
    public interface IVisitApiService
    {
        Task<ResponseModel<VisitDTO>> GetAsync(int id);
        Task<ResponseModel<DataServiceResult<VisitDTO>>> GetAsync(Dictionary<string, string> queryParameters);
        Task<ResponseModel<IEnumerable<VisitDTO>>> GetAsync(int doctorId, DateOnly startDate, DateOnly endDate);
        Task<ResponseModel<string>> PostAsync(VisitDTO data);
        Task<ResponseModel<string>> PutAsync(int id, VisitDTO data);
        Task<ResponseModel<string>> ChangeVisitStatusAsync(VisitStatusChangeViewModel data);
        Task<ResponseModel<string>> DeleteAsync(int id);
    }
    public class VisitApiService : IVisitApiService
    {
        private readonly HttpClient _httpClient;

        public VisitApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseModel<VisitDTO>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/visits/{id}");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return new ResponseModel<VisitDTO>(System.Net.HttpStatusCode.BadRequest, null, responseObjects);
                }

                return new(response.StatusCode, JsonConvert.DeserializeObject<VisitDTO>(responseObjects));
            }
            catch (Exception ex)
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel<VisitDTO>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }
        public async Task<ResponseModel<DataServiceResult<VisitDTO>>> GetAsync(Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters
              .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.GetAsync($"api/visits?{queryString}");
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<DataServiceResult<VisitDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }
        public async Task<ResponseModel<IEnumerable<VisitDTO>>> GetAsync(int doctorId, DateOnly startDate, DateOnly endDate)
        {
            var queryString = $"doctorId={doctorId}&startDate={startDate}&endDate={endDate}";
            var response = await _httpClient.GetAsync($"api/visits/GetForDoctor?{queryString}");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<VisitDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<string>> PostAsync(VisitDTO data)
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

        public async Task<ResponseModel<string>> PutAsync(int id, VisitDTO data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PutAsync($"api/visits/{id}", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
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
                response = await _httpClient.DeleteAsync($"api/visits/{id}");
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public async Task<ResponseModel<string>> ChangeVisitStatusAsync(VisitStatusChangeViewModel data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync($"api/visits/ChangeStatus", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(response.StatusCode, ex.Message);
            }
           
        }
    }
}
