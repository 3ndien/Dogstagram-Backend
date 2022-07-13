namespace Dogstagram.WebApi.Features.Follow
{
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.EntityFrameworkCore;

    public class FollowService : IFollowService
    {
        private readonly DogstagramDbContext dbContext;

        public FollowService(DogstagramDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result> Follow(string userId, string followerId)
        {
            var alreadyFollowed = await this
                .dbContext
                .Follows!
                .AnyAsync(u => u.UserId == userId && u.FollowerId == followerId);

            if (alreadyFollowed)
            {
                return "User is already followed!";
            }

            await this.dbContext
                .Follows!
                .AddAsync(new Data.Models.Follow 
                            {Id = Guid.NewGuid().ToString(), UserId = userId, FollowerId = followerId});
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> FollowingCount(string userId)  
            => await this.dbContext.Follows!.Where(u => u.FollowerId == userId).CountAsync();

        public async Task<int> FollowerCount(string userId)
           => await this.dbContext.Follows!.Where(u => u.UserId == userId).CountAsync();

        public async Task<Result> Unfollow(string userId, string followerId)
        {
            var userAlreadyFollowed = 
                  await this.dbContext
                            .Follows!
                            .AnyAsync(f => f.UserId == userId && f.FollowerId == followerId);

            if (!userAlreadyFollowed)
            {
                return false;
            }

            this.dbContext
                .Follows!
                .RemoveRange(await this.dbContext
                .Follows
                .Where(f => f.UserId == userId && f.FollowerId == followerId)
                .ToListAsync());

            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
