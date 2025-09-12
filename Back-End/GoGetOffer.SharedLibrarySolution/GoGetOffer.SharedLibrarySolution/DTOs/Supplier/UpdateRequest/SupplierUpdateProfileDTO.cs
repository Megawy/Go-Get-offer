using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.UpdateRequest
{
    [MessagePackObject]
    public class SupplierUpdateProfileDTO
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string? NewFullName { get; set; }
        [Key(2)]
        public string? NewGovernorate { get; set; }
        [Key(3)]
        public string? NewCity { get; set; }
        [Key(4)]
        public string? NewArea { get; set; }
        [Key(5)]
        public string? NewAddressDetails { get; set; }
        [Key(6)]
        public string? NewPostalCode { get; set; }
        [Key(7)]
        public string? UserComment { get; set; }
        [Key(8)]
        public string? AdminComment { get; set; }
        [Key(9)]
        public string? IsApproved { get; set; }
        [Key(10)]
        public DateTime RequestedAt { get; set; }
        [Key(11)]
        public DateTime? DecisionAt { get; set; }
        [Key(12)]
        public Guid SupplierProfilesId { get; set; }
    }
}
