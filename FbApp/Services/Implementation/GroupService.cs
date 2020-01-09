using AutoMapper;
using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly IUserService userService = new UserService();

        public GroupService()
        {
        }

        //creates a group with default user 
        public int Create(string name, string description, IEnumerable<string> SelectedUserIds)
        {

            var users = new List<ApplicationUser>();

            foreach (var userId in SelectedUserIds)
            {
                ApplicationUser user = db.Users.Find(userId);
                users.Add(user);
            }

            var group = new Group
            {
                Name = name,
                Description = description,
                Users = users
            };

            db.Groups.Add(group);
            db.SaveChanges();
            return group.Id;
        }

        public IEnumerable<SelectListItem> UsersForDD(string userId)
        {
            var selectList = new List<SelectListItem>();
            var users = userService.All();
            users.ToList().Remove(users.First(x => x.Id == userId));

            foreach (var user in users)
            {
                selectList.Add(new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = user.FullName.ToString()
                });
            }
            return selectList;
        }

        public IEnumerable<SelectListItem> GetDefaultUsersForDD(string userId)
        {
            var selectList = new List<SelectListItem>();
            var users = userService.All().Where(x => x.Id == userId);
            foreach (var user in users)
            {
                selectList.Add(new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = user.FullName.ToString()
                });
            }
            return selectList;
        }

        public ICollection<GroupModel> GetGroupsByUserId(string userId)
        {
            ICollection<GroupModel> groupsOfUser = new List<GroupModel>();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Group, GroupModel>();
            });

            var groups = this.db.Groups.Select(x => x).ToList();
            IMapper iMapper = config.CreateMapper();

            foreach (var group in groups)
            {
                foreach (var user in group.Users)
                {
                    if(user.Id == userId)
                    {
                        GroupModel groupModel = iMapper.Map<Group, GroupModel>(group);
                        groupsOfUser.Add(groupModel);
                    }
                }
            }
            return groupsOfUser;
        }

        public bool Exists(int groupId) => this.db.Groups.Any(p => p.Id == groupId);

        public void Delete(int groupId)
        {
            var group = this.db.Groups.Find(groupId);
            this.db.Groups.Remove(group);
            this.db.SaveChanges();
        }

        public GroupModel GetGroupById(int groupId)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Group, GroupModel>();
            });

            IMapper iMapper = config.CreateMapper();

            Group group = this.db.Groups.Where(p => p.Id == groupId).FirstOrDefault();
            GroupModel groupModel = iMapper.Map<Group, GroupModel>(group);

            return groupModel;
        }

        public void EditGroupInfo(int id, string newName, string newDescription)
        {
            Group group = db.Groups.Find(id);
            group.Name = newName;
            group.Description = newDescription;
            db.SaveChanges();
        }

        public void AddPeopleToGroup(int groupId, IEnumerable<string> SelectedUserIds)
        {
            var group = this.db.Groups.Find(groupId);

            foreach (var userId in SelectedUserIds)
            {
                ApplicationUser user = db.Users.Find(userId);
                if (!group.Users.Contains(user))
                {
                    group.Users.Add(user);
                }
            }
            db.SaveChanges(); 
        }

        public void RemoveUser(int groupId, string userId)
        {
            var group = this.db.Groups.Find(groupId);
            var user = this.db.Users.Find(userId);

            group.Users.Remove(user);
            db.SaveChanges();
        }

    }
} 