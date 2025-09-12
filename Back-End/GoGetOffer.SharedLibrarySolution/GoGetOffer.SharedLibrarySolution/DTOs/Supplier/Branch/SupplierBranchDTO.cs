using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Branch
{
    [MessagePackObject]
    public class SupplierBranchDTO
    {
        [Key(0)]
        public Guid? Id { get; set; }
        [Key(1)]
        public Guid? SupplierProfilesId { get; set; }
        [Key(2)]
        public string? BranchName { get; set; }
        [Key(3)]
        public string? Governorate { get; set; }
        [Key(4)]
        public string? City { get; set; }
        [Key(5)]
        public string? Area { get; set; }
        [Key(6)]
        public string? AddressDetails { get; set; }
        [Key(7)]
        public string? PostalCode { get; set; }
        [Key(8)]
        public List<string>? PhoneNumbers { get; set; }
        [Key(9)]
        public bool? Main_Branch { get; set; }
    }
}
