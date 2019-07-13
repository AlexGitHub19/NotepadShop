using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Util;
using NotepadShop.Models.ItemModels;
using NotepadShop.Utils;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    public class HomeController : Controller
    {
        private IItemService itemService;

        public HomeController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("notepads")]
        public ActionResult Notepads()
        {
            ViewBag.ItemsCategory = GlobalConstants.Notepad;
            return View("Items");
        }

        [Route("pens")]
        public ActionResult Pens()
        {
            ViewBag.ItemsCategory = GlobalConstants.Pen;
            return View("Items");
        }

        [Route("item")]
        public ActionResult GetItem(string code)
        {
            FullItem result;
            IItem item = itemService.getItemByCode(code);
            if (item != null)
            {
                return View("Item", WebAssembler.AssembleFullItem(item, ViewBag.Language));
            }
            else
            {
                return ReturnNotFound();
            }
        }

        private ActionResult ReturnNotFound()
        {
            return View("NotFound");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}