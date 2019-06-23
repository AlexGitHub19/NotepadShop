using NotepadShop.BLL.Interfaces;
using System.Collections.Generic;

namespace NotepadShop.BLL.Entities
{
    public class ItemsData : IItemsData
    {
        public int TotalCount { get; set; }
        public IEnumerable<IItem> Items { get; set; } = new List<IItem>();

        public ItemsData(int totalCount, IEnumerable<IItem> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
