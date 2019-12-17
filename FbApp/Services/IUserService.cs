using FbApp.Models;

namespace FbApp.Services
{
    public interface IUserService
    {
        bool UserExists(string userId);

        void MakeFriends(string senderId, string receiverId);

        bool CheckIfFriends(string requestUserId, string targetUserId);

        UserAccountModel UserDetails(string userId);
    }
}