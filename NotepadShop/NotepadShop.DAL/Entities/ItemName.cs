using System;

namespace NotepadShop.DAL.Entities
{
    public class ItemName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public LanguageType LanguageType { get; set; }
        public Item Item { get; set; }
    }
}
