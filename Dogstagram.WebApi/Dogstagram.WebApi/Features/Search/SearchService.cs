namespace Dogstagram.WebApi.Features.Search
{
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Features.Search.Models;
    using Microsoft.EntityFrameworkCore;

    public class SearchService : ISearchService
    {
        private readonly DogstagramDbContext dbContext;

        public SearchService(DogstagramDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<SearchProfileServiceModel>> Profiles(string query)
            => await this.dbContext
            .Users
            .Where(u => u.UserName == query || u.ShortName == query)
            .Select(u => new SearchProfileServiceModel
            {
                Id = u.Id,
                Username = u.UserName,
                PhotoUrl = u.ProfilePictureUrl,
            })
            .ToListAsync();
    }
}
