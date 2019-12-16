using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public byte[] Photo { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}