using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace WebAPI.Filters
{
    public interface IFilter<T>
    {
        Func<T, bool> FilterExpression { get; }
    }
}
