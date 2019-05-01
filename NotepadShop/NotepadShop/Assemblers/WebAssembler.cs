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

        private static BLL.Interfaces.ItemCategory Assemble(ItemCategory itemCategory)
        {
            BLL.Interfaces.ItemCategory result;
            switch (itemCategory)
            {
                case ItemCategory.Notepad:
                    result = BLL.Interfaces.ItemCategory.Notepad;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.ItemCategory");
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
            return new BLL.Entities.ItemName(itemName.Name, Assemble(itemName.LanguageType));
        }

        private static BLL.Interfaces.LanguageType Assemble(LanguageType language)
        {
            BLL.Interfaces.LanguageType result;
            switch (language)
            {
                case LanguageType.English:
                    result = BLL.Interfaces.LanguageType.English;
                    break;
                case LanguageType.Russian:
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
                case LanguageType.Ukrainian:
                    result = BLL.Interfaces.LanguageType.Ukrainian;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.LanguageType");
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
            }

            return result;
        }

        private static void ThrowAssemblingException(string typeName)
        {
            string exceptionMessage = $"Not supported data of type {typeName}";
            logger.Error(exceptionMessage);
            throw new NotSupportedException(exceptionMessage);
        }
    }
}