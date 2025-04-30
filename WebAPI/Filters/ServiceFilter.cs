using Entities;

namespace WebAPI.Filters
{
    public class ServiceFilter : IFilter<Service>
    {
        public ServiceFilter(string? search, IEnumerable<int>? specializationIds, string? orderDirection, string? orderBy, int top, int skip)
        {
            Search = search;
            SpecializationIds = specializationIds;
            OrderDirection = orderDirection;
            OrderBy = orderBy;
            Top = top;
            Skip = skip;

        }

        public string? Search { get; set; }
        public string? OrderDirection { get; set; }
        public IEnumerable<int>? SpecializationIds { get; set; }
        public string? OrderBy { get; set; }
        public int Top { get; set; }
        public int Skip { get; set; }
        public Func<Service, bool> FilterExpression
        {
            get
            {
                return p =>
                {
                    bool res1 = true;
                    bool res2 = true;

                    if (!string.IsNullOrEmpty(Search))
                        res1 = p.Title.ToLower().Contains(Search.ToLower());

                    if (SpecializationIds != null)
                        res2 = SpecializationIds.Contains(p.SpecializationId);

                    if (!res1 || !res2)
                        return false;

                    return true;


                };
            }
        }
    }
}
