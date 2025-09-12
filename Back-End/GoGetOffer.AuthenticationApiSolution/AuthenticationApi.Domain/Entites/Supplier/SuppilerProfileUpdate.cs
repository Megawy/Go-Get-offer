namespace AuthenticationApi.Domain.Entites.Supplier
{
    public class SuppilerProfileUpdate
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? NewFullName { get; set; }
        public string? NewGovernorate { get; set; }
        public string? NewCity { get; set; }
        public string? NewArea { get; set; }
        public string? NewAddressDetails { get; set; }
        public string? NewPostalCode { get; set; }

        public string? UserComment { get; set; }
        public string? AdminComment { get; set; }

        public IsApproved IsApproved { get; set; } = IsApproved.Pending;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DecisionAt { get; set; }

        public Guid SupplierProfilesId { get; set; }
        public virtual SupplierProfile? SupplierProfile { get; set; }
    }
    public enum IsApproved
    {
        Pending,
        Approved,
        Rejected,
    }
}
