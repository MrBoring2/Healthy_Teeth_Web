using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.DTO;
using Shared.Models;
using WebSite.Models;

namespace WebSite.Pages
{
    public partial class AddSchedule
    {
        [Parameter]
        public int ScheduleId { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        protected ScheduleDTO schedule = new();
        private List<Weekday> Weekdays { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Weekdays = new List<Weekday>
            {
                new Weekday(1, "Понедельник"),
                new Weekday(2, "Вторник"),
                new Weekday(3, "Среда"),
                new Weekday(4, "Четвер"),
                new Weekday(5, "Пятница"),
                new Weekday(6, "Суббота"),
                new Weekday(7, "Воскресенье")
            };
        }

        public async Task SaveSchedule()
        {
            DialogService.Close(schedule);
        }
    }
}
