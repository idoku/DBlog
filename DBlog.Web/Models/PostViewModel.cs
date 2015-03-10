using DBlog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBlog.Web.Models
{
    public class HomeIndexModel
    {
        public IEnumerable<PostDescriptionModel> Posts { get; set; }

        public IEnumerable<Category> Categoryies { get; set; }

        public string Tag { get; set; }

        public string Category { get; set; }
        
        public string Search { get; set; }


    }

    public class PostDescriptionModel
    {
        public string Slug { get; set; }
        public string Key { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsTop { get; set; }
    }
}