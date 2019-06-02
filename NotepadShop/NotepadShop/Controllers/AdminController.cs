using NotepadShop.Models.Admin;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private static IEnumerable<string> categories = new List<string> { "Notepad", "Category2"};

        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddNewItem()
        {
            Guid newItemKey = Guid.NewGuid();       
            AddItemViewModel viewModel = new AddItemViewModel(newItemKey.ToString(), categories);
            return View(viewModel);
        }
    }
}