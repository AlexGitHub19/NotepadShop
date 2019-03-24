using System;
using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    public class LocalizationController : Controller
    {
        [HttpPost]
        public JsonResult SetLanguage(string language)
        {

            HttpCookie cookie = new HttpCookie("ns-language");
            cookie["Language"] = language;
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);

            return Json("true");
        }
    }
}