using AutoMapper;
using FbApp.Models;
using System;

namespace FbApp.Dtos
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public int PostId { get; set; }

        public byte[] UserProfilePicture { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Comment, CommentModel>()
                .ForMember(c => c.UserProfilePicture, cfg => cfg.MapFrom(c => c.User.ProfilePicture))
                .ForMember(c => c.UserFullName, cfg => cfg.MapFrom(c => c.User.FirstName + " " + c.User.LastName));
        }
    }
}