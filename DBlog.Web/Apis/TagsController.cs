using DBlog.Core.Entities;
using DBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBlog.Web.Apis
{
    public class TagsController : ApiController
    {
        DBlog.Core.Database.BlogContext context = new Core.Database.BlogContext();

        [HttpGet]
        public IHttpActionResult Clouds()
        {
            var tags = this.context.Tags.Take(10).ToList();
            Random r = new Random();
            return Json(tags.Select(t => new
            {
                text = t.TagName,
                weight = r.Next(5),
                link = Url.Link("Default", new { controller = "Home", action = "Tags", tag = t.TagName })
            }));
        }


       
        
 
    }
}
