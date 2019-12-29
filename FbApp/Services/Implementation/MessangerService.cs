using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper.QueryableExtensions;
using FbApp.Dtos;
using FbApp.Models;

namespace FbApp.Services.Implementation
{
    public class MessangerService : IMessangerService
    {
        private readonly ApplicationDbContext db;

        public MessangerService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<MessageModel> All()
        {
            return this.db
                .Messages
                .ProjectTo<MessageModel>()
                .ToList();
        }

        public IEnumerable<MessageModel> AllByUserIds(string userId, string otherUserId)
        {
            var messages = this.db
               .Messages
               .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) || (m.SenderId == otherUserId && m.ReceiverId == userId))
               .OrderBy(m => m.DateSent)
               .ProjectTo<MessageModel>();

            return messages?.AsNoTracking();
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
    }
}