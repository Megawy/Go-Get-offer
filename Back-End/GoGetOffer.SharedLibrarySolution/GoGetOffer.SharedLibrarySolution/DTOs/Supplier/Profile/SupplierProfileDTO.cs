using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile
{
    [MessagePackObject]
    public class SupplierProfileDTO
    {
        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string? FullName { get; set; }

        [Key(2)]
        public List<string>? CommercialRegistrationDocumentUrl { get; set; }
        [Key(3)]
        public List<string>? CommercialRegistrationDocumentPublicId { get; set; }

        [Key(4)]
        public List<string>? TaxCardDocumentUrl { get; set; }
        [Key(5)]
        public List<string>? TaxCardDocumentPublicId { get; set; }

        [Key(6)]
        public List<string>? ActivityType { get; set; }
        [Key(7)]
        public int? MinProducts { get; set; }
        [Key(8)]
        public int? MaxProducts { get; set; }
        [Key(9)]
        public int? DeliveryTimeInDays { get; set; }
        [Key(10)]
        public decimal? MinInvoiceAmount { get; set; }
        [Key(11)]
        public bool HasElctroinInvoice { get; set; } = false;
        [Key(12)]
        public bool HasDeliveryService { get; set; } = false;
        [Key(13)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Key(14)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Key(15)]
        public string? Status { get; set; }
        [Key(16)]
        public string? Code { get; set; }
    }
}
