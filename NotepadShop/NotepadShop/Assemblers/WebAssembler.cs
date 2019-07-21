using NotepadShop.BLL.Util;
using NotepadShop.Models.ItemModels;
using NotepadShop.Models.Order;
using NotepadShop.Models.Profile;
using NotepadShop.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NotepadShop.Assemblers
{
    public static class WebAssembler
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private const string PaymentTypeCart = "cart";
        private const string PaymentTypeCash = "cash";
        private const string DeliveryTypeNovaPosta = "novaPoshta";
        private const string DeliveryTypeSelf = "self";
        private const string OrderStatusOrdered = "ordered";
        private const string OrderStatusPaid = "paid";
        private const string OrderStatusSent = "sent";
        private const string OrderStatusСanceled = "canceled";
        private const string OrderStatusСompleted = "completed";
        public const string LanguageTypeRu = "ru";
        public const string LanguageTypeUk = "uk";
        public const string LanguageTypeEn = "en";

        public static BLL.Interfaces.IBriefItem Assemble(CreateItemData item)
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
                case LanguageTypeEn:
                    result = BLL.Interfaces.LanguageType.English;
                    break;
                case LanguageTypeRu:
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
                case LanguageTypeUk:
                    result = BLL.Interfaces.LanguageType.Ukrainian;
                    break;
                default:
                    ThrowAssemblingException("Language", language);
                    result = BLL.Interfaces.LanguageType.Russian;
                    break;
            }

            return result;
        }

        public static string AssembleLanguage(BLL.Interfaces.LanguageType language)
        {
            string result;
            switch (language)
            {
                case BLL.Interfaces.LanguageType.English:
                    result = LanguageTypeEn;
                    break;
                case BLL.Interfaces.LanguageType.Russian:
                    result = LanguageTypeRu;
                    break;
                case BLL.Interfaces.LanguageType.Ukrainian:
                    result = LanguageTypeUk;
                    break;
                default:
                    ThrowAssemblingException("Language", language.ToString());
                    result = LanguageTypeRu;
                    break;
            }

            return result;
        }

        public static IEnumerable<Item> Assemble(IEnumerable<BLL.Interfaces.IItem> items, string language)
        {
            return items.Select(item => Assemble(item, language)).ToList();
        }

        public static Item Assemble(BLL.Interfaces.IItem item, string language)
        {
            return new Item
            {
                Code = item.Code,
                Price = item.Price,
                Name = AssembleName(item.Names, language),
                MainImageName = ItemUtils.getMainImageName(item.Code)
            };
        }

        public static FullItem AssembleFullItem(BLL.Interfaces.IItem item, string language)
        {
            return new FullItem
            {
                Code = item.Code,
                Price = item.Price,
                Name = AssembleName(item.Names, language),
                MainImageName = ItemUtils.getMainImageName(item.Code),
                AdditionalImages = ItemUtils.GetItemAdditionalImageNames(item.Code)
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

        public static BLL.Interfaces.ICreateOrderData Assemble(CreateOrderData createOrderData, string userEmail)
        {
            return new BLL.Entities.CreateOrderData(createOrderData.CustomerName,
                createOrderData.CustomerSurname, createOrderData.CustomerPhone, createOrderData.CustomerEmail, createOrderData.City,
                createOrderData.PostDepartment, AssemblePaymentType(createOrderData.PaymentType),
                AssembleDeliveryType(createOrderData.DeliveryType), userEmail, Assemble(createOrderData.Items));
        }

        private static BLL.Interfaces.DeliveryType AssembleDeliveryType(string deliveryType)
        {
            BLL.Interfaces.DeliveryType result;
            switch (deliveryType)
            {
                case DeliveryTypeNovaPosta:
                    result = BLL.Interfaces.DeliveryType.NovaPosta;
                    break;
                case DeliveryTypeSelf:
                    result = BLL.Interfaces.DeliveryType.Self;
                    break;
                default:
                    ThrowAssemblingException("deliveryType", deliveryType); ;
                    result = BLL.Interfaces.DeliveryType.Self;
                    break;
            }

            return result;
        }

        private static string AssembleDeliveryType(BLL.Interfaces.DeliveryType deliveryType)
        {
            string result;
            switch (deliveryType)
            {
                case BLL.Interfaces.DeliveryType.NovaPosta:
                    result = DeliveryTypeNovaPosta;
                    break;
                case BLL.Interfaces.DeliveryType.Self:
                    result = DeliveryTypeSelf;
                    break;
                default:
                    ThrowAssemblingException("deliveryType", deliveryType.ToString()); ;
                    result = DeliveryTypeSelf;
                    break;
            }

            return result;
        }

        private static BLL.Interfaces.PaymentType AssemblePaymentType(string paymentType)
        {
            BLL.Interfaces.PaymentType result;
            switch (paymentType)
            {
                case PaymentTypeCart:
                    result = BLL.Interfaces.PaymentType.Cart;
                    break;
                case PaymentTypeCash:
                    result = BLL.Interfaces.PaymentType.Cash;
                    break;
                default:
                    ThrowAssemblingException("paymentType", paymentType); ;
                    result = BLL.Interfaces.PaymentType.Cash;
                    break;
            }

            return result;
        }

        private static string AssemblePaymentType(BLL.Interfaces.PaymentType paymentType)
        {
            string result;
            switch (paymentType)
            {
                case BLL.Interfaces.PaymentType.Cart:
                    result = PaymentTypeCart;
                    break;
                case BLL.Interfaces.PaymentType.Cash:
                    result = PaymentTypeCash;
                    break;
                default:
                    ThrowAssemblingException("paymentType", paymentType.ToString()); ;
                    result = PaymentTypeCash;
                    break;
            }

            return result;
        }

        private static string AssembleOrderStatus(BLL.Interfaces.OrderStatus orderStatus)
        {
            string result;
            switch (orderStatus)
            {
                case BLL.Interfaces.OrderStatus.Ordered:
                    result = OrderStatusOrdered;
                    break;
                case BLL.Interfaces.OrderStatus.Paid:
                    result = OrderStatusPaid;
                    break;
                case BLL.Interfaces.OrderStatus.Sent:
                    result = OrderStatusSent;
                    break;
                case BLL.Interfaces.OrderStatus.Сanceled:
                    result = OrderStatusСanceled;
                    break;
                case BLL.Interfaces.OrderStatus.Сompleted:
                    result = OrderStatusСompleted;
                    break;
                default:
                    ThrowAssemblingException("orderStatus", orderStatus.ToString()); ;
                    result = OrderStatusOrdered;
                    break;
            }

            return result;
        }

        private static ICollection<BLL.Interfaces.ICreateOrderItemData> Assemble(ICollection<CreateOrderItemData> itemsData)
        {
            return itemsData.Select(item => Assemble(item)).ToList();
        }

        private static BLL.Interfaces.ICreateOrderItemData Assemble(CreateOrderItemData itemData)
        {
            return new BLL.Entities.CreateOrderItemData(itemData.Code, itemData.Count);
        }

        public static IEnumerable<Order> Assemble(IEnumerable<BLL.Interfaces.IOrder> orders, string language)
        {
            return orders.Select(order => Assemble(order, language));
        }

        public static Order Assemble(BLL.Interfaces.IOrder order, string language)
        {
            return new Order
            {
                Number = order.Number,
                CustomerName = order.CustomerName,
                CustomerSurname = order.CustomerSurname,
                CustomerPhone = order.CustomerPhone,
                CustomerEmail = order.CustomerEmail,
                City = order.City,
                PostDepartment = order.PostDepartment,
                PaymentType = AssemblePaymentType(order.PaymentType),
                DeliveryType = AssembleDeliveryType(order.DeliveryType),
                UserEmail = order.UserEmail,
                OrderStatus = AssembleOrderStatus(order.OrderStatus),
                CreatingDateTime = order.CreatingDateTime.ToString("u"),
                Items = Assemble(order.Items, language)
            };
        }

        public static IEnumerable<OrderItem> Assemble(IEnumerable<BLL.Interfaces.IOrderItem> orderItems, string language)
        {
            return orderItems.Select(item => Assemble(item, language));
        }

        public static OrderItem Assemble(BLL.Interfaces.IOrderItem orderItem, string language)
        {
            return new OrderItem
            {
                Item = Assemble(orderItem.Item, language),
                Count = orderItem.Count,
                Price = orderItem.Price
            };
        }

        public static PersonalInfo Assemble(BLL.Interfaces.IPersonalInfo info)
        {
            string language = info.Language != null ? AssembleLanguage(info.Language.Value) : null;
            return new PersonalInfo
            {
                FirstName = info.FirstName,
                Surname = info.Surname,
                City = info.City,
                PostDepartment = info.PostDepartment,
                Phone = info.Phone,
                Language = language
            };
        }

        public static BLL.Interfaces.IChangePersonalInfoData Assemble(ChangePersonalInfoData data, string email)
        {
            BLL.Interfaces.LanguageType? newLanguage = data.NewLanguage.NewValue != null ? AssembleLanguage(data.NewLanguage.NewValue) :
                default(BLL.Interfaces.LanguageType?);
            return new BLL.Entities.ChangePersonalInfoData(email,
                new BLL.Entities.ChangePersonalInfoItemData<string>(data.NewFirstName.NewValue, data.NewFirstName.IsChanged),
                new BLL.Entities.ChangePersonalInfoItemData<string>(data.NewSurname.NewValue, data.NewSurname.IsChanged),
                new BLL.Entities.ChangePersonalInfoItemData<string>(data.NewCity.NewValue, data.NewCity.IsChanged),
                new BLL.Entities.ChangePersonalInfoItemData<string>(data.NewPostDepartment.NewValue, data.NewPostDepartment.IsChanged),
                new BLL.Entities.ChangePersonalInfoItemData<string>(data.NewPhone.NewValue, data.NewPhone.IsChanged),
                new BLL.Entities.ChangePersonalInfoItemData<BLL.Interfaces.LanguageType?>(newLanguage, data.NewLanguage.IsChanged));
        }

        public static string CalcualteFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.'));
        }
    }
}