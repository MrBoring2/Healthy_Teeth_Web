using WebSite.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace WebSite.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public Task<ResponseModel> GetAsync(string path, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> GetAsync(string path)
        {
            // var client = _httpClientFactory.CreateClient("API");
            var response = await _httpClient.GetAsync(path);
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

        public async Task<ResponseModel> PostAsync(string path, object data)
        {
            //var msg = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri($"{path}", UriKind.RelativeOrAbsolute),
            //    Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json)
            //};
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new ResponseModel(response.StatusCode, responseObjects);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
                return new ResponseModel(response.StatusCode, ex.Message);
            }
        }

        public Task<ResponseModel> PutAsync(string path, int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
