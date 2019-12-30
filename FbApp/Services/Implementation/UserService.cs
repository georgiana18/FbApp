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
        private readonly IMapper mapper;

        public UserService()
        {
        }

        public UserService(ApplicationDbContext db, IPostService postService, IMapper mapper)
        {
            this.mapper = mapper;
            this.db = db;
            this.postService = postService;
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
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<ApplicationUser, UserAccountModel>();
                });
                IMapper iMapper = config.CreateMapper();

                ApplicationUser user = db.Users.Where(u => u.Id == userId).FirstOrDefault();

                UserAccountModel userAccountModel = iMapper.Map<ApplicationUser, UserAccountModel>(user);

                userAccountModel.Posts = this.postService.PostsByUserId(userId);

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

                var userAccountModel = db
                   .Users
                   .Where(u => u.Id == userId)
                   .ProjectTo<UserAccountModel>(configUser)
                   .FirstOrDefault();

                userAccountModel.Posts = this.postService.FriendPostsByUserId(userId);
                return userAccountModel;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<UserListModel> UsersBySearchTerm(string searchTerm)
        {
            var users = this.db.Users
                .Where(u => (u.FirstName.ToLower().Contains(searchTerm.ToLower())
                || u.LastName.ToLower().Contains(searchTerm.ToLower())
                || u.UserName.ToLower().Contains(searchTerm.ToLower()))
                && u.UserName != "Administrator"
                && u.IsDeleted == false)
                .ProjectTo<UserListModel>();

            return users != null ? users.AsNoTracking() : null;
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
            var users = this.db.Users
                .Where(u => u.UserName != "Administrator" && u.IsDeleted == false)
                .ProjectTo<UserListModel>();

            return users != null ? users.AsNoTracking() : null;
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