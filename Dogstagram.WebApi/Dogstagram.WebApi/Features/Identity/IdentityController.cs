namespace Dogstagram.WebApi.Features.Identity
{
    using AutoMapper;
    using Azure.Storage.Blobs;
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Features.Identity.Models;
    using Dogstagram.WebApi.Features.Profile;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System.Net;

    public class IdentityController : ApiController
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly IIdentityService identityService;
        private readonly BlobServiceClient blob;
        private readonly ApplicationSettings applicationSettings;

        public IdentityController(
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            IIdentityService identityService,
            IOptions<ApplicationSettings> applicationSettings,
            BlobServiceClient blob)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.identityService = identityService;
            this.blob = blob;
            this.applicationSettings = applicationSettings.Value;
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = this.mapper.Map<User>(model);

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await blob.CreateBlobContainerAsync($"{user.UserName}-container");
                return this.StatusCode((int)HttpStatusCode.Created);
            }

            return this.BadRequest(result.Errors);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(LoginRequestModel model)
        {
            var user = await this.identityService.GetUserByNameIgnoreFilters(model.UserName!);

            if (user == null)
            {
                return this.NotFound(new CommonResponseModel { Message = "User not found!" });
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                return this.Unauthorized(new CommonResponseModel { Message = "Invalid password!" });
            }

            if (user.IsDeleted)
            {
                return RedirectToActionPreserveMethod(nameof(ProfileController.UndeleteUser), "Profile");
            }
            var responseModel = new LoginResponseModel
            {
                Roles = roles,
                ImageUrl = user?.Profile?.PhotoUrl,
                Token = this.identityService
                .GenerateJwtToken(user.Id, user.UserName, roles, this.applicationSettings.Secret!)
            };
            return this.Ok(responseModel);
        }
    }
}
