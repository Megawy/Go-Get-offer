using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.JoinRequest
{
    [MessagePackObject]
    public class ReplyRequestJoinDTO
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public string? AdminComment { get; set; }
        [Key(2)]
        public string? IsApproved { get; set; }
    }
}
