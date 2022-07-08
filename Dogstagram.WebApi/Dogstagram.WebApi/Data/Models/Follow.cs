namespace Dogstagram.WebApi.Data.Models
{
    using Dogstagram.WebApi.Data.Models.Base;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Follow : BaseModel<string>, IAuditInfo, IDeletableEntity
    {
        [Required]
        public string? UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public string? FollowerId { get; set; }

        public User? Follower { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
