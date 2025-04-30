using Entities;
using Shared.Constants;

namespace WebAPI.Filters
{
    public class VisitsFilter : IFilter<Visit>
    {
        public VisitsFilter(string? patient, string? doctor, IEnumerable<int>? statusIds, DateOnly startDate, DateOnly endDate, string? orderDirection, string? orderBy, int top, int skip)
        {
            Patient = patient;
            Doctor = doctor;
            StatusIds = statusIds;
            StartDate = startDate;
            EndDate = endDate;
            OrderDirection = orderDirection;
            OrderBy = orderBy;
            Top = top;
            Skip = skip;

        }

        public string? Patient { get; set; }
        public string? Doctor { get; set; }
        public IEnumerable<int>? StatusIds { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? OrderDirection { get; set; }
        public string? OrderBy { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
        public Func<Visit, bool> FilterExpression
        {
            get
            {
                return p =>
                {
                    bool res1 = true;
                    bool res2 = true;
                    bool res3 = true;
                    bool res4 = true;


                    if (!string.IsNullOrEmpty(Patient))
                    {
                        res1 = p.Patient.FullName.ToLower().Contains(Patient.ToLower());
                    }
                    if (!string.IsNullOrEmpty(Doctor))
                    {
                        res2 = p.Employee.FullName.ToLower().Contains(Doctor.ToLower());
                    }
                    if (StatusIds != null)
                        res3 = StatusIds.Contains(p.VisitStatusId);
                    res4 = p.VisitDate >= StartDate && p.VisitDate <= EndDate;

                    if (!res1 || !res2 || !res3 || !res4)
                        return false;

                    return true;


                };
            }
        }
    }
}
