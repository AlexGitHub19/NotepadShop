using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotepadShop.Models.ItemModels
{
    public class ItemBriefData
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string MainImageName { get; set; }
    }
}