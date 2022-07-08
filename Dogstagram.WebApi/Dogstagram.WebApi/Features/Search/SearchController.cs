namespace Dogstagram.WebApi.Features.Search
{
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Features.Search.Models;
    using Microsoft.AspNetCore.Mvc;

    public class SearchController : ApiController
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        [Route(nameof(Profiles))]
        public async Task<IEnumerable<SearchProfileServiceModel>> Profiles(string query) 
            => await searchService.Profiles(query);
    }
}
