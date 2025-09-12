using AuthenticationApi.Domain.Entites.Supplier;

namespace AuthenticationApi.Domain.Entites.Auth
{
    public enum UserType
    {
        User,
        Client,
        Supplier,
        Staff,
        Admin
    }
    public class AuthenticationUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }

        public UserType UserType { get; set; } = UserType.User;

        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsStatusConfirmed { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual SupplierProfile? SupplierProfile { get; set; }
        public virtual ICollection<AuthenticationUserUpdateRequest>? AuthenticationUserUpdateRequests { get; set; }
    }
}
