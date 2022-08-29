namespace Dogstagram.WebApi.Features.Profile
{
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Data.Models.Base;
    using Dogstagram.WebApi.Features.Follow;
    using Dogstagram.WebApi.Features.Profile.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> userManager;
        private readonly DogstagramDbContext dbContext;
        private readonly IFollowService followService;

        public ProfileService(UserManager<User> userManager, DogstagramDbContext dbContext, IFollowService followService)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.followService = followService;
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
                Username = user.UserName,
                ShortName = user?.Profile?.ShortName,
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
    }
}
