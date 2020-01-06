using AutoMapper;
using FbApp.Models;
using System.Collections.Generic;

namespace FbApp.Dtos
{
    public class UserAccountModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public byte[] ProfilePicture { get; set; }

        public IEnumerable<PostModel> Posts { get; set; } = null;

        public IEnumerable<ReceivedFriendRequestModel> FriendRequestReceived { get; set; } = new List<ReceivedFriendRequestModel>();

        public IEnumerable<SentFriendRequestModel> FriendRequestSent { get; set; } = new List<SentFriendRequestModel>();

        public IEnumerable<UserListModel> Friends { get; set; } = new List<UserListModel>();

        public string Privacy;

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserAccountModel>()
                .ForMember(u => u.Posts, cfg => cfg.Ignore())
                .ForMember(u => u.Friends, cfg => cfg.Ignore());
        }
    }
}