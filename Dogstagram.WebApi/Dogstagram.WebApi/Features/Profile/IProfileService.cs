namespace Dogstagram.WebApi.Features.Profile
{
    using Azure.Storage.Blobs.Models;
    using Dogstagram.WebApi.Data.Models.Base;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Features.Profile.Models;
    using Dogstagram.WebApi.Infrastructures.Services;

    public interface IProfileService
    {
        Task<ProfileDetailsServiceModel> ProfileDetails(string userId);

        Task<string> AddProfilePictureUrl(PostImageRequestModel model);

        Task<Result> DeleteUser(string user);

        Task Undelete<TEntity>(TEntity entity)
            where TEntity : IDeletableEntity;
    }
}
