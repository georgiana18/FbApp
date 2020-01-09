using AutoMapper;
using FbApp.Models;
using FbApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FbApp.Dtos
{
    public class MessageGroupModel
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string MessageText { get; set; }

        public DateTime DateSent { get; set; }

        public ApplicationUser Sender { get; set; }

        public string SenderFullName { get; set; }

        public bool IsSeen { get; set; }

        public int GroupId { get; set; }

    }
}