using FbApp.Services;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class FriendRequestsController : Controller
    {
        private readonly IFriendRequestService friendRequestService;
        private readonly IUserService userService;

        public FriendRequestsController(IFriendRequestService friendRequestService, IUserService userService)
        {
            this.friendRequestService = friendRequestService;
            this.userService = userService;
        }


        public ActionResult AddFriend(string senderId, string receiverId)
        {
            if (!this.userService.UserExists(senderId) || !this.userService.UserExists(receiverId) || senderId != User.Identity.GetUserId())
            {
                throw new HttpException(404, "User not found");
            }

            this.friendRequestService.Create(senderId, receiverId);

            return RedirectToAction("AccountDetails", "Users", new { id = receiverId });
        }

        public ActionResult Accept(string senderId, string receiverId)
        {
            this.friendRequestService.Accept(senderId, receiverId);
            return RedirectToAction("AccountDetails", "Users", new { id = senderId });
        }

        public ActionResult Decline(string senderId, string receiverId)
        {
            this.friendRequestService.Decline(senderId, receiverId);
            return RedirectToAction("AccountDetails", "Users", new { id = senderId });
        }

    }
}