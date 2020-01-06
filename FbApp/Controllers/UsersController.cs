using FbApp.Dtos;
using FbApp.Models;
using FbApp.Services;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public UsersController()
        {
            userService = new UserService();
        }

        public ActionResult Index()
        {
            UserAccountModel user = this.userService.UserDetailsFriendsCommentsAndPosts(this.User.Identity.GetUserId());

            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;
            return View(user);
        }


        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string id, ApplicationUser newData)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            var userRole = user.Roles.FirstOrDefault();
            ViewBag.userRole = userRole.RoleId;

            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (TryUpdateModel(user))
                {
                    user.UserName = newData.UserName;
                    user.Email = newData.Email;
                    user.PhoneNumber = newData.PhoneNumber;
                    var roles = from role in db.Roles select role;
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(id, role.Name);
                    }
                    var selectedRole =
                    db.Roles.Find(HttpContext.Request.Params.Get("newRole"));
                    UserManager.AddToRole(id, selectedRole.Name);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                return View(user);
            }
        }

        public ActionResult AccountDetails(string id)
        {
            string requestUserId = User.Identity.GetUserId();
            if (requestUserId == id || User.IsInRole("Administrator"))
            {
                ViewData[GlobalConstants.Authorization] = GlobalConstants.FullAuthorization;
            }
            else if (this.userService.UserDetails(id).Privacy == "Public" && !this.userService.CheckIfFriends(requestUserId, id))
            {
                ViewData[GlobalConstants.Authorization] = GlobalConstants.PublicAuthorization;
            }
            else if (this.userService.UserDetails(id).Privacy == "Public" && this.userService.CheckIfFriends(requestUserId, id))
            {
                ViewData[GlobalConstants.Authorization] = GlobalConstants.FriendAuthorization;
            }
            else if(this.userService.UserDetails(id).Privacy == "Private" && this.userService.CheckIfFriends(requestUserId, id))
            {
                ViewData[GlobalConstants.Authorization] = GlobalConstants.FriendAuthorization;
            }
            else
            {
                ViewData[GlobalConstants.Authorization] = GlobalConstants.NoAuthorization;
            }

            UserAccountModel user = this.userService.UserDetails(id);

            return View(user);
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }


        public ActionResult Search(string searchTerm, int? page)
        {
            ViewData[GlobalConstants.SearchTerm] = searchTerm;

            if (string.IsNullOrEmpty(searchTerm))
            {
                var users = this.userService.All();
                return View(users);
            }
            else
            {
                var users = this.userService.UsersBySearchTerm(searchTerm);
                return View(users);
            }
        }

    }


}