using AutoMapper;
using Entities;
using Shared.DTO;
using Shared.Models;

namespace WebAPI.Services
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() 
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Service, ServiceDTO>().ReverseMap();
            CreateMap<Specialization, SpecializationDTO>().ReverseMap();
            CreateMap<Service, ServiceViewModel>();
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(p => p.RoleId, op => op.MapFrom(s => s.Account!.RoleId))
                .ForMember(p => p.Login, op => op.MapFrom(s => s.Account!.Login));
        }
    }
}
