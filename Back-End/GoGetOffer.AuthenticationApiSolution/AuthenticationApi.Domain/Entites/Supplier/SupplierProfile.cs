using AuthenticationApi.Domain.Entites.Auth;

namespace AuthenticationApi.Domain.Entites.Supplier
{
    public class SupplierProfile
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public string? Code { get; set; }

        public List<string>? CommercialRegistrationDocumentUrl { get; set; }
        public List<string>? CommercialRegistrationDocumentPublicId { get; set; }
        public List<string>? TaxCardDocumentUrl { get; set; }
        public List<string>? TaxCardDocumentPublicId { get; set; }
        public List<string>? ActivityType { get; set; }

        public int? MinProducts { get; set; }
        public int? MaxProducts { get; set; }
        public int? DeliveryTimeInDays { get; set; }
        public decimal? MinInvoiceAmount { get; set; }
        public bool HasElctroinInvoice { get; set; } = false;
        public bool HasDeliveryService { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }


        public SupplierStatus? Status { get; set; } = SupplierStatus.Pending;
        public virtual AuthenticationUser? User { get; set; }
        public virtual ICollection<SupplierBranch>? SupplierBranches { get; set; }
        public virtual ICollection<SupplierJoinRequest>? SuppliersJoinRequests { get; set; }
        public virtual ICollection<SuppilerProfileUpdate>? ProfileSuppilerUpdates { get; set; }
    }
    public enum SupplierStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
