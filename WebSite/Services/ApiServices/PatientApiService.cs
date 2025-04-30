using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using System.Net.Mime;
using System.Text;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public interface IPatientApiService
    {
        Task<ResponseModel<IEnumerable<PatientDTO>>> GetAsync();
        Task<ResponseModel<DataServiceResult<PatientDTO>>> GetAsync(Dictionary<string, string> queryParameters);
        Task<ResponseModel<PatientDTO>> GetAsync(int id);
        Task<ResponseModel<string>> PostAsync(PatientDTO data);
        Task<ResponseModel<string>> PutAsync(int id, PatientDTO data);
        Task<ResponseModel<string>> DeleteAsync(int id);
    }
    public class PatientApiService : IPatientApiService
    {
        private readonly HttpClient _httpClient;
        public PatientApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseModel<IEnumerable<PatientDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/patients");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<PatientDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<PatientDTO>>(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<DataServiceResult<PatientDTO>>> GetAsync(Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters
              .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.GetAsync($"api/patients?{queryString}");
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<DataServiceResult<PatientDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<PatientDTO>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/patients/{id}");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return new ResponseModel<PatientDTO>(System.Net.HttpStatusCode.BadRequest, null, responseObjects);
                }

                return new(response.StatusCode, JsonConvert.DeserializeObject<PatientDTO>(responseObjects));
            }
            catch (Exception ex)
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel<PatientDTO>(System.Net.HttpStatusCode.BadRequest, null, ex.Message);
            }
        }

        public async Task<ResponseModel<string>> PostAsync(PatientDTO data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync("api/patients", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObject = await response.Content.ReadAsStringAsync();
                return new ResponseModel<string>(response.StatusCode, responseObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel<string>(response.StatusCode, ex.Message);
            }
        }

        public async Task<ResponseModel<string>> PutAsync(int id, PatientDTO data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PutAsync($"api/patients/{id}", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
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
                response = await _httpClient.DeleteAsync($"api/patients/{id}");
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
