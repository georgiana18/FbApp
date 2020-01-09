using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FbApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MinLength(DataConstants.NameMinLength), MaxLength(DataConstants.NameMaxLength)]
        public string FirstName { get; set; }

        [MinLength(DataConstants.NameMinLength), MaxLength(DataConstants.NameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [Range(DataConstants.MinUserAge, DataConstants.MaxUserAge)]
        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Privacy { get; set; }

        public IEnumerable<Photo> Photos { get; set; } = new List<Photo>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<FriendRequest> FriendRequestSent { get; set; } = new List<FriendRequest>();

        public ICollection<FriendRequest> FriendRequestReceived { get; set; } = new List<FriendRequest>();

        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();

        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();

        public ICollection<UserFriend> Friends { get; set; } = new List<UserFriend>();

        public ICollection<UserFriend> OtherFriends { get; set; } = new List<UserFriend>();

        public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

        public ApplicationUser() : base()
        {
        }

        public IEnumerable<SelectListItem> AllRoles { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Album> Albums { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<UserFriend> UserFriends { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<MessageGroup> MessageGroups { get; set; }

      //  public DbSet<ApplicationUser> Users { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
            //modelBuilder.Entity<Comment>().HasRequired(s => s.Post).WithMany(g => g.Comments).HasForeignKey(s => s.PostId);
        }
    }
}