using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class ChangeItemData : IChangeItemData
    {
        public string Code { get; }
        public decimal? NewPrice { get; }
        public ItemCategory? NewCategory { get; }
        public string NewRuName { get; }
        public string NewUkName { get; }
        public string NewEnName { get; }

        public ChangeItemData(string code, decimal? newPrice, ItemCategory? newCategory, string newRuName, string newUkName, string newEnName)
        {
            Code = code;
            NewPrice = newPrice;
            NewCategory = newCategory;
            NewRuName = newRuName;
            NewUkName = newUkName;
            NewEnName = newEnName;
        }
    }
}
