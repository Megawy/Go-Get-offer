using AuthenticationApi.Domain.Entites.Supplier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.SupplierConfiguration;

internal class SupplierBranchConfiguration : IEntityTypeConfiguration<SupplierBranch>
{
    public void Configure(EntityTypeBuilder<SupplierBranch> builder)
    {
        builder.Property(p => p.BranchName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Governorate)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Area)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.AddressDetails)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(p => p.PostalCode)
        .HasMaxLength(300);

        builder.HasKey(sjr => sjr.Id);

        builder
            .HasOne(sjr => sjr.SupplierProfiles)
            .WithMany(sp => sp.SupplierBranches)
            .HasForeignKey(sjr => sjr.SupplierProfilesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.SupplierProfiles!.User!.IsDeleted);
    }
}

