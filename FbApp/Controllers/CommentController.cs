using AutoMapper;
using FbApp.Dtos;
using FbApp.Services;
using FbApp.Utilities;
using Microsoft.AspNet.Identity;
using System;
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
    }
}