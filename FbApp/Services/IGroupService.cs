using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Services
{
    public interface IGroupService
    {
        int Create(string name, string description, IEnumerable<string> SelectedUserIds);

        IEnumerable<SelectListItem> UsersForDD(string userId);

        ICollection<GroupModel> GetGroupsByUserId(string userId);

        bool Exists(int groupId);

        void Delete(int groupId);

        GroupModel GetGroupById(int groupId);

        void EditGroupInfo(int id, string newName, string newDescription);

        void AddPeopleToGroup(int groupId, IEnumerable<string> SelectedUserIds);

        void RemoveUser(int groupId, string userId);
    }
}