using AutoMapper;
using FbApp.Models;

namespace FbApp.Dtos
{
    public class SentFriendRequestModel
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverFullName { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; }

        public void ConfigureMapping(Profile profile)
        {
            profile.CreateMap<FriendRequest, SentFriendRequestModel>()
                .ForMember(f => f.ReceiverFullName, cfg => cfg.MapFrom(f => f.Receiver.FirstName + " " + f.Receiver.LastName));
        }
    }
}