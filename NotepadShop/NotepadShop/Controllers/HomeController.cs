using NotepadShop.BLL.Util;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    public class HomeController : Controller
    {
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