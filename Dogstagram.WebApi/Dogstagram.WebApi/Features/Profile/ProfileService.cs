namespace Dogstagram.WebApi.Features.Profile
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Data.Models.Base;
    using Dogstagram.WebApi.Features.Follow;
    using Dogstagram.WebApi.Features.Post;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Features.Profile.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> userManager;
        private readonly DogstagramDbContext dbContext;
        private readonly IFollowService followService;
        private readonly BlobServiceClient blob;

        public ProfileService(
            UserManager<User> userManager,
            DogstagramDbContext dbContext,
            IFollowService followService,
            BlobServiceClient blob)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.followService = followService;
            this.blob = blob;
        }

        public async Task<Result> DeleteUser(string userName)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
            {
                return $"User name {userName} not found!";
            }

            this.dbContext.Users.Remove(user);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProfileDetailsServiceModel> ProfileDetails(string userId)
        {
            var user = await this.dbContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var model = new ProfileDetailsServiceModel
            {
                Username = user?.UserName!,
                ShortName = user?.Profile?.ShortName,
                PhotoUrl = user?.Profile?.PhotoUrl,
                FollowerCount = await this.followService.FollowerCount(userId),
                FollowingCount = await this.followService.FollowingCount(userId),
            };

            return model;
        }

        public async Task Undelete<TEntity>(TEntity entity)
            where TEntity : IDeletableEntity
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<string> AddProfilePictureUrl  (PostImageRequestModel model)
        {
            var containerClient = this.blob.GetBlobContainerClient(model.Username + "-container");
            var user = await this.dbContext.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);
            string sasUri = string.Empty;

            var blobNames = containerClient.GetBlobs().Select(b => b.Name).ToList();
            
            if (!blobNames.Any(b => b == model.Image?.FileName))
            {
                var response = await containerClient.GetBlobClient(model.Image?.FileName)
                    .UploadAsync(model.Image?.OpenReadStream(), new BlobHttpHeaders { ContentType = model?.Image?.ContentType });
                if (response.GetRawResponse().IsError)
                {
                    sasUri = string.Empty;
                }
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Name,
                Resource = "c",
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

            sasUri = containerClient.GetBlobClient(model?.Image?.FileName).GenerateSasUri(sasBuilder).AbsoluteUri;

            if (user!.Profile == null)
            {
                user.Profile = new UserProfile();
            }
            user!.Profile!.PhotoUrl = sasUri;
            this.dbContext?.SaveChangesAsync();
            return sasUri;
        }
    }
}
