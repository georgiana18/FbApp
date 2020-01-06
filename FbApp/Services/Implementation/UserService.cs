using AutoMapper;
using AutoMapper.QueryableExtensions;
using FbApp.Dtos;
using FbApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FbApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly IPostService postService = new PostService();

        public UserService()
        {

        }

        public bool UserExists(string userId) => this.db.Users.Any(u => u.Id == userId && u.IsDeleted == false);

        public void MakeFriends(string senderId, string receiverId)
        {
            if (this.UserExists(senderId) && this.UserExists(receiverId) && !this.CheckIfFriends(senderId, receiverId))
            {
                var userFriend = new UserFriend
                {
                    UserId = senderId,
                    FriendId = receiverId
                };
                this.db.UserFriends.Add(userFriend);
                this.db.SaveChanges();
            }
        }

        public bool CheckIfFriends(string requestUserId, string targetUserId)
        {
            return this.db.UserFriends.Any(uf =>
            ((uf.UserId == requestUserId && uf.FriendId == targetUserId) || (uf.UserId == targetUserId && uf.FriendId == requestUserId)));
        }

        public UserAccountModel UserDetails(string userId)
        {
            if (this.UserExists(userId))
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ApplicationUser, UserAccountModel>()
                       .ForMember(p => p.FriendRequestSent, c => c.MapFrom(p => p.FriendRequestSent))
                       .ForMember(p => p.FriendRequestReceived, c => c.MapFrom(p => p.FriendRequestReceived));

                    cfg.CreateMap<FriendRequest, ReceivedFriendRequestModel>()
                      .ForMember(f => f.SenderFullName, c => c.MapFrom(f => f.Sender.FirstName + " " + f.Sender.LastName));

                    cfg.CreateMap<FriendRequest, SentFriendRequestModel>();

                    cfg.CreateMap<ApplicationUser, UserListModel>()
                       .ForMember(u => u.FullName, c => c.MapFrom(u => u.FirstName + " " + u.LastName));
                });
                IMapper iMapper = config.CreateMapper();

                ApplicationUser user = db.Users.Where(u => u.Id == userId).FirstOrDefault();

                UserAccountModel userAccountModel = iMapper.Map<ApplicationUser, UserAccountModel>(user);

                var friends = this.db
                    .UserFriends
                    .Where(u => u.UserId == userId && !u.Friend.IsDeleted)
                    .Select(u => u.Friend)
                    .ToList();

                var friendRequestsReceived = this.db.FriendRequests
                    .Where(u => u.ReceiverId == userId)
                    .ToList();

                var receivedFR = iMapper.Map<List<FriendRequest>, List<ReceivedFriendRequestModel>>(friendRequestsReceived);

                var friendRequestsSent = this.db.FriendRequests
                   .Where(u => u.SenderId == userId)
                   .ToList();

                var sentFR = iMapper.Map<List<FriendRequest>, List<SentFriendRequestModel>>(friendRequestsSent);

                var otherFriends = this.db.UserFriends
                    .Where(u => u.FriendId == userId && !u.User.IsDeleted)
                    .Select(u => u.User)
                    .ToList();

                var friendModels = iMapper.Map<List<ApplicationUser>, List<UserListModel>>(friends);
                var otherFriendModels = iMapper.Map<List<ApplicationUser>, List<UserListModel>>(otherFriends);

                //add other friends to friend model in the future
                friendModels.AddRange(otherFriendModels);

                userAccountModel.FriendRequestSent = sentFR;
                userAccountModel.FriendRequestReceived = receivedFR;

                userAccountModel.Friends = friendModels;
                userAccountModel.Posts = this.postService.PostsByUserId(userId);

                userAccountModel.Privacy = user.Privacy;

                return userAccountModel;
            }
            else
            {
                return null;
            }
        }

        public virtual UserAccountModel UserDetailsFriendsCommentsAndPosts(string userId)
        {
            if (this.UserExists(userId))
            {
                var configUser = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ApplicationUser, UserAccountModel>();


                });

                IMapper iMapper = configUser.CreateMapper();

                var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();

                var model = iMapper.Map<ApplicationUser, UserAccountModel>(user);

                model.Posts = this.postService.FriendPostsByUserId(userId);

                return model;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<UserListModel> UsersBySearchTerm(string searchTerm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserListModel>()
                  .ForMember(u => u.FullName, c => c.MapFrom(u => u.FirstName + " " + u.LastName));
            });

            IMapper iMapper = config.CreateMapper();

            var users = this.db.Users
                .Where(u => (u.FirstName.ToLower().Contains(searchTerm.ToLower())
                || u.LastName.ToLower().Contains(searchTerm.ToLower())
                || u.UserName.ToLower().Contains(searchTerm.ToLower()))
                && u.UserName != "Administrator"
                && u.IsDeleted == false)
                .ToList();

            IEnumerable<UserListModel> userModels = iMapper.Map<List<ApplicationUser>, IEnumerable<UserListModel>>(users);

            return userModels;
        }

        public object GetUserFullName(string id)
        {
            if (this.UserExists(id))
            {
                var user = this.db.Users.Find(id);
                return user.FirstName + " " + user.LastName;
            }
            return null;
        }

        public UserModel GetById(string id)
        {
            if (this.UserExists(id))
            {
                return Mapper.Map<UserModel>(this.db.Users.Find(id));
            }

            return null;
        }

        public IEnumerable<UserListModel> All()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserListModel>()
                  .ForMember(u => u.FullName, c => c.MapFrom(u => u.FirstName + " " + u.LastName));
            });

            IMapper iMapper = config.CreateMapper();

            var users = this.db.Users
                .Where(u => u.UserName != "Administrator" && u.IsDeleted == false).ToList();

            IEnumerable<UserListModel> userModels = iMapper.Map<List<ApplicationUser>, IEnumerable<UserListModel>>(users);

            return userModels;
        }

        public void EditUser(string id, string firstName, string lastName, int age, string email, string username)
        {
            var user = this.db.Users.Find(id);

            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserName = username;
            user.Age = age;
            user.Email = email;

            this.db.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var user = this.db.Users.Find(id);

            user.IsDeleted = true;

            this.db.SaveChanges();
        }

        public bool CheckIfDeletedByUserName(string username)
        {
            if (this.db.Users.Any(u => u.UserName == username))
            {
                return this.db.Users.FirstOrDefault(u => u.UserName == username).IsDeleted;
            }

            return true;
        }

        public List<string> FriendsIds(string userId)
        {
            if (this.UserExists(userId))
            {
                var friends = this.db
                    .UserFriends
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Friend.Id)
                    .ToList();

                var otherFriends = this.db
                    .UserFriends
                    .Where(u => u.FriendId == userId)
                    .Select(u => u.User.Id)
                    .ToList();

                friends.AddRange(otherFriends);

                return friends;
            }
            else
            {
                return null;
            }
        }
    }
}