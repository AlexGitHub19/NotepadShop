using NotepadShop.BLL.Entities;
using NotepadShop.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotepadShop.BLL
{
    public static class Assembler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static DAL.Entities.ItemCategory Assemble(ItemCategory itemCategory)
        {
            DAL.Entities.ItemCategory result;
            switch (itemCategory)
            {
                case ItemCategory.Notepad:
                    result = DAL.Entities.ItemCategory.Notepad;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.ItemCategory");
                    result = DAL.Entities.ItemCategory.Notepad;
                    break;
            }

            return result;
        }

        private static ItemCategory Assemble(DAL.Entities.ItemCategory itemCategory)
        {
            ItemCategory result;
            switch (itemCategory)
            {
                case DAL.Entities.ItemCategory.Notepad:
                    result = ItemCategory.Notepad;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.ItemCategory");
                    result = ItemCategory.Notepad;
                    break;
            }

            return result;
        }

        public static IEnumerable<IItem> Assemble(IEnumerable<DAL.Entities.Item> items)
        {
            return items.Select(item => Assemble(item));
        }

        public static IItem Assemble(DAL.Entities.Item item)
        {
            return new Item(item.Code, item.Price, Assemble(item.Category), item.AddingTime, Assemble(item.Names));
        }

        private static ICollection<IItemName> Assemble(ICollection<DAL.Entities.ItemName> itemNames)
        {
            return itemNames.Select(item => Assemble(item)).ToList();
        }

        public static ICollection<DAL.Entities.ItemName> Assemble(ICollection<IItemName> itemNames)
        {
            return itemNames.Select(item => Assemble(item)).ToList();
        }

        private static IItemName Assemble(DAL.Entities.ItemName itemName)
        {
            return new ItemName(itemName.Name, Assemble(itemName.LanguageType));
        }

        private static DAL.Entities.ItemName Assemble(IItemName itemName)
        {
            return new DAL.Entities.ItemName
            {
                Name = itemName.Name,
                LanguageType = Assemble(itemName.LanguageType)
            };
  
        }

        private static LanguageType Assemble(DAL.Entities.LanguageType language)
        {
            LanguageType result;
            switch (language)
            {
                case DAL.Entities.LanguageType.English:
                    result = LanguageType.English;
                    break;
                case DAL.Entities.LanguageType.Russian:
                    result = LanguageType.Russian;
                    break;
                case DAL.Entities.LanguageType.Ukrainian:
                    result = LanguageType.Ukrainian;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.LanguageType");
                    result = LanguageType.Russian;
                    break;
            }

            return result;
        }

        private static DAL.Entities.LanguageType Assemble(LanguageType language)
        {
            DAL.Entities.LanguageType result;
            switch (language)
            {
                case LanguageType.English:
                    result = DAL.Entities.LanguageType.English;
                    break;
                case LanguageType.Russian:
                    result = DAL.Entities.LanguageType.Russian;
                    break;
                case LanguageType.Ukrainian:
                    result = DAL.Entities.LanguageType.Ukrainian;
                    break;
                default:
                    ThrowAssemblingException("Models.ItemModels.LanguageType");
                    result = DAL.Entities.LanguageType.Russian;
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
