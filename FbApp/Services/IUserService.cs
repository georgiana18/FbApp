using FbApp.Dtos;
using System.Collections.Generic;

namespace FbApp.Services
{
    public interface IUserService
    {
        UserModel GetById(string id);

        bool UserExists(string userId);

        void MakeFriends(string senderId, string receiverId);

        bool CheckIfFriends(string requestUserId, string targetUserId);

        UserAccountModel UserDetails(string userId);

        UserAccountModel UserDetailsFriendsCommentsAndPosts(string userId);

        bool CheckIfDeletedByUserName(string username);

        IEnumerable<UserListModel> UsersBySearchTerm(string searchTerm);

        IEnumerable<UserListModel> All();

        void EditUser(string id, string firstName, string lastName, int age, string email, string username);

        object GetUserFullName(string id);

        void DeleteUser(string id);

        List<string> FriendsIds(string userId);
    }
}