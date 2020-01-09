using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FbApp.Dtos;
using FbApp.Models;

namespace FbApp.Services.Implementation
{
    public class MessangerGroupService : IMessangerGroupService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly UserService userService = new UserService();

        public MessangerGroupService()
        {
        }

        public List<MessageGroupModel> All()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageGroup, MessageGroupModel>();
            });
            IMapper iMapper = config.CreateMapper();

            var messages = this.db.MessageGroups.ToList();

            var messageModels = iMapper.Map<List<MessageGroup>, List<MessageGroupModel>>(messages);

            return messageModels;

        }

        public IEnumerable<MessageGroupModel> AllByUserIds(string userId, int groupId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageGroup, MessageGroupModel>();
            });

            IMapper iMapper = config.CreateMapper();

            var messages = this.db.MessageGroups
               .Where(m => (m.GroupId == groupId))
               .OrderBy(m => m.DateSent)
               .ToList();
            
            var messageModels = iMapper.Map<List<MessageGroup>, List<MessageGroupModel>>(messages);
            foreach (var messageModel in messageModels)
            {
                messageModel.SenderFullName = userService.GetUserFullName(messageModel.SenderId).ToString();
            }
            return messageModels;
        }
   
        public void Create(string senderId, int groupId, string text)
        {
            var messageGroup = new MessageGroup
            {
                SenderId = senderId,
                GroupId = groupId,
                DateSent = DateTime.UtcNow,
                IsSeen = false,
                MessageText = text
            };

            this.db.MessageGroups.Add(messageGroup);
            this.db.SaveChanges();
        }

        public bool UserIsAuthorizedToEdit(int messageId, string userId) => this.db.MessageGroups.Any(p => p.Id == messageId && p.SenderId == userId);
    }
}