using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Util;
using NotepadShop.Models.ItemModels;
using NotepadShop.Models.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    public class OrderController : Controller
    {
        private IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Route("api/create-order")]
        [HttpPost]
        public JsonResult CreateOrder(CreateOrderData createOrderData)
        {
            string userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            orderService.CreateOrder(WebAssembler.Assemble(createOrderData, userName));
            return Json("Ok");
        }

        [Route("api/orders-by-date-range")]
        [HttpGet]
        public JsonResult GetOrdersByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            DateTime utcDateFrom = dateFrom.ToUniversalTime();
            DateTime utcDateTo = dateTo.ToUniversalTime();
            IEnumerable<IOrder> foundOrders = orderService.GetOrdersByDateRange(utcDateFrom, utcDateTo);
            IEnumerable<Order> assembledOrders = WebAssembler.Assemble(foundOrders, ViewBag.Language);
           
            return Json(assembledOrders, JsonRequestBehavior.AllowGet);
        }

        [Route("api/order-by-number")]
        [HttpGet]
        public JsonResult GetOrderByOrderNumber(string number)
        {
            IOrder foundOrder = orderService.GetOrderByNumber(number);
            return Json(foundOrder == null ? "not exists" : 
                WebAssembler.Assemble(foundOrder, ViewBag.Language), JsonRequestBehavior.AllowGet);
        }

        private string calculateMainImageName(Item data)
        {
            string result = null;
            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(GlobalConstants.ImagesDirectoryPath));
            FileInfo mainImagefile = imagesDirectory.GetFiles().FirstOrDefault(image => image.Name.StartsWith(data.Code + "_Main"));

            if (mainImagefile != null)
            {
                result = data.Code + "_Main" + calcualteFileExtension(mainImagefile.Name);
            }

            return result;
        }

        private string calcualteFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.'));
        }

    }
}