using FbApp.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FbApp.Services
{
    public interface IMessangerGroupService
    {
        void Create(string senderId, int groupId, string text);

        IEnumerable<MessageGroupModel> AllByUserIds(string userId, int groupId);

        bool UserIsAuthorizedToEdit(int messageId, string userId);
    }
}