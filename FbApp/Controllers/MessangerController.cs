using FbApp.Dtos;
using FbApp.Services;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class MessangerController : Controller
    {
        private readonly IUserService userService;
        private readonly IMessangerService messangerService;

        public MessangerController()
        {
            this.userService = new UserService();
            this.messangerService = new MessangerService();
        }

        public ActionResult Index(string id)
        {
            if (id == this.User.Identity.GetUserId() || !this.userService.CheckIfFriends(this.User.Identity.GetUserId(), id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }

            var messangerModel = new MessangerModel();

            string compositeChatId = string.Compare(User.Identity.GetUserId(), id) > 0 ? User.Identity.GetUserId() + id : id + User.Identity.GetUserId();

            ViewData[GlobalConstants.CompsiteChatId] = compositeChatId;
            ViewData[GlobalConstants.CounterPartFullName] = this.userService.GetUserFullName(id);

            messangerModel.Messages = this.messangerService.AllByUserIds(User.Identity.GetUserId(), id);

            return View(messangerModel);
        }

        [HttpPost]
        public ActionResult Index(string id, int? pageIndex, MessangerModel model)
        {
            if (id == this.User.Identity.GetUserId() || !this.userService.CheckIfFriends(this.User.Identity.GetUserId(), id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }

            this.messangerService.Create(User.Identity.GetUserId(), id, model.MessageText);
            return RedirectToAction(nameof(Index), new { id, pageIndex });
        }
       
    }
}