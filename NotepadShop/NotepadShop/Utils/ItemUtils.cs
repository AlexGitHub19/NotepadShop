using NotepadShop.BLL.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NotepadShop.Utils
{
    public static class ItemUtils
    {
        public static IEnumerable<string> GetItemAdditionalImageNames(string itemCode)
        {
            DirectoryInfo imagesDirectory = 
                new DirectoryInfo(HttpContext.Current.Server.MapPath(GlobalConstants.ImagesDirectoryPath));
            return imagesDirectory.GetFiles().
                Where(image => image.Name.StartsWith(itemCode + "_") && !image.Name.StartsWith(itemCode + "_Main")).
                Select(image => image.Name);
        }

        public static string getMainImageName(string itemCode)
        {
            DirectoryInfo imagesDirectory =
                new DirectoryInfo(HttpContext.Current.Server.MapPath(GlobalConstants.ImagesDirectoryPath));
            return imagesDirectory.GetFiles().
                First(image => image.Name.StartsWith(itemCode + "_Main")).Name;
        }
    }
}