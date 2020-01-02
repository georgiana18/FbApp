using FbApp.Dtos;
using FbApp.Services;
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

        public PostsController()
        {
            this.postService = new PostService();
        }

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        public ActionResult Create()
        {
            return View();
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

            this.postService.Create(this.User.Identity.GetUserId(), model.Feeling, model.Text, imageData);

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

            this.postService.Edit(id, model.Feeling, model.Text, imageData);

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
                Feeling = postInfo.Feeling
            };

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