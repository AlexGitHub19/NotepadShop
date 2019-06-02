using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface IItemService
    {
        string createItem(IBriefItem item);
        IItem getItemByCode(string code);
        IEnumerable<IItem> getItemsByCategory(ItemCategory category);
    }
}
