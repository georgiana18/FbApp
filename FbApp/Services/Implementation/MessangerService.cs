using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FbApp.Dtos;
using FbApp.Models;

namespace FbApp.Services
{
    public class MessangerService : IMessangerService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public MessangerService()
        {
        }

        public List<MessageModel> All()
        {
            var config = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<Message, MessageModel>()
                    .ForMember(m => m.SenderFullName, c => c.MapFrom(m => m.Sender.FirstName + " " + m.Sender.LastName));
           });
            IMapper iMapper = config.CreateMapper();

            var messages = this.db.Messages.ToList();

            var messageModels = iMapper.Map<List<Message>, List<MessageModel>>(messages);

            return messageModels;

        }

        public IEnumerable<MessageModel> AllByUserIds(string userId, string otherUserId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Message, MessageModel>()
                     .ForMember(m => m.SenderFullName, c => c.MapFrom(m => m.Sender.FirstName + " " + m.Sender.LastName));
            });

            IMapper iMapper = config.CreateMapper();

            var messages = this.db.Messages
               .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) || (m.SenderId == otherUserId && m.ReceiverId == userId))
               .OrderBy(m => m.DateSent)
               .ToList();

            var messageModels = iMapper.Map<List<Message>, IEnumerable<MessageModel>>(messages);
            return messageModels;
        }

        public void Create(string senderId, string receiverId, string text)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                DateSent = DateTime.UtcNow,
                IsSeen = false,
                MessageText = text
            };

            this.db.Messages.Add(message);
            this.db.SaveChanges();
        }

        public bool UserIsAuthorizedToEdit(int messageId, string userId) => this.db.Messages.Any(p => p.Id == messageId && p.SenderId == userId);
    }
}