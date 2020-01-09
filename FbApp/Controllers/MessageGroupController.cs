using FbApp.Models;
using FbApp.Services;
using FbApp.Services.Implementation;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class MessageGroupController : Controller
    {
        private readonly IMessangerGroupService messangerService = new MessangerGroupService();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id) // messageId
        {
            var message = this.db.MessageGroups.Find(id);
            return View(message);
        }

        [HttpPost]
        public ActionResult Edit(int id, MessageGroup newMessage)
        {
            if (!this.messangerService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            if (newMessage == null)
            {
                throw new ArgumentNullException(nameof(newMessage));
            }

            var message = this.db.MessageGroups.Find(id);
            message.MessageText = newMessage.MessageText;

            db.SaveChanges();

            int groupId = this.db.MessageGroups.Find(id).GroupId;
            return RedirectToAction("Index", "MessangerGroup", new { id = groupId });
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var message = this.db.MessageGroups.Find(id);
            int groupId = message.GroupId;
            this.db.MessageGroups.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index", "MessangerGroup", new { id = groupId });
        }
    }
}