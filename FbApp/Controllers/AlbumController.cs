using FbApp.Dtos;
using FbApp.Models;
using FbApp.Services;
using FbApp.Services.Implementation;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService albumService = new AlbumService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // GET: Album
        [HttpPost]
        public ActionResult Create(AlbumModel album)
        {
            this.albumService.Create(album.Title, User.Identity.GetUserId(), album.Description);
            return Redirect("/");
        }

        public ActionResult Show(string id) /// user.Id
        {
            var albumsByUser = this.albumService.AllAlbumsByUser(id);
            return View(albumsByUser);
        }

        public ActionResult EditAlbumInfo(int id)
        {
            if (!this.albumService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }

            var albumModel = this.albumService.AlbumById(id);
            
            return View(albumModel);
        }

        [HttpPost]
        public ActionResult EditAlbumInfo(int id, AlbumModel albumModel)
        {
            albumService.EditAlbumInfo(id, albumModel.Title, albumModel.Description);
            return RedirectToAction("ShowAlbum", new { id = albumModel.Id });
        }

        public ActionResult ShowAlbum(int id) // album id
        {
            if (!this.albumService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            var albumModel = this.albumService.AlbumById(id);
            return View(albumModel);
        }

        public ActionResult Delete(int id)
        {
            if (!this.albumService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }
            albumService.Delete(id);
            return RedirectToAction("Show", new { id = User.Identity.GetUserId()});
        }
    }
}