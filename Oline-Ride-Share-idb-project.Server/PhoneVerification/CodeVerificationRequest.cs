namespace Oline_Ride_Share_idb_project.Server.PhoneVerification
{
    public class CodeVerificationRequest
    {
        public string PhoneNumber { get; set; } 
        public required string Code { get; set; } 
        public string Role { get; set; }
    }
}
