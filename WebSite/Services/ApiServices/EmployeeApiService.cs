using WebSite.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace WebSite.Services.ApiServices
{
    public class EmployeeApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public EmployeeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<ResponseModel> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/employees");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel(response.StatusCode, responseObjects);
            }
            catch (Exception ex)
            {
                return new ResponseModel(response.StatusCode, null);
            }
        }

        public async Task<ResponseModel> GetAsync(string[] filterNames, string[] filterValues)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var builder = new UriBuilder();
                for (int i = 0; i < filterNames.Length; i++)
                {
                    if (i == filterNames.Length - 1)
                    {
                        builder.Query += $"{filterNames[i]}={filterValues[i]}";
                    }
                    else
                    {
                        builder.Query += $"{filterNames[i]}={filterValues[i]}&";
                    }

                }
                response = await _httpClient.GetAsync(builder.ToString());
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel(response.StatusCode, responseObjects);
            }
            catch (Exception ex)
            {
                return new ResponseModel(response.StatusCode, null);
            }

        }

        public async Task<ResponseModel> PostAsync(object data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync("api/employees", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel(response.StatusCode, responseObjects);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel(response.StatusCode, ex.Message);
            }
        }

        public Task<ResponseModel> PutAsync(int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
