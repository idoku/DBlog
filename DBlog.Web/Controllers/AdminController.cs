using DBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBlog.Web.Controllers
{
    //[Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
         DBlog.Core.Database.BlogContext context = new Core.Database.BlogContext();
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Edit(int? Id)
        {
            

            var post = Id.HasValue ? context.Posts.Find(Id) : new Post()
            {
                CreateDate = DateTime.Now,                            
            };
            if (post.Id > 0)
            {
                context.Entry(post).Collection(c => c.Categories).Load();
                context.Entry(post).Collection(c => c.Tags).Load();
                context.Entry(post).Collection(c => c.Comments).Load();
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "Title,Slug,Description,Author")] Post post)
        {

            post.CreateDate = DateTime.Now;
            post.Author = "doku";

            return View(post);
        }

    }
}