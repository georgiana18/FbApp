using AutoMapper;
using FbApp.Dtos;
using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Services.Implementation
{
    public class AlbumService : IAlbumService
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public AlbumService()
        {

        }

        public void Create(string title, string userId, string description)
        {
            var album = new Album
            {
                Title = title,
                Description = description,
                UserId = userId
            };
            db.Albums.Add(album);
            db.SaveChanges();
        }

        public bool Exists(int albumId) => this.db.Albums.Any(p => p.Id == albumId);

        public void AddPost(int albumId, int postId)
        {
            Album album = this.db.Albums.Find(albumId);
            album.Posts.Add(this.db.Posts.Find(postId));
            db.SaveChanges();
        }

        public void RemovePost(int albumId, int postId)
        {
            Album album = this.db.Albums.Find(albumId);
            //album.Posts.Remove(this.db.Posts.Find(postId));
            //db.SaveChanges();
        }

        public ICollection<Album> AllAlbumsByUser(string userId)
        {
            var albums = new List<Album>();

            albums = db.Albums.Select(x => x).Where(x => x.UserId == userId).ToList();

            return albums;
        }

        public AlbumModel AlbumById(int albumId)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Album, AlbumModel>();
            });

            IMapper iMapper = config.CreateMapper();

            Album album = this.db.Albums.Where(p => p.Id == albumId).FirstOrDefault();
            AlbumModel model = iMapper.Map<Album, AlbumModel>(album);

            return model;
        }

        public IEnumerable<SelectListItem> AlbumsByUserForDD(string userId)
        {
            var selectList = new List<SelectListItem>();
            var albums = db.Albums.Select(x => x).Where(x => x.UserId == userId).ToList();
            foreach (var album in albums)
            {
                selectList.Add(new SelectListItem
                {
                    Value = album.Id.ToString(),
                    Text = album.Title.ToString()
                });
            }
            return selectList;
        }

        public void EditAlbumInfo(int id, string newTitle, string newDescription)
        {
            Album album = db.Albums.Find(id);
            album.Title = newTitle;
            album.Description = newDescription;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
        }
    }
}