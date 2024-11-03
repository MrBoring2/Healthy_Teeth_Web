namespace WebAPI.SignalR
{
    public interface IMainHub
    {
        Task ServicesChanged(string mes);
        Task EmployeesChanged(string mes);
        Task VisitsChanged(string mes);
        Task PatientsChanged(string mes);
    }
}
