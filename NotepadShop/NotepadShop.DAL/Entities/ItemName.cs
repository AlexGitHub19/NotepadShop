using System;

namespace NotepadShop.DAL.Entities
{
    public class ItemName
    {
        public string Name { get; set; }
        public LanguageType LanguageType { get; set; }

        public Guid? ItemId { get; set; }
        public Item item { get; set; }
    }
}
