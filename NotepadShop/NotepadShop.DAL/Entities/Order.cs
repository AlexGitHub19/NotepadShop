using NotepadShop.DAL.Identity.Entities;
using System;
using System.Collections.Generic;

namespace NotepadShop.DAL.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string City { get; set; }
        public string PostDepartment { get; set; }
        public PaymentType PaymentType { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public ApplicationUser User { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatingDateTime { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        public Order()
        {
            Items = new List<OrderItem>();
        }
    }
}
