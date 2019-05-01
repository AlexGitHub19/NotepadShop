﻿using System;
using System.Collections.Generic;

namespace NotepadShop.DAL.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public ItemCategory Category { get; set; }
        public DateTime AddingTime { get; set; }

        public virtual ICollection<ItemName> Names { get; set; }

        public Item()
        {
            Names = new List<ItemName>();
        }
    }
}
