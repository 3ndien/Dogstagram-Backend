
namespace Dogstagram.WebApi.Features.Post
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PostService : IPostService
    {
        private readonly BlobServiceClient blob;

        public PostService(BlobServiceClient blob)
        {
            this.blob = blob;
        }

        public IEnumerable<BlobItem> GetAllFiles(string username)
        {
            var containerClient = this.blob.GetBlobContainerClient($"{username}-container");
            var files = containerClient.GetBlobs().ToList();
            return files;
        }

        public Result UploadFile(PostImageRequestModel model)
        {
            var containerClient = this.blob.GetBlobContainerClient(model.Username + "-container");
            var response = containerClient.UploadBlob(model.Image!.FileName, model.Image.OpenReadStream());

            if (response.GetRawResponse().IsError)
            {
                return false;
            }

            return true;
        }
    }
}
