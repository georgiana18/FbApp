using FbApp.Utilities;
using System.ComponentModel.DataAnnotations;

namespace FbApp.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(DataConstants.MaxPhotoLength)]
        public byte[] PhotoAsBytes { get; set; }

        public int PostId { get; set; }
  
        public virtual Post Post { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}