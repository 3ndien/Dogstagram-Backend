namespace Dogstagram.WebApi.Features.Post.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostImageRequestModel
    {
        public string? Username { get; set; }

        [Required]
        public IFormFile? Image { get; set; }
    }
}
