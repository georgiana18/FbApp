using FbApp.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

       // [Required]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

       // [Required]
        public string ReceiverId { get; set; }

        public ApplicationUser Receiver { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxMessageLength)]
        public string MessageText { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public bool IsSeen { get; set; }
    }
}