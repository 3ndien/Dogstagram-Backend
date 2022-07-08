namespace Dogstagram.WebApi.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class UserProfile
    {
        [Required]
        public string? ShortName { get; set; }
        
        [Required]
        public string? PhotoUrl { get; set; }
    }
}
