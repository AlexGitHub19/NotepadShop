using NotepadShop.Models.ItemModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotepadShop.Assemblers
{
    public static class WebAssembler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static BLL.Interfaces.IBriefItem Assemble(Item item)
        {
            return new BLL.Entities.BriefItem(item.Price, Assemble(item.Category), Assemble(item.Names));
        }

        private static BLL.Interfaces.ItemCategory Assemble(string category)
        {
            BLL.Interfaces.ItemCategory result;
            switch (category)
            {
                case "Notepad":
                    result = BLL.Interfaces.ItemCategory.Notepad;
                    break;
                default:
                    ThrowAssemblingException("Item.Category", category);
                    result = BLL.Interfaces.ItemCategory.Notepad;
                    break;
            }

            return result;
        }

        private static ICollection<BLL.Interfaces.IItemName> Assemble(ICollection<ItemName> itemNames)
        {
            return itemNames.Select(item => Assemble(item)).ToList();
        }

        private static BLL.Interfaces.IItemName Assemble(ItemName itemName)
        {
            return new BLL.Entities.ItemName(itemName.Name, AssembleLanguage(itemName.Language));
        }

        private static BLL.Interfaces.LanguageType AssembleLanguage(string language)
        {
            BLL.Interfaces.LanguageType result;
            switch (language)
            {
                case "en":
                    result = BLL.Interfaces.LanguageType.English;
                    break;
                case "ru":
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
                case "uk":
                    result = BLL.Interfaces.LanguageType.Ukrainian;
                    break;
                default:
                    ThrowAssemblingException("Item.ItemName.Language", language);
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
            }

            return result;
        }

        private static void ThrowAssemblingException(string fieldName, string value)
        {
            string exceptionMessage = $"Not supported value {value} of field {fieldName}";
            logger.Error(exceptionMessage);
            throw new NotSupportedException(exceptionMessage);
        }
    }
}