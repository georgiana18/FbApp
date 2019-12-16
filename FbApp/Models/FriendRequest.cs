using System.ComponentModel.DataAnnotations;

namespace FbApp.Models
{
    public enum FriendRequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Declined = 2,
        Blocked = 3
    }

    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}