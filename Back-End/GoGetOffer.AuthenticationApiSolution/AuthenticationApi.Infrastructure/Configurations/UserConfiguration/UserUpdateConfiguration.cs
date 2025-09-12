using AuthenticationApi.Domain.Entites.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationApi.Infrastructure.Configurations.UserConfiguration
{
    public class UserUpdateConfiguration : IEntityTypeConfiguration<AuthenticationUserUpdateRequest>
    {
        public void Configure(EntityTypeBuilder<AuthenticationUserUpdateRequest> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(p => p.User)
                .WithMany(sp => sp.AuthenticationUserUpdateRequests)
                .HasForeignKey(sjr => sjr.AuthenticationUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
