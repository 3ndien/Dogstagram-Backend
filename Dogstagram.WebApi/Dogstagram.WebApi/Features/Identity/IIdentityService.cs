namespace Dogstagram.WebApi.Features.Identity
{
    using Dogstagram.WebApi.Data.Models;

    public interface IIdentityService
    {
        string GenerateJwtToken(string userId, string userName, IEnumerable<string> userRoles, string secret);

        Task<User> GetUserByNameIgnoreFilters(string userName);
    }
}
