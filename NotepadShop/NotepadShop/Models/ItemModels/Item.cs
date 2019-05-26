﻿using System.Collections.Generic;

namespace NotepadShop.Models.ItemModels
{
    public class Item
    {
        public decimal Price { get; set; }
        public string Category { get; set; }
        public ICollection<ItemName> Names { get; set; }
    }
}