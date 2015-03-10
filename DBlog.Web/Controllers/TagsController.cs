using DBlog.Core.Common;
using DBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace DBlog.Web.Controllers
{
    public class TagsController : Controller
    {
        DBlog.Core.Database.BlogContext context = new Core.Database.BlogContext();

        private const int PageSize = 8;

        // GET: Tags
        public ActionResult Index(string tag,int? page)
        {
            int pageIndex = page ?? 1;
            var posts = this.GetPosts(tag,"",new Paging(pageIndex, PageSize));                  
            return View(posts);
        }

        private PagedList<PostDescriptionModel> GetPosts(string tag, string search, Paging paging)
        {
            var query = this.context.Posts
                .Where(p => !p.IsDelete && p.CreateDate <= DateTime.Now);

            
            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(p => p.Tags.Count(t => t.TagName.Equals(tag, StringComparison.OrdinalIgnoreCase)) > 0);
            }

            if (!string.IsNullOrEmpty(search))
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
            return new PagedList<PostDescriptionModel>(list, paging.PageIndex + 1, paging.PageSize, count);
        }
    }
}