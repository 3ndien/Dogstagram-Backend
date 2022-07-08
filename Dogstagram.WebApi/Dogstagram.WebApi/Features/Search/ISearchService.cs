namespace Dogstagram.WebApi.Features.Search
{
    using Dogstagram.WebApi.Features.Search.Models;

    public interface ISearchService
    {
        Task<IEnumerable<SearchProfileServiceModel>> Profiles(string query);
    }
}
