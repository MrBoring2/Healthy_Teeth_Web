using Shared.DTO;

namespace WebSite.Models
{
    public class ServiceInVisit
    {
        public ServiceDTO Service { get; set; }
        public string Title => Service == null ? "" : Service.Title;
        private int quantity;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value > 0 ? value : 1;
            }
        }
    }
}
