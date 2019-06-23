namespace NotepadShop.BLL.Interfaces
{
    public interface IItemService
    {
        string createItem(IBriefItem item);
        IItem getItemByCode(string code);
        IItemsData getItemsByCategory(ItemCategory category, int countOnPage, int page);
        void deleteItemByCode(string code);
        void changeItem(IChangeItemData itemData);
    }
}
