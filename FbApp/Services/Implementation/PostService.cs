using AutoMapper;
using AutoMapper.QueryableExtensions;
using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FbApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly IPhotoService photoService;
        private readonly ICommentService commentService;
        private readonly IMapper mapper;

        public PostService()
        {
        }

        public PostService(ApplicationDbContext db,
                           IPhotoService photoService,
                           ICommentService commentService,
                           IMapper mapper)
        {
            this.mapper = mapper;
            this.db = db;
            this.photoService = photoService;
            this.commentService = commentService;
        }

        public void Create(string userId, Feeling feeling, string text, byte[] photo)
        {
            var post = new Post
            {
                UserId = userId,
                Feeling = feeling,
                Text = text,
                Likes = 0,
                Date = DateTime.UtcNow,
                Photo = photo ?? null
            };

            db.Posts.Add(post);
            db.SaveChanges();
        }

        public void Delete(int postId)
        {
            var post = this.db.Posts.Find(postId);
            this.commentService.DeleteCommentsByPostId(postId);
            this.db.Posts.Remove(post);
            this.db.SaveChanges();
        }

        public void Edit(int postId, Feeling feeling, string text, byte[] photo)
        {
            var post = this.db.Posts.Find(postId);
            post.Feeling = feeling;
            post.Text = text;
            post.Photo = photo ?? null;
            this.db.SaveChanges();
        }

        public bool Exists(int id) => this.db.Posts.Any(p => p.Id == id);

        public IEnumerable<PostModel> FriendPostsByUserId(string userId)
        {
            var config = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<Post, PostModel>();
               cfg.CreateMap<Comment, CommentModel>();
           });

            IMapper iMapper = config.CreateMapper();

            var friendListIds = this.FriendsIds(userId);

            var posts = this.db
                .Posts
                .Where(p => friendListIds.Contains(p.UserId) || p.UserId == userId)
                //.Include(p => p.Comments.Select(y=> y.User))
                .Include(p => p.Comments)
                .OrderByDescending(p => p.Date)
                .ToList();

            IEnumerable<PostModel> postsModels = iMapper.Map<List<Post>, IEnumerable<PostModel>>(posts);

            return postsModels ?? null;
        }

        public void Like(int postId)
        {
            if (this.Exists(postId))
            {
                var post = this.db.Posts.Find(postId);
                post.Likes++;
                this.db.SaveChanges();
            }
        }

        public PostModel PostById(int postId)
        {
            return this.db.Posts.Where(p => p.Id == postId).ProjectTo<PostModel>().FirstOrDefault();
        }

        public IEnumerable<PostModel> PostsByUserId(string userId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostModel>()
                 .ForMember(p => p.UserFullName, c => c.MapFrom(p => p.User.FirstName + " " + p.User.LastName));
                cfg.CreateMap<Comment, CommentModel>();
            });

            IMapper iMapper = config.CreateMapper();

            var posts = this.db
                .Posts
                .Where(p => p.UserId == userId)
                //.Include(p => p.Comments.Select(y=> y.User))
               // .Include(p => p.Comments)
                .OrderByDescending(p => p.Date)
                .ToList();

            IEnumerable<PostModel> postsModels = iMapper.Map<List<Post>, IEnumerable<PostModel>>(posts);

            return postsModels;
        }

        public bool UserIsAuthorizedToEdit(int postId, string userId) => this.db.Posts.Any(p => p.Id == postId && p.UserId == userId);

        private List<string> FriendsIds(string userId)
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
    }
}