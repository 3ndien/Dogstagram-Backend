namespace Dogstagram.WebApi.Features.Post
{
    using Dogstagram.WebApi.Controllers;
    using Dogstagram.WebApi.Features.Post.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> Create([FromForm] PostImageRequestModel model)
        {
            var result = this.postService.UploadFile(model);
            
            if (result.Failure)
            {
                return this.BadRequest();
            }
            return this.StatusCode((int)HttpStatusCode.Created);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(GetAllFiles))]
        public async Task<AllFilesServiceModel> GetAllFiles()
        {
            var username = this.HttpContext.User.Identity!.Name;
            var result = new AllFilesServiceModel();

            var files = this.postService.GetAllFiles(username!);
            files.ToList().ForEach(f => result.Files!.Add(f));

            return result;
        }
    }
}
