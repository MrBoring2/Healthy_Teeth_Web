using Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using WebSite.Models;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class TestVisits
    {
        [Inject]
        private IEmployeeApiService EmployeeApiService { get; set; }
        private DateOnly CurrentDate { get; set; }
        public List<Dictionary<string, ScheduleRegister>> data { get; set; }
        public IDictionary<string, object> columns { get; set; }
        public IList<Tuple<Dictionary<string, ScheduleRegister>, RadzenDataGridColumn<Dictionary<string, ScheduleRegister>>>> selectedCellData = new List<Tuple<Dictionary<string, ScheduleRegister>, RadzenDataGridColumn<Dictionary<string, ScheduleRegister>>>>();
        protected override async Task OnInitializedAsync()
        {
            CurrentDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            await LoadDoctors();
        }

        private async Task LoadDoctors()
        {
            data = new List<Dictionary<string, ScheduleRegister>>();
            columns = new Dictionary<string, object>();
            var queryParameters = new Dictionary<string, string>();
            queryParameters.Add("date", new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToShortDateString());
            queryParameters.Add("specializationId", "2");
            var response = await EmployeeApiService.GetForScheduleAsync(queryParameters);
            var list = response.Content;
            columns.Add("Время", "Время");
            foreach (var item in list)
            {
                columns.Add(item.Id.ToString(), item.FullName);
            }
            TimeOnly time = new TimeOnly(8, 0, 0, 0);
            for (int i = 0; i < (20 - 8) * 2; i++)
            {
                var row = new Dictionary<string, ScheduleRegister>();
                row.Add(columns.Keys.First(), new ScheduleRegister(0, null, null, null, time)); ;
                int k = 0;
                foreach (KeyValuePair<string, object> column in columns)
                {
                    if (k == 0)
                    {
                        k++;
                        continue;
                    }
                    var employee = list.FirstOrDefault(p => p.Id == Convert.ToInt32(column.Key));
                    var employeeSchedule = employee.Schedules.FirstOrDefault(p => p.Weekday == (int)CurrentDate.DayOfWeek);
                    row.Add(column.Key.ToString(),
                        new ScheduleRegister(employee.Id,
                                                   employeeSchedule?.TimeFrom,
                                                   employeeSchedule?.TimeTo,
                                                   time,
                                                   employee.Visits.FirstOrDefault(p => p.VisirtTime == time)));
                    k++;
                }

                time = time.AddMinutes(30);
                data.Add(row);
            }
            StateHasChanged();
        }
        public async Task OnCellClick(DataGridCellMouseEventArgs<Dictionary<string, ScheduleRegister>> args)
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
            var a = selectedCellData.First().Item2;
            var c = args.Column.UniqueID;
            var d = args.Data[c];
            Console.WriteLine(d.Value);
        }


        void OnCellRender(DataGridCellRenderEventArgs<Dictionary<string, ScheduleRegister>> args)
        {
            var c = args.Column.UniqueID;
            var d = args.Data[c];
            if (args.Column.Title == "Время")
            {
                args.Column.Width = "50px";
            }
            if (args.Column.UniqueID == c)
            {
                if (d.Data != null && d.Data.GetType() == typeof(VisitDTO))
                {
                    args.Attributes.Add("style", $"background-color: var(--rz-warning-light)");
                }
                else if (d.Data == null && (d.StartTime == null || d.EndTime == null || d.StartTime > d.TargetTime || d.EndTime < d.TargetTime))
                {
                    //args.Attributes.Add("style", $"background-color: var(--rz-base-300)");
                }
                else if (d.StartTime != null && d.EndTime != null)
                {
                    //args.Attributes.Add("style", $"background-color: {(d.StartTime <= d.TargetTime && d.EndTime >= d.TargetTime ? "var(--rz-info-light)" : "var(--rz-base-background-color)")};");
                    args.Attributes.Add("style", $"background-color: var(--rz-info-light)");

                }
            }
        }
    }
}
