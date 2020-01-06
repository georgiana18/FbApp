using FbApp.Dtos;
using FbApp.Models;
using FbApp.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessangerService messangerService = new MessangerService();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id) // messageId
        {
            var message = this.db.Messages.Find(id);
            return View(message);
        }

        [HttpPost]
        public ActionResult Edit(int id, Message newMessage)
        {
            if (!this.messangerService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            if (newMessage == null)
            {
                throw new ArgumentNullException(nameof(newMessage));
            }

            var message = this.db.Messages.Find(id);
            message.MessageText = newMessage.MessageText;

            db.SaveChanges();

            string receiverId = this.db.Messages.Find(id).ReceiverId;
            return RedirectToAction("Index", "Messanger", new { id = receiverId });
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var message = this.db.Messages.Find(id);
            string receiverId = message.ReceiverId;
            this.db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index", "Messanger", new { id = receiverId });
        }
    }
}