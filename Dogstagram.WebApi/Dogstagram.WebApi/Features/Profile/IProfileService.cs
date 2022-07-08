namespace Dogstagram.WebApi.Features.Profile
{
    using Dogstagram.WebApi.Data.Models.Base;
    using Dogstagram.WebApi.Features.Profile.Models;

    public interface IProfileService
    {
        Task<ProfileDetailsServiceModel> ProfileDetails(string userId);

        Task<bool> DeleteUser(string user);

        Task Undelete<TEntity>(TEntity entity)
            where TEntity : IDeletableEntity;
    }
}
