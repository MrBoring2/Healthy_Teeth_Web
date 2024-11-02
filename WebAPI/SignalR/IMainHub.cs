namespace WebAPI.SignalR
{
    public interface IMainHub
    {
        Task ServicesChanged(string mes);
        Task EmployeesChagned(string mes);
        Task VisitsChanged(string mes);
        Task PatientsChanged(string mes);
    }
}
