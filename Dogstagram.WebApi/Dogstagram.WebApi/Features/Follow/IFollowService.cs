namespace Dogstagram.WebApi.Features.Follow
{
    public interface IFollowService
    {
        Task<bool> Follow(string userId, string followerId);

        Task<bool> Unfollow(string userId, string followerId);

        Task<int> FollowerCount(string userId);

        Task<int> FollowingCount(string userId);
    }
}
