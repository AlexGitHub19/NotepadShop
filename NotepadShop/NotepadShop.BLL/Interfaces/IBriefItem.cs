using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface IBriefItem
    {
        decimal Price { get; }
        ItemCategory Category { get; }
        ICollection<IItemName> Names { get; }
    }
}
