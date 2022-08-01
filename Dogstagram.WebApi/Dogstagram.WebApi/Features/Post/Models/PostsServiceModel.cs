namespace Dogstagram.WebApi.Features.Post.Models
{
    public class PostsServiceModel
    {
        public string? Label { get; set; }

        public ICollection<string> Content { get; set; } = new HashSet<string>();
    }
}
