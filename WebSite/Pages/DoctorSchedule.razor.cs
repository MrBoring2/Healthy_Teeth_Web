using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen.Blazor;
using Radzen;
using WebSite.Services.ApiServices;
using WebSite.Providers;
using System.Security.Claims;
using WebSite.Models;
using System.Collections.ObjectModel;
using Shared.Constants;

namespace WebSite.Pages
{
    public partial class DoctorSchedule
    {
        [Inject]
        private HubConnection HubConnection { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }
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
                LoadVisits(lastArgs);
                scheduler.Reload();
            });
        }
        private SchedulerLoadDataEventArgs lastArgs;
        private RadzenScheduler<Appointment> scheduler;
        private Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();

        private IList<Appointment> appointments = new List<Appointment>();
        private async Task LoadVisits(SchedulerLoadDataEventArgs args)
        {
            StateHasChanged();
            var user = await CustomStateProvider.GetAuthenticationStateAsync();

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
                        VisitStatusId = item.VisitStatusId,
                        VisitId = item.Id,
                        VisitPurpose = item.VisitPurpose,
                        PatientFullName = item.Patient.FullName,
                        Status = item.VisitStatus.Title
                    });
                }

            }
            lastArgs = args;
            appointments = new List<Appointment>(list);
            //StateHasChanged();

        }

        private void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour >= 8 && args.Start.Hour <= 20)
            {

                args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
            }
        }

        private async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<Appointment> args)
        {
            await DialogService.OpenAsync<VisitDetails>($"Посещение",
              new Dictionary<string, object>() { { "VisitId", args.Data.VisitId } },
              new DialogOptions()
              {
                  Resizable = true,
                  Draggable = true,

                  Width = "900px",
                  Height = "720px"
              });
        }
        private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<Appointment> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.VisitStatusId == (int)VisitStatuses.Waiting)
            {
                args.Attributes["style"] = "background-color: var(--rz-success-light)";
            }
        }
    }
}
