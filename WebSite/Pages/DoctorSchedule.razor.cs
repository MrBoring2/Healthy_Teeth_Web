using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen.Blazor;
using Radzen;
using WebSite.Services.ApiServices;
using WebSite.Providers;
using System.Security.Claims;
using WebSite.Models;
using System.Collections.ObjectModel;

namespace WebSite.Pages
{
    public partial class DoctorSchedule
    {
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private IVisitApiService VisitApiService { get; set; }
        [Inject]
        private CustomStateProvider CustomStateProvider { get; set; }
        private TimeSpan StartTime { get; set; } = new TimeSpan(8, 0, 0);
        private TimeSpan EndTime { get; set; } = new TimeSpan(20, 0, 0);
        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<string>("VisitsChanged", async mes =>
            {
                Console.WriteLine("Пришло сообщение");
            });
        }
        private LoadDataArgs lastArgs;
        RadzenScheduler<Appointment> scheduler;
        //EventConsole console;
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        private IList<Appointment> appointments = new List<Appointment>();
        //{
        //new Appointment { Start = DateTime.Today.AddDays(-2), End = DateTime.Today.AddDays(-2), Text = "Birthday" },
        //new Appointment { Start = DateTime.Today.AddDays(-11), End = DateTime.Today.AddDays(-10), Text = "Day off" },
        //new Appointment { Start = DateTime.Today.AddDays(-10), End = DateTime.Today.AddDays(-8), Text = "Work from home" },
        //new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(12), Text = "Online meeting" },
        //new Appointment { Start = DateTime.Today.AddHours(10), End = DateTime.Today.AddHours(13), Text = "Skype call" },
        //new Appointment { Start = DateTime.Today.AddHours(14), End = DateTime.Today.AddHours(14).AddMinutes(30), Text = "Dentist appointment" },
        //new Appointment { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(12), Text = "Vacation" },
        //};

        private async Task LoadVisits(SchedulerLoadDataEventArgs args)
        {
            StateHasChanged();
            var user = await CustomStateProvider.GetAuthenticationStateAsync();
            foreach (var item in user.User.Claims)
            {
                Console.WriteLine(item.Value);
            }
            var response = await VisitApiService.GetAsync(int.Parse(user.User.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value),
                                                        DateOnly.FromDateTime(args.Start),
                                                        DateOnly.FromDateTime(args.End));
            var list = new List<Appointment>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = response.Content;
                foreach (var item in result)
                {
                    list.Add(new Appointment
                    {
                        Start = new DateTime(item.VisitDate, item.VisirtTime),
                        End = new DateTime(item.VisitDate, item.VisirtTime.AddMinutes(30)),
                        Text = $"{item.Patient.FullName}\n{item.VisitPurpose}"
                    });
                }
                
            }
            appointments = new List<Appointment>(list);
            //StateHasChanged();
            
        }

        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            //scheduler.
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour >= 8 && args.Start.Hour <= 20)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }
        }

        async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {

        }

        async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<Appointment> args)
        {


            
        }

        void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<Appointment> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.Text == "Birthday")
            {
                args.Attributes["style"] = "background: red";
            }
        }

        async Task OnAppointmentMove(SchedulerAppointmentMoveEventArgs args)
        {
            var draggedAppointment = appointments.FirstOrDefault(x => x == args.Appointment.Data);

            if (draggedAppointment != null)
            {
                draggedAppointment.Start = draggedAppointment.Start + args.TimeSpan;

                draggedAppointment.End = draggedAppointment.End + args.TimeSpan;

                await scheduler.Reload();
            }
        }
    }
}
