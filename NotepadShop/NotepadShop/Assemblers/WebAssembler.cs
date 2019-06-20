using NotepadShop.BLL.Util;
using NotepadShop.Models.ItemModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NotepadShop.Assemblers
{
    public static class WebAssembler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static BLL.Interfaces.IBriefItem Assemble(Item item)
        {
            return new BLL.Entities.BriefItem(AssemblePrice(item.Price), Assemble(item.Category), Assemble(item.Names));
        }

        public static BLL.Interfaces.ItemCategory Assemble(string category)
        {
            BLL.Interfaces.ItemCategory result;
            switch (category)
            {
                case GlobalConstants.Notepad:
                    result = BLL.Interfaces.ItemCategory.Notepad;
                    break;
                case GlobalConstants.Pen:
                    result = BLL.Interfaces.ItemCategory.Pen;
                    break;
                default:
                    ThrowAssemblingException("Item.Category", category);
                    result = BLL.Interfaces.ItemCategory.Notepad;
                    break;
            }

            return result;
        }

        public static string Assemble(BLL.Interfaces.ItemCategory category)
        {
            string result;
            switch (category)
            {
                case BLL.Interfaces.ItemCategory.Notepad:
                    result = GlobalConstants.Notepad;
                    break;
                case BLL.Interfaces.ItemCategory.Pen:
                    result = GlobalConstants.Pen;
                    break;
                default:
                    ThrowAssemblingException("Item.Category", category.ToString());
                    result = "";
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

        public static IEnumerable<ItemBriefData> Assemble(IEnumerable<BLL.Interfaces.IItem> items, string language)
        {
            return items.Select(item => Assemble(item, language)).ToList();
        }

        public static ItemBriefData Assemble(BLL.Interfaces.IItem item, string language)
        {
            return new ItemBriefData
            {
                Code = item.Code,
                Price = item.Price,
                Name = AssembleName(item.Names, language)
            };
        }

        private static string AssembleName(IEnumerable<BLL.Interfaces.IItemName> itemNames, string language)
        {
            BLL.Interfaces.LanguageType languageType = AssembleLanguage(language);
            return itemNames.First(itemName => itemName.LanguageType == languageType).Name;
        }

        public static decimal AssemblePrice(string price)
        {
            return Convert.ToDecimal(price, NumberFormatInfo.InvariantInfo);
        }

        private static void ThrowAssemblingException(string fieldName, string value)
        {
            string exceptionMessage = $"Not supported value {value} of field {fieldName}";
            logger.Error(exceptionMessage);
            throw new NotSupportedException(exceptionMessage);
        }
    }
}