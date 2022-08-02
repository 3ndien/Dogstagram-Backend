namespace Dogstagram.WebApi.Features.Post
{
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Features.Post.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Net;

    public class PostController : ApiController
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult<UploadImageResponseModel>> Create([FromForm] PostImageRequestModel model)
        {
            var result = await this.postService.UploadFile(model);

            if (result == null)
            {
                return this.BadRequest();
            }
            return this.StatusCode((int)HttpStatusCode.Created, result);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(GetAllFiles))]
        public async Task<ActionResult<PostsServiceModel>> GetAllFiles()
        {
            var username = this.HttpContext.User.Identity?.Name;
            var result = await this.postService.GetAllFiles(username!);
            return result;
        }
    }
}
