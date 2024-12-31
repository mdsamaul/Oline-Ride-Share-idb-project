namespace Oline_Ride_Share_idb_project.Server.PhoneVerification
{
    public class JwtTokenResponse
    {
        public string Token { get; set; } 
        public DateTime Expiration { get; set; }
    }
}
