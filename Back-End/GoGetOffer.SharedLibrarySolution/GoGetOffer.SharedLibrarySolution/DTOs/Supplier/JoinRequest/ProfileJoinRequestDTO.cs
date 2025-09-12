using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest
{
    [MessagePackObject]
    public class ProfileJoinRequestDTO
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string? AdminComment { get; set; }
        [Key(2)]
        public string? IsApproved { get; set; }
        [Key(3)]
        public DateTime RequestedAt { get; set; }
        [Key(4)]
        public DateTime? DecisionAt { get; set; }
        [Key(5)]
        public Guid SupplierProfilesId { get; set; }
    }
}
