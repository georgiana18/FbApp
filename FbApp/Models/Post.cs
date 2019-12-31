using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Models
{
    public enum Feeling
    {
        Happy = 0,
        Luky = 1,
        Great = 2,
        Dissapointed = 3,
        Miserable = 4
    }

    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public Feeling Feeling { get; set; }

        [Range(0, int.MaxValue)]
        public int Likes { get; set; }

        public DateTime Date { get; set; }

        public byte[] Photo { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}