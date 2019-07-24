using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Util;
using NotepadShop.Models.Admin;
using NotepadShop.Utils;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static IEnumerable<string> categories = new List<string> { GlobalConstants.Notepad, GlobalConstants.Pen };

        private IItemService itemService;

        public AdminController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("add-new-item")]
        public ActionResult AddNewItem()
        {      
            AddItemViewModel viewModel = new AddItemViewModel(Guid.NewGuid().ToString(), categories);
            return View(viewModel);
        }


        [HttpGet]
        [Route("change-item")]
        public ActionResult ChangeItem(string code)
        {
            IItem item = itemService.getItemByCode(code);
            string ruName = null;
            string ukrName = null;
            string engName = null;
            foreach (IItemName itemName in item.Names)
            {
                
                switch (itemName.LanguageType)
                {
                    case LanguageType.English:
                        engName = itemName.Name;
                        break;
                    case LanguageType.Russian:
                        ruName = itemName.Name;
                        break;
                    case LanguageType.Ukrainian:
                        ukrName = itemName.Name;
                        break;
                    default:
                        string exceptionMessage = $"Not supported value {itemName.LanguageType.ToString()} of field IItemName.LanguageType";
                        logger.Error(exceptionMessage);
                        throw new NotSupportedException(exceptionMessage);
                }
            }


            IEnumerable<string> itemAdditionalImageNames = ItemUtils.GetItemAdditionalImageNames(code);
            string mainImageName = ItemUtils.getMainImageName(code);


            ChangeItemViewModel viewModel = new ChangeItemViewModel(Guid.NewGuid().ToString(), item.Code, item.Price.ToString(), 
                WebAssembler.Assemble(item.Category), ruName, ukrName, engName, mainImageName, categories, itemAdditionalImageNames);
            return View(viewModel);
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

        [Route("orders")]
        public ActionResult Orders()
        {
            return View();
        }
    }
}