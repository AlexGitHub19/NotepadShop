using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.ItemModels;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System;

namespace NotepadShop.Controllers
{
    public class ItemsController : Controller
    {
        private IItemService itemService;

        private string tempFolderPath = "~/Content/Item/Temp/";
        private string imagesDirectoryPath = "~/Content/Item/Images/";

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult CreateItem(Item item, string key)
        {
            IBriefItem assembled = WebAssembler.Assemble(item);
            string newIteCode = itemService.createItem(assembled);

            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(imagesDirectoryPath));
            if (!imagesDirectory.Exists)
            {
                imagesDirectory.Create();
            }

            DirectoryInfo tempItemDirectory = new DirectoryInfo(Server.MapPath(tempFolderPath + key));
            FileInfo[] files = tempItemDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
                if (isFileImage(file))
                {
                    string newName;
                    if (file.Name == "Main")
                    {
                        newName = $"{newIteCode}_Main";
                    }
                    else
                    {
                        newName = $"{newIteCode}_{Guid.NewGuid().ToString()}";
                    }   

                    newName += calcualteFileExtension(file.Name);

                    System.IO.File.Move(file.FullName, Server.MapPath(imagesDirectoryPath + newName));
                }
            }

            tempItemDirectory.Delete(true);
            return Json("Ok");
        }

        private bool isFileImage(FileInfo file)
        {
            List<string> imageExtensions = new List<string> { ".png", ".jpg", ".jpeg" };
            string fileExtension = file.Name.Substring(file.Name.LastIndexOf('.'));
            return imageExtensions.Any(extension => extension == fileExtension);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public JsonResult UploadItemImages(string key)
        {
            DirectoryInfo tempDirectory = new DirectoryInfo(Server.MapPath(tempFolderPath + key));
            if (!tempDirectory.Exists)
            {
                tempDirectory.Create();
            }

            for (int i = 0; i < Request.Files.Count; i ++)
            {
                HttpPostedFileBase image = Request.Files[i];
                string newImageName;
                if (i == 0)
                {
                    newImageName = "Main" + calcualteFileExtension(image.FileName);
                }
                else
                {
                    newImageName = $"{i}{calcualteFileExtension(image.FileName)}";
                }

                image.SaveAs(Server.MapPath(tempFolderPath + key + "/" + newImageName));
            }

            return Json("Ok");
        }

        private string calcualteFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.'));
        }

        [HttpGet]
        public void GetItemsByCategory(Item item)
        {
        }

        [HttpGet]
        public void GetItemsByCode(Item item)
        {
        }
    }
}