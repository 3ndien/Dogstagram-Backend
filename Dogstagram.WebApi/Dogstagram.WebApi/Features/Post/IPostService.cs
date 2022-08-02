namespace Dogstagram.WebApi.Features.Post
{
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Infrastructures.Services;

    public interface IPostService
    {
        Task<UploadImageResponseModel> UploadFile(PostImageRequestModel model);

        Task<PostsServiceModel> GetAllFiles(string username);
    }
}
