namespace AuthenticationApi.Domain.Entites.Supplier
{
    public class SupplierJoinRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? AdminComment { get; set; }

        public IsApproved_Join IsApproved { get; set; } = IsApproved_Join.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DecisionAt { get; set; }

        public Guid SupplierProfilesId { get; set; }
        public virtual SupplierProfile? SupplierProfiles { get; set; }
    }
    public enum IsApproved_Join
    {
        Pending,
        Approved,
        Rejected
    }
}
