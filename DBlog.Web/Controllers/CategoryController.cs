using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBlog.Web.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(string c,int? page)
        {
            return View();
        }


    }
}