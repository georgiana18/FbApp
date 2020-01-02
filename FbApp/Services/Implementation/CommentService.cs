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
    }
}