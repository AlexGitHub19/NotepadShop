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
                case ItemCategory.Pen:
                    result = DAL.Entities.ItemCategory.Pen;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.BLL.Entities.ItemCategory");
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
                case DAL.Entities.ItemCategory.Pen:
                    result = ItemCategory.Pen;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.BLL.Entities.ItemCategory");
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

        public static LanguageType Assemble(DAL.Entities.LanguageType language)
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
                    ThrowAssemblingException("NotepadShop.BLL.Entities.LanguageType");
                    result = LanguageType.Russian;
                    break;
            }

            return result;
        }

        public static DAL.Entities.LanguageType Assemble(LanguageType language)
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
                    ThrowAssemblingException("NotepadShop.BLL.Entities.LanguageType");
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

        public static DAL.Entities.DeliveryType Assemble(DeliveryType deliveryType)
        {
            DAL.Entities.DeliveryType result;
            switch (deliveryType)
            {
                case DeliveryType.NovaPosta:
                    result = DAL.Entities.DeliveryType.NovaPosta;
                    break;
                case DeliveryType.Self:
                    result = DAL.Entities.DeliveryType.Self;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.BLL.Entities.DeliveryType");
                    result = DAL.Entities.DeliveryType.Self;
                    break;
            }

            return result;
        }

        public static DAL.Entities.PaymentType Assemble(PaymentType paymentType)
        {
            DAL.Entities.PaymentType result;
            switch (paymentType)
            {
                case PaymentType.Cart:
                    result = DAL.Entities.PaymentType.Cart;
                    break;
                case PaymentType.Cash:
                    result = DAL.Entities.PaymentType.Cash;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.BLL.Entities.PaymentType");
                    result = DAL.Entities.PaymentType.Cash;
                    break;
            }

            return result;
        }

        private static DeliveryType Assemble(DAL.Entities.DeliveryType deliveryType)
        {
            DeliveryType result;
            switch (deliveryType)
            {
                case DAL.Entities.DeliveryType.NovaPosta:
                    result = DeliveryType.NovaPosta;
                    break;
                case DAL.Entities.DeliveryType.Self:
                    result = DeliveryType.Self;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.DAL.Entities.DeliveryType");
                    result = DeliveryType.Self;
                    break;
            }

            return result;
        }

        private static PaymentType Assemble(DAL.Entities.PaymentType paymentType)
        {
            PaymentType result;
            switch (paymentType)
            {
                case DAL.Entities.PaymentType.Cart:
                    result = PaymentType.Cart;
                    break;
                case DAL.Entities.PaymentType.Cash:
                    result = PaymentType.Cash;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.DAL.Entities.PaymentType");
                    result = PaymentType.Cash;
                    break;
            }

            return result;
        }

        private static OrderStatus Assemble(DAL.Entities.OrderStatus orderStatus)
        {
            OrderStatus result;
            switch (orderStatus)
            {
                case DAL.Entities.OrderStatus.Ordered:
                    result = OrderStatus.Ordered;
                    break;
                case DAL.Entities.OrderStatus.Paid:
                    result = OrderStatus.Paid;
                    break;
                case DAL.Entities.OrderStatus.Sent:
                    result = OrderStatus.Sent;
                    break;
                case DAL.Entities.OrderStatus.Сanceled:
                    result = OrderStatus.Сanceled;
                    break;
                case DAL.Entities.OrderStatus.Сompleted:
                    result = OrderStatus.Сompleted;
                    break;
                default:
                    ThrowAssemblingException("NotepadShop.DAL.Entities.OrderStatus");
                    result = OrderStatus.Сompleted;
                    break;
            }

            return result;
        }

        public static IEnumerable<IOrder> Assemble(IEnumerable<DAL.Entities.Order> orders)
        {
            return orders.Select(order => Assemble(order));
        }

        public static IOrder Assemble(DAL.Entities.Order order)
        {
            return new Order(order.Number, order.CustomerName, order.CustomerSurname, order.CustomerPhone, order.CustomerEmail,
                order.City, order.PostDepartment, Assemble(order.PaymentType), Assemble(order.DeliveryType),
                order.User == null ? null : order.User.Email, Assemble(order.OrderStatus), order.CreatingDateTime,
                Assemble(order.Items.ToList()));
        }

        private static IEnumerable<IOrderItem> Assemble(IEnumerable<DAL.Entities.OrderItem> items)
        {
            return items.Select(order => Assemble(order));
        }

        private static IOrderItem Assemble(DAL.Entities.OrderItem orderItem)
        {
            return new OrderItem(Assemble(orderItem.Item), orderItem.Count, orderItem.Price);
        }
    }
}
