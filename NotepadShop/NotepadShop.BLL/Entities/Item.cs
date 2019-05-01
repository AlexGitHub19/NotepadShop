using NotepadShop.BLL.Interfaces;
using System;
using System.Collections.Generic;

namespace NotepadShop.BLL.Entities
{
    public class Item: BriefItem, IItem
    {
        public string Code { get; private set; }
        public DateTime AddingTime { get; private set; }

        public Item(string code, decimal price, ItemCategory category, DateTime addingTime, ICollection<IItemName> names) : base(price, category, names)
        {
            Code = code;
            AddingTime = addingTime;
        }
    }
}
