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
        [Inject]
        private NotificationService NotificationService { get; set; }
        private TimeOnly timeFrom;
        private TimeOnly timeTo;
        public TimeOnly TimeFrom
        {
            get => timeFrom;
            set
            {
                if (value < TimeTo)
                {
                    timeFrom = value;
                }
                else
                {
                    timeFrom = timeFrom;
                }
            }
        }
        public TimeOnly TimeTo
        {
            get => timeTo;
            set
            {
                if (value > TimeFrom)
                {
                    timeTo = value;
                }
                else
                {
                    timeTo = timeTo;
                }
            }
        }

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
                new Weekday(0, "Воскресенье")
            };
            timeFrom = new TimeOnly(8,0);
            timeTo = new TimeOnly(14,0);
        }

        public async Task OnSubmit(ScheduleDTO schedule)
        {
            schedule.TimeFrom = TimeFrom;
            schedule.TimeTo = TimeTo;
            DialogService.Close(schedule);
        }
        public async Task OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Duration = 2000,
                Summary = "Оповещение",
                Detail = "Неверно заполнены данные"
            });
        }
    }
}
