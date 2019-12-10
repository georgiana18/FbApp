using FbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FbApp.Controllers
{
    public class PhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Index(Photo photo, List<IFormFile> image)
        //{
        //    foreach(var item in image)
        //    {
        //        if(item.Length > 0)
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                await item.CopyToAsync(stream);
        //                photo.Image = stream.ToArray();
        //            }
        //        }
        //    }
        //}
    }
}