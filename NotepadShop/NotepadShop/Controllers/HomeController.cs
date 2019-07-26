using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Util;
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

        [Route("")]
        public ActionResult IndexPage()
        {
            return View("Index");
        }

        [Route("shop")]
        public ActionResult ShopPage()
        {
            return View("Shop");
        }

        [Route("notepads")]
        public ActionResult NotepadsPage()
        {
            ViewBag.ItemsCategory = GlobalConstants.Notepad;
            return View("Items");
        }

        [Route("pens")]
        public ActionResult PensPage()
        {
            ViewBag.ItemsCategory = GlobalConstants.Pen;
            return View("Items");
        }

        [Route("item")]
        public ActionResult GetItem(string code)
        {
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