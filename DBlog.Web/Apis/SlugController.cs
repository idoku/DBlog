using DBlog.Core.Translate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBlog.Web.Apis
{
    public class SlugController : ApiController
    {
        DBlog.Core.Database.BlogContext context = new Core.Database.BlogContext();

        [HttpGet]
        public string Trans(string name)
        {
            string slug = TransHelper.Translate(name, Language.zh, Language.en);
            return slug;
        }

        [HttpGet]
        public bool Exist(string slug)
        {
            return context.Posts.Any(p => p.Slug == slug);
        }
    }
}
