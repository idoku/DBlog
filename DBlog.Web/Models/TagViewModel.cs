using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBlog.Web.Models
{
    public class TagCloudViewModel
    {
        public string Text { get; set; }
        public int Weight { get; set; }
        public string Link { get; set; }
    }
}