using AutoMapper;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FbApp.Dtos
{
    public class PostModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int Likes { get; set; }

        public byte[] Photo { get; set; }

        public byte[] UserProfilePicture { get; set; }

        public string UserId { get; set; }

        public string UserFullName { get; set; }

        public Feeling Feeling { get; set; }

        public int AlbumId { get; set; }

        public IEnumerable<SelectListItem> Albums { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<Post, PostModel>()
                .ForMember(p => p.UserProfilePicture, cfg => cfg.MapFrom(p => p.User.ProfilePicture))
                .ForMember(p => p.UserFullName, cfg => cfg.MapFrom(p => p.User.FirstName + " " + p.User.LastName))
                .ForMember(p => p.Comments, cfg => cfg.MapFrom(p => p.Comments));
        }
    }
}