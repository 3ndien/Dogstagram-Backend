namespace Dogstagram.WebApi.Data.Models
{
    using Dogstagram.WebApi.Data.Models.Base;

    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }

        public string? Caption { get; set; }

        public string? ImageUrl { get; set; }

        public string? Tag { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
