namespace Dogstagram.WebApi.Features.Follow
{
    using Dogstagram.WebApi.Infrastructures.Services;

    public interface IFollowService
    {
        Task<Result> Follow(string userId, string followerId);

        Task<Result> Unfollow(string userId, string followerId);

        Task<int> FollowerCount(string userId);

        Task<int> FollowingCount(string userId);
    }
}
