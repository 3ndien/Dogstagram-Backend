namespace Dogstagram.WebApi.Data.Models
{
    using Dogstagram.WebApi.Data.Models.Base;

    public class Comment : BaseDeletableModel<int>
    {
        public string? Content { get; set; }

        public int PostId { get; set; }

        public virtual Post? Post { get; set; }


        public string? UserId { get; set; }

        public User? User { get; set; }
    }
}
