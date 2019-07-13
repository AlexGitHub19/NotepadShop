using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class ItemsData
    {
        public int TotalCount { get; set; }
        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public ItemsData(int totalCount, IEnumerable<Item> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}