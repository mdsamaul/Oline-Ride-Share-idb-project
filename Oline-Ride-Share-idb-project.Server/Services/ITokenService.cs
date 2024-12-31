namespace Oline_Ride_Share_idb_project.Server.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(string phoneNumber, string userId);
    }

}
