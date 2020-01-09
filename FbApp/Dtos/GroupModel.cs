using FbApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Dtos
{
    public class GroupModel
    {
        public int Id { get; set; }

        [Display(Name = "Enter a name for the group")]
        public string Name { get; set; }

        [Display(Name = "Enter a group description")]
        public string Description { get; set; }

        public IEnumerable<SelectListItem> UsersForDD { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> SelectedUserIds { get; set; } = new List<string>();

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}