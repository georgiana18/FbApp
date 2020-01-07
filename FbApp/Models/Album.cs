using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FbApp.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string UserId { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}