using FbApp.Dtos;
using FbApp.Models;
using System.Collections.Generic;

namespace FbApp.Services
{
    public interface IPostService
    {
        int Create(string userId, Feeling feeling, string text, byte[] photo, int albumId);

        void Edit(int postId, Feeling feeling, string text, byte[] photo, int albumId);

        bool Exists(int id);

        bool UserIsAuthorizedToEdit(int postId, string userId);

        IEnumerable<PostModel> PostsByUserId(string userId);

        IEnumerable<PostModel> FriendPostsByUserId(string userId);

        PostModel PostById(int postId);

        void Delete(int postId);

        void Like(int postId);
    }
}