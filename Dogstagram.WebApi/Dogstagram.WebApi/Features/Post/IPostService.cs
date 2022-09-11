namespace Dogstagram.WebApi.Features.Post
{
    using Dogstagram.WebApi.Features.Post.Models;

    public interface IPostService
    {
        Task<UploadImageResponseModel> UploadFile(PostImageRequestModel model);

        Task<PostsServiceModel> GetAllFiles(string username);
    }
}
