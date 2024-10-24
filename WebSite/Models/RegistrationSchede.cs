using Shared.DTO;

namespace WebSite.Models
{
    public class RegistrationSchede
    {

        public EmployeeDTO Doctor { get; set; }
        public string FullName => Doctor.FirstName + " " + Doctor.MiddleName[0] + " " + Doctor.LastName[0];
    }
}
