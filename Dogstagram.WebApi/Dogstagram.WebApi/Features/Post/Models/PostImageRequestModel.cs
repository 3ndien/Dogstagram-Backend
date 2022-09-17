namespace Dogstagram.WebApi.Features.Post.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostImageRequestModel
    {
        [Required]
        public IFormFile? Image { get; set; }
    }
}
