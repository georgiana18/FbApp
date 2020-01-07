using FbApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FbApp.Dtos
{
    public class AlbumModel
    {
        public int Id { get; set; }

        [Display(Name = "What is the title of your album?")]
        [Required]
        public string Title { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Write a description about your album!")]
        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    }
}