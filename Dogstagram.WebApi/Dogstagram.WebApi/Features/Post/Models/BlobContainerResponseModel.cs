namespace Dogstagram.WebApi.Features.Post.Models
{
    public class BlobContainerResponseModel
    {
        public byte[]? Content { get; set; }
        
        public string? ContentType { get; set; }
    }
}
