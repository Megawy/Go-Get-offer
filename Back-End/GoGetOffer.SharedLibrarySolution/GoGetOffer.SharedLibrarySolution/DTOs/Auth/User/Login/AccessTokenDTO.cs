namespace GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.Login
{
    public class AccessTokenDTO
    {
        public string? AccessToken { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsStatusConfirmed { get; set; }
    }
}
