namespace Dogstagram.WebApi.Features.Post
{
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.AspNetCore.Mvc;

    public interface IPostService
    {
        Result UploadFile(PostImageRequestModel model);
    }
}
