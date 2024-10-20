using Shared.DTO;

namespace WebSite.Services.ApiServices
{
    public interface IApiServiceFactory
    {
        IApiService<EmployeeDTO> GetEmployeeApiService();
        IApiService<ServiceDTO> GetServiceApiService();
        IApiService<RoleDTO> GetRoleApiService();
        IApiService<SpecializationDTO> GetSpecializationApiService();
    }
    public class ApiServiceFactory : IApiServiceFactory
    {
        private readonly HttpClient _httpClient;

        public ApiServiceFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IApiService<EmployeeDTO> GetEmployeeApiService()
        {
            return new EmployeeApiService(_httpClient);

        }
        public IApiService<ServiceDTO> GetServiceApiService()
        {
            return new ServiceApiService(_httpClient);
        }
        public IApiService<RoleDTO> GetRoleApiService()
        {
            return new RoleApiService(_httpClient);
        }
        public IApiService<SpecializationDTO> GetSpecializationApiService()
        {
            return new SpecializationApiService(_httpClient);
        }
    }
}
