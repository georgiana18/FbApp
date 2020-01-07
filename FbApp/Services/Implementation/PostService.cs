using AutoMapper;
using FbApp.Dtos;
using FbApp.Models;
using FbApp.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FbApp.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly IPhotoService photoService = new PhotoService();
        private readonly ICommentService commentService = new CommentService();
        private readonly IAlbumService albumService = new AlbumService();

        public PostService()
        {
        }

        public int Create(string userId, Feeling feeling, string text, byte[] photo, int albumId)
        {
            var post = new Post
            {
                UserId = userId,
                Feeling = feeling,
                Text = text,
                Likes = 0,
                Date = DateTime.UtcNow,
                Photo = photo ?? null,
                Album = db.Albums.Find(albumId)
            };
            db.Posts.Add(post);
            db.SaveChanges();
            return post.Id;
        }

        public void Delete(int postId)
        {
            var post = this.db.Posts.Find(postId);
            this.commentService.DeleteCommentsByPostId(postId);
            this.db.Posts.Remove(post);
            this.db.SaveChanges();
        }

        public void Edit(int postId, Feeling feeling, string text, byte[] photo, int albumId)
        {
            var post = this.db.Posts.Find(postId);
            this.albumService.RemovePost(post.AlbumId, postId);
            post.Feeling = feeling;
            post.Text = text;
            post.Photo = photo ?? null;
            post.AlbumId = albumId;
            post.Album = db.Albums.Find(albumId);
            this.albumService.AddPost(albumId, postId);
            this.db.SaveChanges();
        }

        public bool Exists(int id) => this.db.Posts.Any(p => p.Id == id);

        public IEnumerable<PostModel> FriendPostsByUserId(string userId)
        {
            var config = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<Post, PostModel>()
                 .ForMember(p => p.UserFullName, c => c.MapFrom(p => p.User.FirstName + " " + p.User.LastName))
                 .ForMember(p => p.Comments, c => c.MapFrom(p => p.Comments));
               cfg.CreateMap<Comment, CommentModel>();
           });

            IMapper iMapper = config.CreateMapper();

            var friendListIds = this.FriendsIds(userId);

            var posts = this.db
                .Posts
                .Where(p => friendListIds.Contains(p.UserId) || p.UserId == userId)
                .Include(p => p.Comments.Select(y => y.User))
                .OrderByDescending(p => p.Date)
                .ToList();

            var postsModels = iMapper.Map<List<Post>, IEnumerable<PostModel>>(posts);

            return postsModels;
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostModel>()
                 .ForMember(p => p.UserFullName, c => c.MapFrom(p => p.User.FirstName + " " + p.User.LastName))
                 .ForMember(p => p.Comments, c => c.MapFrom(p => p.Comments));
            });

            IMapper iMapper = config.CreateMapper();

            Post post = this.db.Posts.Where(p => p.Id == postId).FirstOrDefault();
            PostModel model = iMapper.Map<Post, PostModel>(post);

            return model;
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

            var posts = this.db.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments.Select(y => y.User))
                .OrderByDescending(p => p.Date)
                .ToList();

            var postsModels = iMapper.Map<List<Post>, IEnumerable<PostModel>>(posts);

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