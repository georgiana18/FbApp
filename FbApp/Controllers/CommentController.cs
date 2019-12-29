using AutoMapper;
using FbApp.Dtos;
using FbApp.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;

namespace FbApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;

        public CommentController(IPostService postService, ICommentService commentService)
        {
            this.postService = postService;
            this.commentService = commentService;
        }

        public ActionResult Create(int postId)
        {
            var postCommentViewModel = this.postService.PostById(postId);

            PostCommentCreateModel postCommentCreateModel = Mapper.Map<PostCommentCreateModel>(postCommentViewModel);

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
            return RedirectToAction("Index", "Users");
        }
    }
}