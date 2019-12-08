using System.Collections.Generic;

namespace FbApp.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}