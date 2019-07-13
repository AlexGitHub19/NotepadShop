using System.Collections.Generic;

namespace NotepadShop.Models.Order
{
    public class CreateOrderData
    {
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string City { get; set; }
        public string PostDepartment { get; set; }
        public string PaymentType { get; set; }
        public string DeliveryType { get; set; }
        public ICollection<CreateOrderItemData> Items { get; set; }
    }
}