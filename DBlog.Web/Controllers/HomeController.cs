using DBlog.Core.Common;
using DBlog.Core.Entities;
using DBlog.Core.Translate;
using DBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace DBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        DBlog.Core.Database.BlogContext context = new Core.Database.BlogContext();

        private const int PageSize = 8;

        public ActionResult Index(string tag, string search, int? page)
        {            
            int pageIndex = page ?? 1;
            var posts = this.GetPosts(tag, "", search, new Paging(pageIndex, PageSize));
            var categories = this.GetCategories();
            var model = new HomeIndexModel();
            model.Posts = posts;
            model.Categoryies = categories;
            model.Search = search;
            model.Tag = tag;
            return View(model);
        }

        public ActionResult Tags(string tag, int? page)
        {
            int pageIndex = page ?? 1;
            var posts = this.GetPosts(tag,"", "", new Paging(pageIndex, PageSize));
            var model = new HomeIndexModel();
            model.Posts = posts;
            model.Categoryies = null;
            model.Search = "";
            model.Tag = tag;         
            return View(model);
        }

        public ActionResult Category(string cat, int? page)
        {
            int pageIndex = page ?? 1;
            var posts = this.GetPosts("", cat, "", new Paging(pageIndex, PageSize));
            var model = new HomeIndexModel();
            model.Posts = posts;
            model.Categoryies = this.GetCategories();
            model.Search = "";
            model.Tag = "";
            model.Category = cat;
            return View(model);
        }

        public ActionResult Post(string slug)
        {
            var post = this.GetBySlug(slug);
            if (post == null)
            {
                return new HttpNotFoundResult();
            }
            return View(post);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Post(string slug, [Bind(Include = "Author,Email,Content")]Comment comment)
        {
            var post = GetBySlug(slug);
            if (post == null)
            {
                return new HttpNotFoundResult();
            }
            if (!ModelState.IsValid)
            {
                if (this.Request.IsAjaxRequest())
                {
                    return PartialView("_CommentsControl", post);
                }
                else
                {
                    return PartialView(post);
                }
            }
            comment.CommentDate = DateTime.Now;
            comment.IsAudit = Request.IsAuthenticated;
            comment.PostId = post.Id;
            context.Comments.Add(comment);
            context.SaveChanges();
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("_CommentsControl", post);
            }
            else
            {
                return View(post);
            }
        }
       
        private IEnumerable<Category> GetCategories()
        {
            return this.context.Categories
                .Where(c => c.ParentCategory == null)
                .Take(9).ToList();
        }

        private PagedList<PostDescriptionModel> GetPosts(string tag,string cat,string search, Paging paging)
        {
            var query = this.context.Posts
                .Where(p => !p.IsDelete && p.CreateDate <= DateTime.Now);

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(p => p.Tags.Count(t => t.TagName.Equals(tag, StringComparison.OrdinalIgnoreCase)) > 0);
            }

            if (!string.IsNullOrEmpty(cat))
            {
                query = query.Where(p => p.Categories.Count(c => c.Name.Equals(cat, StringComparison.OrdinalIgnoreCase)) > 0);
            }

            if(!string.IsNullOrEmpty(search))
            {
                foreach (var item in search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Where(p => p.Title.Contains(item));
                }
            }
            int count = query.Count();
            var list = query.OrderBy(s => s.CreateDate)
                .Skip(paging.PageIndex * paging.PageSize)
                .Take(paging.PageSize)
                .Select(s => new PostDescriptionModel()
            {
                Author = s.Author,
                Date = s.CreateDate,
                Description = s.Description,
                Slug = s.Slug,
                IsTop = s.IsTop,
                Title = s.Title,
            }).ToList();
            return new PagedList<PostDescriptionModel>(list, paging.PageIndex+1, paging.PageSize, count);
        }

        private Post GetBySlug(string slug)
        {
            var post = this.context.Posts
                .FirstOrDefault(p => !p.IsDelete && !string.IsNullOrEmpty(slug) && p.Slug.Equals(slug));
            if (post != null)
            {
                context.Entry(post).Collection(c => c.Categories).Load();
                context.Entry(post).Collection(c => c.Tags).Load();
                context.Entry(post).Collection(c => c.Comments).Load();
            }
            return post;
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
