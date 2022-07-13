namespace Dogstagram.WebApi.Features.Profile
{
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Features.Identity;
    using Dogstagram.WebApi.Features.Identity.Models;
    using Dogstagram.WebApi.Features.Profile.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ProfileController : ApiController
    {
        private readonly IProfileService profileService;
        private readonly IIdentityService identityService;

        public ProfileController(IProfileService profileService, IIdentityService identityService)
        {
            this.profileService = profileService;
            this.identityService = identityService;
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(Details))]
        public async Task<ActionResult<ProfileDetailsServiceModel>> Details(string userId)
        {
            return await this.profileService.ProfileDetails(userId);
        }

        [Authorize]
        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<ActionResult<CommonResponseModel>> Delete()
        {
            var userName = this.ControllerContext?.HttpContext?.User?.Identity?.Name;
            var result = await this.profileService.DeleteUser(userName!);

            if (result.Failure)
            {
                return this.NotFound(new CommonResponseModel { Message = result.Error });
            }

            return this.Ok(new CommonResponseModel { Message = userName + " Deleted!" });
        }

        [HttpPost]
        [Route(nameof(UndeleteUser))]
        public async Task<ActionResult<CommonResponseModel>> UndeleteUser(LoginRequestModel model)
        {
            var user = this.identityService.GetUserByNameIgnoreFilters(model.UserName!).Result;

            if (user is null)
            {
                return this.NotFound(new CommonResponseModel { Message = "User not found!" });
            }

            await this.profileService.Undelete(user);
            return this.Ok(new CommonResponseModel { Message = "Account has been restored" });
        }
    }
}
