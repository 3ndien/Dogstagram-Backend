
namespace Dogstagram.WebApi.Features.Post
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Sas;
    using Dogstagram.WebApi.Features.Post.Models;
    using System.Threading.Tasks;

    public class PostService : IPostService
    {
        private readonly BlobServiceClient blob;

        public PostService(BlobServiceClient blob)
        {
            this.blob = blob;
        }

        public async Task<PostsServiceModel> GetAllFiles(string username)
        {
            var posts = new PostsServiceModel();
            posts.Label = "Posts";
            var blobContainer = this.blob.GetBlobContainerClient($"{username}-container");

            var blobNames = blobContainer.GetBlobs().Select(b => b.Name).ToList();

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobContainer.Name,
                Resource = "c",
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

            for (int i = 0; i < blobNames.Count; i++)
            {
                var sasUri = blobContainer.GetBlobClient(blobNames[i]).GenerateSasUri(sasBuilder);
                posts.Content.Add(sasUri.AbsoluteUri);
            }
            return posts;
        }

        public async Task<UploadImageResponseModel> UploadFile(PostImageRequestModel model)
        {
            var containerClient = this.blob.GetBlobContainerClient(model.Username + "-container");
            var response = await containerClient
                .GetBlobClient(model.Image!.FileName)
                .UploadAsync(model.Image.OpenReadStream(), new BlobHttpHeaders { ContentType = model.Image.ContentType });

            if (response.GetRawResponse().IsError)
            {
                return null!;
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerClient.Name,
                Resource = "c"
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            var result = new UploadImageResponseModel
            {
                ImageUrl = containerClient.GetBlobClient(model.Image.FileName).GenerateSasUri(sasBuilder).AbsoluteUri
            }; 
            return result;
        }
    }
}
