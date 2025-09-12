using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Domain.Entites.Supplier;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AuthenticationDbContext(DbContextOptions options) : DbContext(options)
    {
        // Authentication
        // Authentication User
        public DbSet<AuthenticationUser> AuthenticationUser { get; set; }

        // Authentication User Update Request
        public DbSet<AuthenticationUserUpdateRequest> AuthenticationUserUpdateRequests { get; set; }

        // Supplier 
        // Supplier Profile
        public DbSet<SupplierProfile> Suppliers { get; set; }

        // Supplier Branch
        public DbSet<SupplierBranch> SupplierBranches { get; set; }

        // Supplier Join Request
        public DbSet<SupplierJoinRequest> SupplierJoinRequests { get; set; }

        // Supplier Profile Update
        public DbSet<SuppilerProfileUpdate> SuppilerProfileUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Authentication
            // Authentication User
            modelBuilder.Entity<AuthenticationUser>().ToTable("AuthenticationUser", "Auth");

            // Authentication User Update Request
            modelBuilder.Entity<AuthenticationUserUpdateRequest>().ToTable("AuthenticationUserUpdateRequests", "AuthApi");

            // Supplier 
            // Supplier Profile
            modelBuilder.Entity<SupplierProfile>().ToTable("Suppliers", "AuthApi");

            // Supplier Branch
            modelBuilder.Entity<SupplierBranch>().ToTable("SupplierBranches", "AuthApi");

            // Supplier Join Request
            modelBuilder.Entity<SupplierJoinRequest>().ToTable("SupplierJoinRequests", "AuthApi");

            // Supplier Profile Update
            modelBuilder.Entity<SuppilerProfileUpdate>().ToTable("SuppilerProfileUpdates", "AuthApi");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthenticationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
