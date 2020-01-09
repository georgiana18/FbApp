using FbApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FbApp.Models
{
    public class MessageGroup
    {
        [Key]
        public int Id { get; set; }

        // [Required]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        // [Required]
        public int GroupId { get; set; } 

        public virtual Group Group { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxMessageLength)]
        public string MessageText { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public bool IsSeen { get; set; }
    }
}