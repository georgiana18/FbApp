using AutoMapper;
using FbApp.Dtos;
using FbApp.Services;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IPostService postService = new PostService();
        private readonly ICommentService commentService = new CommentService();

        public CommentController()
        {
        }

        public ActionResult Create(int id)  //id = postId
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostModel, PostCommentCreateModel>()
                 .ForMember(p => p.Photo, c => c.MapFrom(p => p.Photo.ToRenderablePictureString()))
                 .ForMember(p => p.UserProfilePicture, c => c.MapFrom(p => p.UserProfilePicture.ToRenderablePictureString()));
            });
            IMapper iMapper = config.CreateMapper();

            var postCommentViewModel = this.postService.PostById(id);

            PostCommentCreateModel postCommentCreateModel = iMapper.Map<PostModel, PostCommentCreateModel>(postCommentViewModel);

            return View(postCommentCreateModel);
        }

        [HttpPost]
        public ActionResult Create(PostCommentCreateModel model, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(model.CommentText))
            {
                ModelState.AddModelError(string.Empty, "You cannot submit an empty comment!");
                return View(model);
            }

            this.commentService.Create(model.CommentText, User.Identity.GetUserId(), model.Id);

            return RedirectToAction("AccountDetails", "Users", new { id = this.User.Identity.GetUserId()});
        }

        public ActionResult Edit(int id)
        {
            if (!this.commentService.Exists(id))
            {
                throw new HttpException(404, "Not found"); 
            }

            var commentInfo = this.commentService.CommentById(id);

            ViewData["CommentPhoto"] = commentInfo.UserProfilePicture;
            
            return View(commentInfo);
        }

        [HttpPost]
        public ActionResult Edit(int id,  CommentModel model)
        {
            if(!this.commentService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }

            this.commentService.Edit(id, model.Text);

            return RedirectToAction("AccountDetails", "Users", new { id = this.User.Identity.GetUserId() });
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if(!this.commentService.UserIsAuthorizedToEdit(id, this.User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad Request");
            }
            this.commentService.Delete(id);

            return RedirectToAction("AccountDetails", "Users", new { id = this.User.Identity.GetUserId() });
        }
    }
}