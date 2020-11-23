using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class StaffCategoryConfiguration : IEntityTypeConfiguration<StaffCategory>
    {
        public void Configure(EntityTypeBuilder<StaffCategory> builder)
        {
            builder.ToTable("StaffCategories");
            builder.HasIndex(x => x.Id);
            builder.HasMany(x => x.Staff)
                .WithOne(x => x.StaffCategory)
                .HasForeignKey(x => x.StaffCategoryId);
        }
    }
}