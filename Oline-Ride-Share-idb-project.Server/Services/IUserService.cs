namespace Oline_Ride_Share_idb_project.Server.Services
{
    public interface IUserService
    {
        UserModel GetUserByPhoneNumber(string phoneNumber);
        void AddUser(UserModel user);
    }

}
