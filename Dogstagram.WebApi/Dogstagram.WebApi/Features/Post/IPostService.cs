namespace Dogstagram.WebApi.Features.Post
{
    using Azure.Storage.Blobs.Models;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.AspNetCore.Mvc;

    public interface IPostService
    {
        Result UploadFile(PostImageRequestModel model);

        IEnumerable<BlobItem> GetAllFiles(string username);
    }
}
