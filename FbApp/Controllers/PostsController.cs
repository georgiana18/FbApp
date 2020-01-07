using FbApp.Dtos;
using FbApp.Services;
using FbApp.Services.Implementation;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IAlbumService albumService;

        public PostsController()
        {
            this.postService = new PostService();
            this.albumService = new AlbumService();
        }

        public PostsController(IPostService postService, IAlbumService albumService)
        {
            this.postService = postService;
            this.albumService = albumService;
        }

        public ActionResult Create()
        {
            PostFormModel postFormModel = new PostFormModel();
            if (albumService.AllAlbumsByUser(User.Identity.GetUserId()).Count == 0)
            {
                albumService.Create("Default Album", User.Identity.GetUserId(), "Your Default Album");
            }
            postFormModel.Albums = this.albumService.AlbumsByUserForDD(User.Identity.GetUserId());
            return View(postFormModel);
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Photo")]  PostFormModel model)
        {
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["Photo"];
                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            if (imageData.Length > DataConstants.MaxPhotoLength)
            {
                ModelState.AddModelError(string.Empty, "Your photo should be a valid image file with max size 5MB!");
                return View(model);
            }

            int postId = this.postService.Create(this.User.Identity.GetUserId(), model.Feeling, model.Text, imageData, model.AlbumId);
            this.albumService.AddPost(model.AlbumId, postId);
            return Redirect("/");
        }

        public ActionResult Edit(int id)
        {

            if (!this.postService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }

            var postInfo = this.postService.PostById(id);

            ViewData["PostPhoto"] = postInfo.Photo;

            var postFormModel = new PostFormModel
            {
                Text = postInfo.Text,
                Feeling = postInfo.Feeling
            };

            postFormModel.Albums = this.albumService.AlbumsByUserForDD(User.Identity.GetUserId());

            return View(postFormModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, [Bind(Exclude = "Photo")]  PostFormModel model)
        {
            if (!this.postService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }
            byte[] imageData = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["Photo"];
                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            this.postService.Edit(id, model.Feeling, model.Text, imageData, model.AlbumId);

            return RedirectToAction("AccountDetails", "Users", new { id = this.User.Identity.GetUserId() });
        }

        public ActionResult Delete(int id)
        {
            if (!this.postService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }

            var postInfo = this.postService.PostById(id);

            ViewData["PostPhoto"] = postInfo.Photo;

            var postFormModel = new PostFormModel
            {
                Text = postInfo.Text,
                Feeling = postInfo.Feeling,
                Photo = postInfo.Photo
            };

            postFormModel.Albums = this.albumService.AlbumsByUserForDD(User.Identity.GetUserId());

            return View(postFormModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Destroy(int id)
        {
            if (!this.postService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }

            this.postService.Delete(id);

            return RedirectToAction("AccountDetails", "Users", new { id = this.User.Identity.GetUserId() });
        }

        public ActionResult Like(int id) //id = postId
        {
            if (!this.postService.Exists(id))
            {
                throw new HttpException(404, "Not found");
            }

            this.postService.Like(id);

            return RedirectToAction("Index", "Users");
        }
    }
}