namespace FbApp.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }
       // public int UserId { get; set; }
        public virtual Album Album { get; set; }
       // public virtual ApplicationUser User { get; set; }
    }
}