using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private static List<string> categories = new List<string> { "Notepad", "Category2"};

        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddNewItem()
        {
            return View(categories);
        }
    }
}