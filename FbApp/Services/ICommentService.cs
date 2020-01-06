using FbApp.Dtos;
using System.Collections.Generic;

namespace FbApp.Services
{
    public interface ICommentService
    {
        void Create(string commentText, string userId, int postId);

        void DeleteCommentsByPostId(int postId);

        IEnumerable<CommentModel> CommentsByPostId(int postId);

        bool Exists(int id);

        CommentModel CommentById(int id);

        bool UserIsAuthorizedToEdit(int commentId, string userId);

        void Edit(int commentId, string commentText);

        void Delete(int commentId);
    }
}