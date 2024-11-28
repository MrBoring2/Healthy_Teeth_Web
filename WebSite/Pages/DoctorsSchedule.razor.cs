using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using Radzen.Blazor;
using Shared.Constants;
using Shared.DTO;
using Shared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using WebSite.Models;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class DoctorsSchedule
    {
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private ContextMenuService ContextMenuService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]
        private IEmployeeApiService EmployeeApiService { get; set; }
        [Inject]
        private IVisitApiService VisitApiService { get; set; }
        [Inject]
        private ISpecializationApiService SpeciazaizationApiService { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        public List<Dictionary<string, ScheduleRegister>> Data { get; set; }
        private string dispalyedDate;
        public string DisplayedDate
        {
            get => dispalyedDate;
            set
            {
                dispalyedDate = value;
                StateHasChanged();
            }
        }
        private SpecializationDTO selectedSpecialization;

        private IList<EmployeeDTO> selectedEmployees;
        public List<SpecializationDTO> Specializations { get; set; }
        public List<EmployeeDTO> DisplayedEmployees { get; set; }
        public List<EmployeeDTO> Employees { get; set; }
        private bool isLoading;
        private LoadDataArgs lastArgs;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                StateHasChanged();
            }
        }
        public DateOnly SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                DisplayedDate = SelectedDate.ToShortDateString() + ", " + CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(SelectedDate.DayOfWeek);
                LoadTable(lastArgs);
            }
        }
        private DateOnly selectedDate;
        public SpecializationDTO SelectedSpecialization
        {
            get => selectedSpecialization;
            set
            {
                selectedSpecialization = value;
                LoadTable(lastArgs);
            }
        }
        public IList<EmployeeDTO> SelectedEmployees
        {
            get => selectedEmployees;
            set
            {
                selectedEmployees = value;
                GenerateTable(SelectedEmployees.ToList());
                StateHasChanged();
            }
        }
        public IDictionary<string, object> Columns { get; set; }
        public IList<Tuple<Dictionary<string, ScheduleRegister>, RadzenDataGridColumn<Dictionary<string, ScheduleRegister>>>> selectedCellData = new List<Tuple<Dictionary<string, ScheduleRegister>, RadzenDataGridColumn<Dictionary<string, ScheduleRegister>>>>();
        protected override async Task OnInitializedAsync()
        {
            HubConnection.On<string>("VisitsChanged", async mes =>
            {
                await LoadTable(lastArgs);
            });
            selectedDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DisplayedDate = SelectedDate.ToShortDateString() + ", " + CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(SelectedDate.DayOfWeek);
            await LoadSpecializations();
        }

        private async Task LoadTable(LoadDataArgs args)
        {
            lastArgs = args;
            IsLoading = true;
            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("date", SelectedDate.ToShortDateString());
            queryParameters.Add("specializationId", SelectedSpecialization == null ? "2" : SelectedSpecialization.Id.ToString());
            var response = await EmployeeApiService.GetForScheduleAsync(queryParameters);
            var tempCount = Employees?.Count;
            Employees = response.Content.ToList();

            Console.WriteLine("Выбрано:" + SelectedEmployees?.Count);
            if (SelectedEmployees == null || SelectedEmployees?.Count == 0 || Employees.Count != tempCount)
            {
                await GenerateTable(Employees);
            }
            else
            {
                if (SelectedEmployees != null)
                {
                    selectedEmployees = Employees.Where(p => SelectedEmployees.Select(e => e.Id).Contains(p.Id)).ToList();
                }
                await GenerateTable(SelectedEmployees.ToList());

            }
        }

        private async Task GenerateTable(List<EmployeeDTO> employees)
        {
            if (employees.Count == 0)
                employees = Employees;


            await Task.Run(() =>
            {
                Columns = new Dictionary<string, object>();
                if (Columns.Count == 0)
                    Columns.Add("Время", "Время");
                foreach (var item in employees)
                {
                    Columns.Add(item.Id.ToString(), item.FullName);
                }
                var temp = new List<Dictionary<string, ScheduleRegister>>();

                for (var time = new TimeOnly(8, 0, 0, 0); time < new TimeOnly(20, 0, 0); time = time.AddMinutes(30))
                {
                    var row = new Dictionary<string, ScheduleRegister>();
                    row.Add(Columns.Keys.First(), new ScheduleRegister(0, null, null, null, time)); ;
                    int k = 0;
                    foreach (KeyValuePair<string, object> column in Columns)
                    {
                        if (k == 0)
                        {
                            k++;
                            continue;
                        }
                        var employee = employees.FirstOrDefault(p => p.Id == Convert.ToInt32(column.Key));
                        var employeeSchedule = employee.Schedules.FirstOrDefault(p => p.Weekday == (int)SelectedDate.DayOfWeek);
                        Console.WriteLine(employee.Visits?.Count());
                        row.Add(column.Key.ToString(),
                            new ScheduleRegister(employee.Id,
                                                       employeeSchedule?.TimeFrom,
                                                       employeeSchedule?.TimeTo,
                                                       time,
                                                       employee.Visits?.FirstOrDefault(p => p.VisitDate.DayOfWeek == SelectedDate.DayOfWeek && p.VisirtTime == time)));
                        k++;
                    }

                    temp.Add(row);
                }
                Data = temp;
                
                IsLoading = false;
                StateHasChanged();
            });

        }

        public async Task LoadSpecializations()
        {
            var response = await SpeciazaizationApiService.GetAsync();
            Specializations = response.Content.ToList();
            Specializations.Remove(Specializations.FirstOrDefault(p => p.Id == 1));
            selectedSpecialization = Specializations.FirstOrDefault();
            StateHasChanged();
        }
        async void OnCellDoubleClick(DataGridCellMouseEventArgs<Dictionary<string, ScheduleRegister>> args)
        {
            selectedCellData.Clear();

            var cellData = selectedCellData.FirstOrDefault(i => i.Item1 == args.Data && i.Item2 == args.Column);
            if (cellData != null)
            {
                selectedCellData.Remove(cellData);
            }
            else
            {
                selectedCellData.Add(new Tuple<Dictionary<string, ScheduleRegister>, RadzenDataGridColumn<Dictionary<string, ScheduleRegister>>>(args.Data, args.Column));
            }

            var a = selectedCellData?.FirstOrDefault()?.Item2;
            var c = args.Column.UniqueID;
            // Console.WriteLine(c);
            var d = args.Data[c];

            if (d.StartTime <= d.TargetTime && d.EndTime.Value.AddMinutes(-30) >= d.TargetTime)
            {
                if (d.Data?.GetType() == typeof(VisitDTO))
                {

                }
                else
                {
                    await OpenVisitWindow(d);
                }
            }


        }

        public async Task OpenVisitWindow(ScheduleRegister schedule)
        {
            Console.WriteLine(schedule.EmployeeId);
            await DialogService.OpenAsync<AddVisit>($"Добавление",
               new Dictionary<string, object>() { { "EmployeeId", schedule.EmployeeId }, { "VisitDate", SelectedDate }, { "VisitTime", schedule.TargetTime } },
               new DialogOptions()
               {
                   Resizable = true,
                   Draggable = true,

                   Width = "1000px",
                   Height = "720px"
               });

        }

        public async Task ChangeVisitStatus(int id, VisitStatuses visitStatus)
        {
            string text = "";
            if (visitStatus == VisitStatuses.Waiting)
                text = "Ожидание";
            else if (visitStatus == VisitStatuses.NotCome)
                text = "Не пришёл";
            else if (visitStatus == VisitStatuses.Canceled)
                text = "Отменена";


            var confirm = await DialogService.Confirm($"Изменить статус на '{text}'?", "Подтверждение", new ConfirmOptions() { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirm == true)
            {
                var visitStatusVM = new VisitStatusChangeViewModel(id, (int)visitStatus);
                var response = await VisitApiService.ChangeVisitStatusAsync(visitStatusVM);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = "Запись успешно обновлена"
                    });
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = response.Content
                    });
                }

            }
        }

        private void OnCellContextMenu(DataGridCellMouseEventArgs<Dictionary<string, ScheduleRegister>> args)
        {
            var c = args.Column.UniqueID;
            var d = args.Data[c];

            if (d.Data == null)
                return;

            ContextMenuService.Open(args,
            new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = "Подтвердить запись", Value = 2, Icon = "check" },
                new ContextMenuItem(){ Text = "Отменить запись", Value = 5, Icon = "block" },
                new ContextMenuItem(){ Text = "Пометить запись как 'Не пришёл'", Value = 4, Icon = "person_remove" },
            },
            async (e) =>
            {
                await ChangeVisitStatus((d.Data as VisitDTO).Id, (VisitStatuses)e.Value);
                //console.Log($"Menu item with Value={e.Value} clicked. Column: {args.Column.Property}, EmployeeID: {args.Data.EmployeeID}");
            }
         );
        }

        private void OnCellRender(DataGridCellRenderEventArgs<Dictionary<string, ScheduleRegister>> args)
        {
            var c = args.Column.UniqueID;
            var d = args.Data[c];
            if (args.Column.Title == "Время")
            {
                args.Column.Width = "55px";
            }
            if (args.Column.UniqueID == c)
            {
                if (d.Data != null && d.Data.GetType() == typeof(VisitDTO))
                {
                    var visit = d.Data as VisitDTO;
                    if (visit.VisitStatusId == (int)VisitStatuses.Waiting)
                    {
                        args.Attributes.Add("style", $"background-color: var(--rz-info-light)");
                    }
                    else if (visit.VisitStatusId == (int)VisitStatuses.Written)
                    {
                        args.Attributes.Add("style", $"background-color: var(--rz-warning-light)");
                    }
                    else if (visit.VisitStatusId == (int)VisitStatuses.Canceled)
                    {
                        args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");
                    }
                    else if (visit.VisitStatusId == (int)VisitStatuses.Compleated)
                    {
                        args.Attributes.Add("style", $"background-color: var(--rz-success-light)");

                    }
                    else if (visit.VisitStatusId == (int)VisitStatuses.NotCome)
                    {
                        args.Attributes.Add("style", $"background-color: var(--rz-danger-light)");

                    }

                }
                else if (d.Data == null && (d.StartTime == null || d.EndTime == null || d.StartTime > d.TargetTime || d.EndTime < d.TargetTime))
                {
                    //args.Attributes.Add("style", $"background-color: var(--rz-base-300)");
                }
                else if (d.StartTime <= d.TargetTime && d.EndTime.Value.AddMinutes(-30) >= d.TargetTime)
                {
                    //args.Attributes.Add("style", $"background-color: {(d.StartTime <= d.TargetTime && d.EndTime >= d.TargetTime ? "var(--rz-info-light)" : "var(--rz-base-background-color)")};");
                    args.Attributes.Add("style", $"background-color: var(--rz-secondary-lighter)");

                }
            }
        }
    }
}
