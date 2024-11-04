using Entities;

namespace WebAPI.Filters
{
    public class PatientFilter : IFilter<Patient>
    {
        public PatientFilter(string? fullName, string? phonenumber, string? passport, string? orderDirection, string? orderBy, int top, int skip)
        {
            FullName = fullName;
            PhoneNumber = phonenumber;
            Passport = passport;
            OrderDirection = orderDirection;
            OrderBy = orderBy;
            Top = top;
            Skip = skip;

        }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Passport { get; set; }
        public string? OrderDirection { get; set; }
        public string? OrderBy { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
        public Func<Patient, bool> FilterExpression
        {
            get
            {
                return p =>
                {
                    bool res1 = true;
                    bool res2 = true;
                    bool res3 = true;

                    if (!string.IsNullOrEmpty(FullName))
                        res1 = p.FullName.ToLower().Contains(FullName.ToLower());

                    if (!string.IsNullOrEmpty(PhoneNumber))
                        res2 = p.Phone.Contains(PhoneNumber);

                    if (!string.IsNullOrEmpty(Passport))
                        res3 = p.Passport.Contains(Passport);

                    if (!res1 || !res2 || !res3)
                        return false;

                    return true;


                };
            }
        }
    }
}
