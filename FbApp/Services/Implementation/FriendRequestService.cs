using FbApp.Models;
using System.Linq;

namespace FbApp.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;

        public FriendRequestService()
        {
            db = new ApplicationDbContext();
            userService = new UserService();
        }

        public void Accept(string senderId, string receiverId)
        {
            if (this.Exists(senderId, receiverId) && this.userService.UserExists(senderId) && this.userService.UserExists(receiverId))
            {
                var friendRequest = db.FriendRequests.FirstOrDefault(fr => fr.ReceiverId == receiverId && fr.SenderId == senderId);
                friendRequest.FriendRequestStatus = FriendRequestStatus.Accepted;
                this.userService.MakeFriends(senderId, receiverId);
                this.db.SaveChanges();
            }
        }

        public void Create(string senderId, string receiverId)
        {
            if (!this.Exists(senderId, receiverId) && this.userService.UserExists(senderId) && this.userService.UserExists(receiverId))
            {
                var friendRequest = new FriendRequest
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    FriendRequestStatus = FriendRequestStatus.Pending
                };

                this.db.FriendRequests.Add(friendRequest);
                this.db.SaveChanges();
            }
        }

        public void Decline(string senderId, string receiverId)
        {
            if (this.Exists(senderId, receiverId) && this.userService.UserExists(senderId) && this.userService.UserExists(receiverId))
            {
                var friendRequest = db.FriendRequests.FirstOrDefault(fr => fr.ReceiverId == receiverId && fr.SenderId == senderId);
                db.FriendRequests.Remove(friendRequest);
                this.db.SaveChanges();
            }
        }

        public bool Exists(string senderId, string receiverId) =>
            this.db.FriendRequests.Any(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);

    }
}