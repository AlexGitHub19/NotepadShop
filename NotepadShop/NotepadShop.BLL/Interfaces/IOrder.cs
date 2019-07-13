using System;
using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface IOrder
    {
        string Number { get; }
        string CustomerName { get; }
        string CustomerSurname { get; }
        string CustomerPhone { get; }
        string CustomerEmail { get; }
        string City { get; }
        string PostDepartment { get; }
        PaymentType PaymentType { get; }
        DeliveryType DeliveryType { get; }
        string UserEmail { get; }
        OrderStatus OrderStatus { get; }
        DateTime CreatingDateTime { get; }
        IEnumerable<IOrderItem> Items { get; }
    }
}
