using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.Profile;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [Authorize]
    [RoutePrefix("profile")]
    public class ProfileController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IPersonalInfoService personalInfoService;

        public ProfileController(IOrderService orderService, IPersonalInfoService personalInfoService)
        {
            this.orderService = orderService;
            this.personalInfoService = personalInfoService;
        }

        [Route("personal-info")]
        [HttpGet]
        public ActionResult GetPersonalInfoView()
        {
            return View("PersonalInfo");
        }

        [Route("api/personal-info")]
        [HttpGet]
        public JsonResult GetPersonalInfo()
        {
            IPersonalInfo info = personalInfoService.GetPersonalInfo(HttpContext.User.Identity.Name);
            return Json(WebAssembler.Assemble(info), JsonRequestBehavior.AllowGet);
        }  

        [Route("api/change-personal-info")]
        [HttpPost]
        public JsonResult ChangePersonalInfo(ChangePersonalInfoData data)
        {
            personalInfoService.ChangePersonalInfoData(WebAssembler.Assemble(data, HttpContext.User.Identity.Name));
            return Json("Ok");
        }

        [Route("orders")]
        [HttpGet]
        public ActionResult GetOrdersView()
        {
            return View("Orders");
        }

        [Route("api/user-orders")]
        [HttpGet]
        public JsonResult GetUserOrders()
        {
            IEnumerable<IOrder> foundOrders = orderService.GetOrdersByUser(HttpContext.User.Identity.Name);
            return Json(WebAssembler.Assemble(foundOrders, ViewBag.Language), JsonRequestBehavior.AllowGet);
        }
    }
}