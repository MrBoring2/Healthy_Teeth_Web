﻿using WebSite.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using Shared.DTO;
using Shared.Models;
using Radzen;

namespace WebSite.Services.ApiServices
{
    public class EmployeeApiService : IApiService<EmployeeDTO>
    {
        private readonly HttpClient _httpClient;
        public EmployeeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ResponseModel<IEnumerable<EmployeeDTO>>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/employees");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<IEnumerable<EmployeeDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<EmployeeDTO>>(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }


        public Task<ResponseModel<EmployeeDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<DataServiceResult<EmployeeDTO>>> GetAsync(Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.GetAsync($"api/employees?{queryString}");
                var responseObjects = await response.Content.ReadAsStringAsync();
                return new(response.StatusCode, JsonConvert.DeserializeObject<DataServiceResult<EmployeeDTO>>(responseObjects));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(System.Net.HttpStatusCode.BadRequest, null, "Не удалось получить данные");
            }
        }

        public async Task<ResponseModel<string>> PostAsync(object data)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await _httpClient.PostAsync("api/employees", new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, MediaTypeNames.Application.Json));
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
