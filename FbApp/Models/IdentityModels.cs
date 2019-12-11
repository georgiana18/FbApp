using System;
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

        //[Required]
        //[MaxLength(DataConstants.MaxPhotoLength)]
        public byte[] ProfilePicture { get; set; }

        public ICollection<Album> Albums { get; set; }

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
        public DbSet<Photo> Photos { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}