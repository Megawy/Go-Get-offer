using AuthenticationApi.Domain.Entites.Supplier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.SupplierConfiguration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<SupplierProfile>
    {
        public void Configure(EntityTypeBuilder<SupplierProfile> builder)
        {
            builder.Property(p => p.FullName)
                .HasMaxLength(400)
                .IsRequired();

            builder.Property(p => p.CommercialRegistrationDocumentUrl)
                   .IsRequired();

            builder.Property(p => p.TaxCardDocumentUrl)
                .IsRequired();

            builder.Property(p => p.ActivityType)
                .IsRequired();

            builder.Property(p => p.Code)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(u => u.Code).IsUnique();

            builder.Property(p => p.HasElctroinInvoice)
                .IsRequired();

            builder.Property(p => p.MinInvoiceAmount).HasPrecision(18, 2);

            builder.HasQueryFilter(s => !s.User!.IsDeleted);

            builder.HasOne(u => u.User).
                WithOne(u => u.SupplierProfile).
                HasForeignKey<SupplierProfile>(s => s.Id);
        }
    }
}
