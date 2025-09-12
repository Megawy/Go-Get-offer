using AuthenticationApi.Domain.Entites.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.UserConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<AuthenticationUser>
    {
        public void Configure(EntityTypeBuilder<AuthenticationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.CompanyName)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.CompanyName).IsUnique();

            builder.HasIndex(u => u.PhoneNumber).IsUnique();

            // Del User
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
