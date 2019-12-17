using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FbApp.Models
{
    public class UserAccountModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }

        public IEnumerable<Post> Albums { get; set; } = new List<Post>();

        public IEnumerable<ApplicationUser> Friends { get; set; } = new List<ApplicationUser>();
        
    }
}