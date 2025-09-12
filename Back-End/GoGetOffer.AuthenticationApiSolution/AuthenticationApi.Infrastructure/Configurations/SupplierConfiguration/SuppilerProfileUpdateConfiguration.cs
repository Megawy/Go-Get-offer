using AuthenticationApi.Domain.Entites.Supplier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.SupplierConfiguration
{
    public class SuppilerProfileUpdateConfiguration : IEntityTypeConfiguration<SuppilerProfileUpdate>
    {
        public void Configure(EntityTypeBuilder<SuppilerProfileUpdate> builder)
        {
            builder.HasKey(sjr => sjr.Id);

            builder
                .HasOne(sjr => sjr.SupplierProfile)
                .WithMany(sp => sp.ProfileSuppilerUpdates)
                .HasForeignKey(sjr => sjr.SupplierProfilesId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(sjr => sjr.IsApproved)
                       .HasDefaultValue(IsApproved.Pending);

            builder.HasQueryFilter(x => !x.SupplierProfile!.User!.IsDeleted);
        }
    }
}
