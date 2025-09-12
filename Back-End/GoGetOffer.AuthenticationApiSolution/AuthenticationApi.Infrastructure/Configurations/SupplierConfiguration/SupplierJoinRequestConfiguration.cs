using AuthenticationApi.Domain.Entites.Supplier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.SupplierConfiguration
{
    public class SupplierJoinRequestConfiguration : IEntityTypeConfiguration<SupplierJoinRequest>
    {
        public void Configure(EntityTypeBuilder<SupplierJoinRequest> builder)
        {
            builder.HasKey(sjr => sjr.Id);

            builder
                .HasOne(sjr => sjr.SupplierProfiles)
                .WithMany(sp => sp.SuppliersJoinRequests)
                .HasForeignKey(sjr => sjr.SupplierProfilesId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(sjr => sjr.AdminComment)
                   .HasMaxLength(500);

            builder.Property(sjr => sjr.IsApproved)
                   .HasDefaultValue(IsApproved_Join.Pending);

            builder.Property(sjr => sjr.RequestedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasQueryFilter(x => !x.SupplierProfiles!.User!.IsDeleted);
        }
    }
}
