namespace Dogstagram.WebApi.Data.Models
{
    using Dogstagram.WebApi.Data.Models.Base;
    using Microsoft.AspNetCore.Identity;

    public class UserRole : IdentityRole, IAuditInfo, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
