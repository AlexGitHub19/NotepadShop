using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.BLL.Util;
using NotepadShop.Models.Admin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static IEnumerable<string> categories = new List<string> { GlobalConstants.Notepad, GlobalConstants.Pen };

        private IItemService itemService;

        public AdminController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddNewItem()
        {      
            AddItemViewModel viewModel = new AddItemViewModel(Guid.NewGuid().ToString(), categories);
            return View(viewModel);
        }


        [HttpGet]
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

            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(GlobalConstants.ImagesDirectoryPath));
            IEnumerable<string> itemAdditionalImageNames = imagesDirectory.GetFiles().
                Where(image => image.Name.StartsWith(code + "_") && !image.Name.StartsWith(code + "_Main")).
                Select(image => image.Name);
            string mainImageName = imagesDirectory.GetFiles().
                First(image => image.Name.StartsWith(code + "_Main")).Name;


            ChangeItemViewModel viewModel = new ChangeItemViewModel(Guid.NewGuid().ToString(), item.Code, item.Price.ToString(), 
                WebAssembler.Assemble(item.Category), ruName, ukrName, engName, mainImageName, categories, itemAdditionalImageNames);
            return View(viewModel);
        }

        public ActionResult Notepads()
        {
            ViewBag.ItemsCategory = GlobalConstants.Notepad;
            return View("Items");
        }

        public ActionResult Pens()
        {
            ViewBag.ItemsCategory = GlobalConstants.Pen;
            return View("Items");
        }
    }
}