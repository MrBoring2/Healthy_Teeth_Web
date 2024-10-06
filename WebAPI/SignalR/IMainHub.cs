namespace WebAPI.SignalR
{
    public interface IMainHub
    {
        Task ServiceAdded(string mes);
        Task ServiceUpdated(string mes);
        Task EmployeeAdded(string mes);
        Task EmployeeUpdated(string mes);
    }
}
