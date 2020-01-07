using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Services
{
    public interface IAlbumService
    {
        void Create(string title, string userId, string description);

        ICollection<Album> AllAlbumsByUser(string userId);

        bool Exists(int albumId);

        void AddPost(int albumId, int postId);

        AlbumModel AlbumById(int albumId);

        IEnumerable<SelectListItem> AlbumsByUserForDD(string userId);

        void RemovePost(int albumId, int postId);

        void EditAlbumInfo(int id, string newTitle, string newDescription);

        void Delete(int id);
    }
}