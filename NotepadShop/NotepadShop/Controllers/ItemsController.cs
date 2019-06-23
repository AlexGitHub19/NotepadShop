using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.ItemModels;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System;
using NotepadShop.BLL.Util;

namespace NotepadShop.Controllers
{
    public class ItemsController : Controller
    {
        private IItemService itemService;

        private string tempFolderPath = "~/Content/Item/Temp/";

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

            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(GlobalConstants.ImagesDirectoryPath));
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
                    if (file.Name.Substring(0, file.Name.LastIndexOf('.')) == "Main")
                    {
                        newName = $"{newIteCode}_Main";
                    }
                    else
                    {
                        newName = $"{newIteCode}_{Guid.NewGuid().ToString()}";
                    }   

                    newName += calcualteFileExtension(file.Name);

                    System.IO.File.Move(file.FullName, Server.MapPath(GlobalConstants.ImagesDirectoryPath + newName));
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult ChangeItem(ChangeItemData itemData, string key)
        {
            IChangeItemData data = new NotepadShop.BLL.Entities.ChangeItemData(
                itemData.Code, 
                string.IsNullOrEmpty(itemData.NewPrice) ? (decimal?)null : WebAssembler.AssemblePrice(itemData.NewPrice),
                string.IsNullOrEmpty(itemData.NewCategory) ? (BLL.Interfaces.ItemCategory?)null : WebAssembler.Assemble(itemData.NewCategory),
                itemData.NewRuName,
                itemData.NewUkName,
                itemData.NewEnName
                );

            itemService.changeItem(data);

            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(GlobalConstants.ImagesDirectoryPath));

            if (itemData.AdditionalImagesToDeleteNames?.Any() == true)
            {
                foreach (FileInfo file in imagesDirectory.GetFiles().Where(file => file.Name.StartsWith($"{itemData.Code}_")))
                {
                    if (itemData.AdditionalImagesToDeleteNames.Contains(file.Name))
                    {
                        file.Delete();
                    }
                }
            }

            if (itemData.IsMainImageChanged || itemData.AreAdditionalImagesAdded)
            {
                DirectoryInfo tempItemDirectory = new DirectoryInfo(Server.MapPath(tempFolderPath + key));

                if (itemData.IsMainImageChanged)
                {
                    FileInfo oldMainImage = imagesDirectory.GetFiles().
                        First(file => file.Name.Substring(0, file.Name.LastIndexOf('.')) == $"{itemData.Code}_Main");
                    oldMainImage.Delete();
                    FileInfo mainImage = tempItemDirectory.GetFiles().First(file => file.Name.Substring(0, file.Name.LastIndexOf('.')) == "Main");
                    string newName = $"{itemData.Code}_Main" + calcualteFileExtension(mainImage.Name);
                    System.IO.File.Move(mainImage.FullName, Server.MapPath(GlobalConstants.ImagesDirectoryPath + newName));

                }

                if (itemData.AreAdditionalImagesAdded)
                {
                    foreach (FileInfo file in tempItemDirectory.GetFiles())
                    {
                        if (isFileImage(file))
                        {
                            string newName = $"{itemData.Code}_{Guid.NewGuid().ToString()}" + calcualteFileExtension(file.Name);
                            System.IO.File.Move(file.FullName, Server.MapPath(GlobalConstants.ImagesDirectoryPath + newName));
                        }
                    }
                }

                tempItemDirectory.Delete(true);
            }
        
            return Json("Ok");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeItemUploadItemImages(string key)
        {
            DirectoryInfo tempDirectory = new DirectoryInfo(Server.MapPath(tempFolderPath + key));

            if (!tempDirectory.Exists)
            {
                tempDirectory.Create();
            }

            for (int i = 0; i < Request.Files.Count; i ++)
            {
                HttpPostedFileBase image = Request.Files[i];
                string newImageName = $"{i}{calcualteFileExtension(image.FileName)}";
                image.SaveAs(Server.MapPath(tempFolderPath + key + "/" + newImageName));
            }

            return Json("Ok");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeItemUploadMainImage(string key)
        {
            DirectoryInfo tempDirectory = new DirectoryInfo(Server.MapPath(tempFolderPath + key));

            if (!tempDirectory.Exists)
            {
                tempDirectory.Create();
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase image = Request.Files[i];
                string newImageName = "Main" + calcualteFileExtension(image.FileName);
                image.SaveAs(Server.MapPath(tempFolderPath + key + "/" + newImageName));
            }

            return Json("Ok");
        }

            
        [HttpGet]
        public JsonResult GetItems(string category, int countOnPage, int page)
        {
            IItemsData itemsData = itemService.getItemsByCategory(WebAssembler.Assemble(category), countOnPage, page);

            ItemsData result = new ItemsData(itemsData.TotalCount, WebAssembler.Assemble(itemsData.Items, ViewBag.Language));
            foreach (ItemBriefData data in result.Items)
            {
                data.MainImageName = calculateMainImageName(data);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteItem(string code)
        {
            itemService.deleteItemByCode(code);

            DirectoryInfo imagesDirectory = new DirectoryInfo(Server.MapPath(GlobalConstants.ImagesDirectoryPath));
            IEnumerable<FileInfo> itemImages = imagesDirectory.GetFiles().Where(image => image.Name.StartsWith(code + "_"));

            foreach (FileInfo image in itemImages)
            {
                System.IO.File.Delete(Server.MapPath(GlobalConstants.ImagesDirectoryPath + image.Name));
            }

            return Json("Ok");
        }

        private string calculateMainImageName(ItemBriefData data)
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