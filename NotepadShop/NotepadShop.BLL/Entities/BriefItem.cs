using NotepadShop.BLL.Interfaces;
using System.Collections.Generic;

namespace NotepadShop.BLL.Entities
{
    public class BriefItem: IBriefItem
    {
        public decimal Price { get; private set; }
        public ItemCategory Category { get; private set; }
        public ICollection<IItemName> Names { get; private set; }

        public BriefItem(decimal price, ItemCategory category, ICollection<IItemName> names)
        {
            Price = price;
            Category = category;
            Names = names;
        }
    }
}
