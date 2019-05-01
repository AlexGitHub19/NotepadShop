using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class ItemName: IItemName
    {
        public string Name { get; private set; }
        public LanguageType LanguageType { get; private set; }

        public ItemName(string name, LanguageType languageType)
        {
            Name = name;
            LanguageType = languageType;
        }
    }
}
