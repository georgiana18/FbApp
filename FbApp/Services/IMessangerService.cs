using FbApp.Dtos;
using System.Collections.Generic;

namespace FbApp.Services
{
    public interface IMessangerService
    {
        void Create(string senderId, string receiverId, string text);

        IEnumerable<MessageModel> AllByUserIds(string userId, string otherUserId);

        bool UserIsAuthorizedToEdit(int messageId, string userId);
    }
}