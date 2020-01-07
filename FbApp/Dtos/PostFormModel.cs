using FbApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FbApp.Dtos
{
    public class PostFormModel
    {
        [Required]
        [Display(Name = "What do you think?")]
        public string Text { get; set; }

        [Display(Name = "How do you feel?")]
        public Feeling Feeling { get; set; }

        [Display(Name = "Upload a photo")]
        public byte[] Photo { get; set; }

        public IEnumerable<SelectListItem> Albums { get; set; }

        public int AlbumId { get; set; }
    }
}