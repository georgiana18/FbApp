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
    public class MessangerGroupModel
    {
        public IEnumerable<MessageGroupModel> Messages { get; set; } = new List<MessageGroupModel>();

        [Required]
        [MaxLength(DataConstants.MaxMessageLength)]
        public string MessageText { get; set; }
    }
}