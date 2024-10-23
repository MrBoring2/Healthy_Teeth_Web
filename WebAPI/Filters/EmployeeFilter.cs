
using Entities;

namespace WebAPI.Filters
{
    public class EmployeeFilter : IFilter<Employee>
    {
        public EmployeeFilter(string? search, IEnumerable<int>? roleIds, IEnumerable<int>? specializationIds, string? orderDirection, string? orderBy, int top, int skip)
        {
            Search = search;
            RoleIds = roleIds;
            SpecializationIds = specializationIds;
            OrderDirection = orderDirection;
            OrderBy = orderBy;
            Top = top;
            Skip = skip;
            
        }

        public string? Search { get; set; }
        public IEnumerable<int>? RoleIds { get; set; }
        public IEnumerable<int>? SpecializationIds { get; set; }
        public string? OrderDirection { get; set; }
        public string? OrderBy { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
        public Func<Employee, bool> FilterExpression
        {
            get
            {
                return p =>
                {
                    bool res1 = true;
                    bool res2 = true;
                    bool res3 = true;

                    if(!string.IsNullOrEmpty(Search))
                        res1 = p.FullName.ToLower().Contains(Search.ToLower());
                    if(RoleIds != null)
                        res2 = RoleIds.Contains(p.Account.RoleId);
                    if(SpecializationIds != null)
                        res3 = SpecializationIds.Contains(p.SpecializationId);

                    if (!res1 || !res2 || !res3)
                        return false;

                    return true;


                };
            }
        }
    }
}
