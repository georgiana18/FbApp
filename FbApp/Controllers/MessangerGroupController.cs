using FbApp.Services;
using FbApp.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using FbApp.Dtos;
using FbApp.Utilities;

namespace FbApp.Controllers
{
    public class MessangerGroupController : Controller
    {
        private readonly IUserService userService;
        private readonly IMessangerGroupService messangerGroupService;

        public MessangerGroupController()
        {
            this.userService = new UserService();
            this.messangerGroupService = new MessangerGroupService();
        }

        public ActionResult Index(int id)
        {
            var messangerGroupModel = new MessangerGroupModel();

            //string compositeChatId = string.Compare(User.Identity.GetUserId(), id) > 0 ? User.Identity.GetUserId() + id : id + User.Identity.GetUserId();

            //ViewData[GlobalConstants.CompsiteChatId] = compositeChatId;
            //ViewData[GlobalConstants.CounterPartFullName] = this.userService.GetUserFullName(id);

            messangerGroupModel.Messages = this.messangerGroupService.AllByUserIds(User.Identity.GetUserId(), id);

            return View(messangerGroupModel);
        }

        [HttpPost]
        public ActionResult Index(int id, int? pageIndex, MessangerGroupModel model)
        {
            this.messangerGroupService.Create(User.Identity.GetUserId(), id, model.MessageText);
            return RedirectToAction(nameof(Index), new { id, pageIndex });
        }
       
    }
}