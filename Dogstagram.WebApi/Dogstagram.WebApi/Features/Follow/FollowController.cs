namespace Dogstagram.WebApi.Features.Follow
{
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FollowController : ApiController
    {
        private readonly IFollowService followService;
        private readonly UserManager<User> userManager;

        public FollowController(IFollowService followService, UserManager<User> userManager)
        {
            this.followService = followService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(Follow))]
        public async Task<ActionResult> Follow(string userId)
        {
            var currentUserId = this.userManager.FindByNameAsync(this.User?.Identity?.Name).Result.Id;
            var result = await this.followService.Follow(currentUserId, userId);
            if (result.Failure)
            {
                return this.BadRequest(new CommonResponseModel { Message = result.Error});
            }
            return this.Ok();
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(Unfollow))]
        public async Task<ActionResult> Unfollow(string userId)
        {
            var currentUserId = this.userManager.FindByNameAsync(this.User?.Identity?.Name).Result.Id;
            var result = await this.followService.Unfollow(currentUserId, userId);

            if (result.Failure)
            {
                return this.BadRequest();
            }

            return this.Ok();
        }
    }
}
