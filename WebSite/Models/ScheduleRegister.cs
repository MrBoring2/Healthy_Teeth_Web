using Microsoft.AspNetCore.Components;
using Shared.DTO;

namespace WebSite.Models
{
    public class ScheduleRegister
    {
        public ScheduleRegister()
        {
        }

        public ScheduleRegister(int employeeId, TimeOnly? startTime, TimeOnly? endTime, TimeOnly? targenTime, object data)
        {
            EmployeeId = employeeId;
            StartTime = startTime;
            Data = data;
            EndTime = endTime;
            TargetTime = targenTime;
        }

        public int EmployeeId { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public TimeOnly? TargetTime { get; set; }
        public object Data { get; set; }
        public MarkupString Value
        {
            get
            {
                if (Data != null && Data.GetType() == typeof(VisitDTO))
                {
                    var data = (Data as VisitDTO);
                    return (MarkupString)$"Пациент: {data.Patient.FullName}<br /> Цель визита: {data.VisitPurpose}";

                }
                else if (Data == null && (StartTime == null || EndTime == null || StartTime > TargetTime || EndTime < TargetTime))
                {
                    return (MarkupString)"";
                }
                else if (Data == null)
                {
                    return (MarkupString)"";
                }
                else if (Data.GetType() == typeof(TimeOnly))
                {
                    return (MarkupString)(TimeOnly.ParseExact(Data.ToString(), "HH:mm").ToShortTimeString());
                }
                return (MarkupString)"";
            }
        }
    }
}
