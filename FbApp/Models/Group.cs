using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public IEnumerable<SelectListItem> UsersForDD { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> SelectedUserIds { get; set; } = new List<string>();

        public virtual ICollection<MessageGroup> MessageGroups { get; set; } = new List<MessageGroup>();
    }
}