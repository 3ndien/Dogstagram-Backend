namespace Dogstagram.WebApi.Data
{
    using Dogstagram.WebApi.Data.Models.Base;
    using Microsoft.EntityFrameworkCore;

    public static class EntityIndexesConfiguration
    {
        public static void Configure(ModelBuilder builder)
        {
            var deletableEntityTypes =
                builder
                .Model
                .GetEntityTypes()
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                builder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
            }
        }
    }
}
