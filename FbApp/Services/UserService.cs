using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FbApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool UserExists(string userId) => this.db.Users.Any(u => u.Id == userId && u.IsDeleted == false);


        public void MakeFriends(string senderId, string receiverId)
        {
            if (this.UserExists(senderId) && this.UserExists(receiverId) && !this.CheckIfFriends(senderId, receiverId))
            {
                var userFriend = new UserFriend
                {
                    UserId = senderId,
                    FriendId = receiverId
                };
                this.db.UserFriends.Add(userFriend);
                this.db.SaveChanges();
            }
        }

        public bool CheckIfFriends(string requestUserId, string targetUserId)
        {
            return this.db.UserFriends.Any(uf =>
            (uf.UserId == requestUserId && uf.FriendId == targetUserId) || (uf.UserId == targetUserId && uf.FriendId == requestUserId));
        }

        //public UserAccountModel UserDetails(string userId)
        //{

        //}

    }
}