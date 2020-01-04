using AutoMapper;
using FbApp.Models;

namespace FbApp.Dtos
{
    public class UserListModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserListModel>()
                .ForMember(u => u.FullName, cfg => cfg.MapFrom(u => u.FirstName + " " + u.LastName));
        }
    }
}