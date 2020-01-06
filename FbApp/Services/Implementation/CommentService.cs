using AutoMapper;
using AutoMapper.QueryableExtensions;
using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FbApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public CommentService()
        {
          
        }

        public bool Exists(int id) => this.db.Comments.Any(p => p.Id == id);

        public IEnumerable<CommentModel> CommentsByPostId(int postId)
        {
            return this.db
                .Comments
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.Date)
                .ProjectTo<CommentModel>()
                .ToList();
        }

        public void Create(string commentText, string userId, int postId)
        {
            var comment = new Comment
            {
                Date = DateTime.UtcNow,
                Text = commentText,
                UserId = userId,
                PostId = postId
            };

            this.db.Comments.Add(comment);
            this.db.SaveChanges();
        }

        public void DeleteCommentsByPostId(int postId)
        {
            var comments = this.db.Comments.Where(c => c.PostId == postId);

            foreach (var comment in comments)
            {
                this.db.Comments.Remove(comment);
            }

            this.db.SaveChanges();
        }

        public void Edit(int commentId, string commentText)
        {
            var comment = db.Comments.Find(commentId);
            comment.Text = commentText;
            this.db.SaveChanges();
        }

        public void Delete(int commentId)
        {
            var comment = this.db.Comments.Find(commentId);
            this.db.Comments.Remove(comment);
            this.db.SaveChanges();
        }

        public CommentModel CommentById(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Comment, CommentModel>()
                    .ForMember(p => p.UserProfilePicture, c => c.MapFrom(p => p.User.ProfilePicture))
                    .ForMember(p => p.UserFullName, c => c.MapFrom(p => p.User.FirstName + " " + p.User.LastName));
            });

            IMapper iMapper = config.CreateMapper();

            Comment comment = this.db.Comments.Where(p => p.Id == id).FirstOrDefault();
            CommentModel commentModel = iMapper.Map<Comment, CommentModel>(comment);

            return commentModel;
        }

        public bool UserIsAuthorizedToEdit(int commentId, string userId) => this.db.Comments.Any(p => p.Id == commentId && p.UserId == userId);
    }
}