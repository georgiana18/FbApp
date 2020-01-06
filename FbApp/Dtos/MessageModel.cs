using AutoMapper;
using FbApp.Models;
using System;

namespace FbApp.Dtos
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string MessageText { get; set; }

        public DateTime DateSent { get; set; }

        public bool IsSeen { get; set; }

        public string SenderFullName { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Message, MessageModel>()
                 .ForMember(m => m.SenderFullName, cfg => cfg.MapFrom(m => m.Sender.FirstName + " " + m.Sender.LastName));
        }
    }
}