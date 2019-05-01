using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class Item
    {
        public decimal Price { get; private set; }
        public ItemCategory Category { get; private set; }
        public ICollection<ItemName> Names { get; private set; }
    }
}