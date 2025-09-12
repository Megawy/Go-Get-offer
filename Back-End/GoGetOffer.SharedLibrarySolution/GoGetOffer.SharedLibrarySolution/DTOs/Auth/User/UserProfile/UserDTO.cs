using MessagePack;

namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile
{
    [MessagePackObject]
    public class UserDTO
    {
        [Key(0)]
        public Guid? Id { get; set; }
        [Key(1)]
        public string? Email { get; set; }
        [Key(3)]
        public string? CompanyName { get; set; }
        [Key(4)]
        public string? PhoneNumber { get; set; }
        [Key(5)]
        public string? UserType { get; set; }
        [Key(6)]
        public bool? IsEmailConfirmed { get; set; }
        [Key(7)]
        public bool? IsStatusConfirmed { get; set; }
        [Key(8)]
        public DateTime? CreatedAt { get; set; }
        [Key(9)]
        public bool? IsBanned { get; set; }
        [Key(10)]
        public bool? IsDeleted { get; set; }
        [Key(11)]
        public DateTime? DeletedAt { get; set; }
    }
}