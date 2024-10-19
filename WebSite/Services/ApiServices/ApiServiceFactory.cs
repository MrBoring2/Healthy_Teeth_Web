namespace WebSite.Services.ApiServices
{
    public interface IApiServiceFactory
    {
        IApiService GetEmployeeApiService();
        IApiService GetServiceApiService();
        IApiService GetRoleApiService();
        IApiService GetSpecificationApiService();
    }
    public class ApiServiceFactory : IApiServiceFactory
    {
        private readonly HttpClient _httpClient;

        public ApiServiceFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IApiService GetEmployeeApiService()
        {
            return new EmployeeApiService(_httpClient);

        }
        public IApiService GetServiceApiService()
        {
            return new ServiceApiService(_httpClient);
        }
        public IApiService GetRoleApiService()
        {
            return new RoleApiService(_httpClient);
        }
        public IApiService GetSpecificationApiService()
        {
            return new SpecializationApiService(_httpClient);
        }
    }
}
