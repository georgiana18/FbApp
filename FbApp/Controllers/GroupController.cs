using FbApp.Dtos;
using FbApp.Services;
using FbApp.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using FbApp.Models;

namespace FbApp.Controllers
{
    public class GroupController : Controller
    {
        
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        public GroupController()
        {
            this.groupService = new GroupService();
            this.userService = new UserService();
        }

        public GroupController(IGroupService groupService, IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            GroupModel groupModel = new GroupModel();
            groupModel.UsersForDD = groupService.UsersForDD(User.Identity.GetUserId());
            return View(groupModel);
        }

        [HttpPost]
        public ActionResult Create(GroupModel groupModel)
        {
            string userId = User.Identity.GetUserId();
            groupModel.SelectedUserIds = groupModel.SelectedUserIds.Concat(new[] { userId });
           
            this.groupService.Create(groupModel.Name, groupModel.Description, groupModel.SelectedUserIds);
            return Redirect("/");
        }

        public ActionResult Show(string id) // user id 
        {
            ICollection<GroupModel> groupModels = groupService.GetGroupsByUserId(id);
            return View(groupModels);
        }     

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if (!groupService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            groupService.Delete(id);
            return RedirectToAction("Show", new { id = User.Identity.GetUserId() });
        }

        public ActionResult EditGroupInfo(int id)
        {
            if(!this.groupService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            var groupModel = this.groupService.GetGroupById(id);

            return View(groupModel);
        }

        [HttpPost]
        public ActionResult EditGroupInfo(int id, GroupModel groupModel)
        {
            groupService.EditGroupInfo(id, groupModel.Name, groupModel.Description);
            return RedirectToAction("ShowGroup", new { id = groupModel.Id });
        }

        public ActionResult ShowGroup(int id) // album id
        {
            if (!this.groupService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            var groupModel = this.groupService.GetGroupById(id);
            return View(groupModel);
        } 

        public ActionResult AddPeople(int id)
        {
            GroupModel groupModel = new GroupModel();
            groupModel.UsersForDD = groupService.UsersForDD(User.Identity.GetUserId());
            return View(groupModel);
        }

        [HttpPost]
        public ActionResult AddPeople(GroupModel groupModel)
        {
            this.groupService.AddPeopleToGroup(groupModel.Id, groupModel.SelectedUserIds);
            return RedirectToAction("ShowGroup", new { id = groupModel.Id });
        }

        [HttpDelete]
        public ActionResult LeaveGroup(int id, string userId)
        {
            if (!this.groupService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            this.groupService.RemoveUser(id, userId);
            return Redirect("/");
        }

    }
}