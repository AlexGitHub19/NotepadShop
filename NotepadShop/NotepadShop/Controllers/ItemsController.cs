using NotepadShop.Assemblers;
using NotepadShop.BLL.Interfaces;
using NotepadShop.Models.ItemModels;
using System.Web;
using System.Web.Mvc;

namespace NotepadShop.Controllers
{
    public class ItemsController : Controller
    {
        private IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public void CreateItem(Item item)
        {
            IBriefItem assembled = WebAssembler.Assemble(item);
            itemService.createItem(assembled);
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