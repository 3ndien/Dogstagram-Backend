namespace Dogstagram.WebApi.Features.Profile
{
    using Dogstagram.WebApi.Data.Models.Base;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Features.Profile.Models;
    using Dogstagram.WebApi.Infrastructures.Services;

    public interface IProfileService
    {
        Task<ProfileDetailsServiceModel> ProfileDetails(string userId);

        Task<Result> UpdateProfileDetails(UpdateProfileDetailsRequestModel model, string userId);

        Task<string> AddProfilePictureUrl(PostImageRequestModel model, string userId);

        Task<Result> DeleteUser(string user);

        Task Undelete<TEntity>(TEntity entity)
            where TEntity : IDeletableEntity;
    }
}
