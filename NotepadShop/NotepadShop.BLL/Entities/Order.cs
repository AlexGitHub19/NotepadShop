using NotepadShop.BLL.Interfaces;
using System;
using System.Collections.Generic;

namespace NotepadShop.BLL.Entities
{
    public class Order: IOrder
    {
        public string CustomerName { get; }
        public string Number { get; }
        public string CustomerSurname { get; }
        public string CustomerPhone { get; }
        public string CustomerEmail { get; }
        public string City { get; }
        public string PostDepartment { get; }
        public PaymentType PaymentType { get; }
        public DeliveryType DeliveryType { get; }
        public string UserEmail { get; }
        public OrderStatus OrderStatus { get; }
        public DateTime CreatingDateTime { get; }
        public IEnumerable<IOrderItem> Items { get; } = new List<IOrderItem>();

        public Order(string number, string customerName, string customerSurname, string customerPhone, 
            string customerEmail, string city, string postDepartment, PaymentType paymentType, 
            DeliveryType deliveryType, string userEmail, OrderStatus orderStatus, DateTime creatingDateTime, 
            IEnumerable<IOrderItem> items)
        {
            Number = number;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            City = city;
            PostDepartment = postDepartment;
            PaymentType = paymentType;
            DeliveryType = deliveryType;
            UserEmail = userEmail;
            OrderStatus = orderStatus;
            CreatingDateTime = creatingDateTime;
            Items = items;
        }
    }
}
