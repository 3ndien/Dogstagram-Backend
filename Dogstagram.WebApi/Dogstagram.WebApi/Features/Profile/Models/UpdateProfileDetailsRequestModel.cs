namespace Dogstagram.WebApi.Features.Profile.Models
{
    public class UpdateProfileDetailsRequestModel
    {
        public string? UserName { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public string? Biography { get; set; }
    }
}
