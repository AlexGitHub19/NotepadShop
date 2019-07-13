using System;
using System.Collections.Generic;

namespace NotepadShop.Models.Order
{
    public class Order
    {
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string City { get; set; }
        public string PostDepartment { get; set; }
        public string PaymentType { get; set; }
        public string DeliveryType { get; set; }
        public string UserEmail { get; set; }
        public string OrderStatus { get; set; }
        public string CreatingDateTime { get; set; }
        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}