using Newtonsoft.Json;
using Shared.DTO;
using Shared.Models;
using WebSite.Models;

namespace WebSite.Services.ApiServices
{
    public class SpecializationApiService : IApiService<SpecializationDTO>
    {
        private readonly HttpClient _httpClient;

        public SpecializationApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DataServiceResult<SpecializationDTO>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/specializations");
            try
            {
                var responseObjects = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DataServiceResult<SpecializationDTO>>(responseObjects);
            }
            catch (Exception ex)
            {
                return new DataServiceResult<SpecializationDTO>(null, 0);
            }
        }

        public Task<DataServiceResult<SpecializationDTO>> GetAsync(Dictionary<string, string> queryParameters)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<SpecializationDTO>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<string>> PostAsync(object data)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<string>> PutAsync(int id, object data)
        {
            throw new NotImplementedException();
        }
    }
}
