using System.Collections.Generic;


namespace NotepadShop.BLL.Interfaces
{
    public interface IItemsData
    {
        int TotalCount { get; set; }
        IEnumerable<IItem> Items { get; set; }
    }
}
