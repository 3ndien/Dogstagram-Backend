
namespace Dogstagram.WebApi.Features.Post
{
    using Azure.Storage.Blobs;
    using Dogstagram.WebApi.Features.Post.Models;
    using Dogstagram.WebApi.Infrastructures.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    public class PostService : IPostService
    {
        private readonly BlobServiceClient blob;

        public PostService(BlobServiceClient blob)
        {
            this.blob = blob;
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
