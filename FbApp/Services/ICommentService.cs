using FbApp.Dtos;
using System.Collections.Generic;

namespace FbApp.Services
{
    public interface ICommentService
    {
        void Create(string commentText, string userId, int postId);

        void DeleteCommentsByPostId(int postId);

        IEnumerable<CommentModel> CommentsByPostId(int postId);
    }
}