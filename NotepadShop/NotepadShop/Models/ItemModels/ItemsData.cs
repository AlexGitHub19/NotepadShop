using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class ItemsData
    {
        public int TotalCount { get; set; }
        public IEnumerable<ItemBriefData> Items { get; set; } = new List<ItemBriefData>();

        public ItemsData(int totalCount, IEnumerable<ItemBriefData> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}