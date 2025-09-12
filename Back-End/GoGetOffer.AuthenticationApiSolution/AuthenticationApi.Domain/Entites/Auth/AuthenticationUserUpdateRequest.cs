namespace AuthenticationApi.Domain.Entites.Auth
{
    public enum IsApproved
    {
        Rejected,
        Approved,
        Pending
    }

    public class AuthenticationUserUpdateRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AuthenticationUserId { get; set; }

        public string? NewEmail { get; set; }
        public string? NewPhoneNumber { get; set; }
        public string? NewCompanyName { get; set; }
        public string? UserComment { get; set; }
        public string? AdminComment { get; set; }
        public IsApproved IsApproved { get; set; } = IsApproved.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DecisionAt { get; set; }

        public virtual AuthenticationUser? User { get; set; }
    }
}
