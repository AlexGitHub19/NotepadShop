using NotepadShop.BLL.Interfaces;
using System.Collections.Generic;

namespace NotepadShop.BLL.Entities
{
    public class CreateOrderData: ICreateOrderData
    {
        public string CustomerName { get; }
        public string CustomerSurname { get; }
        public string CustomerPhone { get; }
        public string CustomerEmail { get; }
        public string City { get; set; }
        public string PostDepartment { get; }
        public PaymentType PaymentType { get; }
        public DeliveryType DeliveryType { get; }
        public string UserEmail { get; }
        public ICollection<ICreateOrderItemData> Items { get; }

        public CreateOrderData(string customerName, string customerSurname, string customerPhone, 
            string customerEmail, string city, string postDepartment, PaymentType paymentType,
            DeliveryType deliveryType, string userEmai, ICollection<ICreateOrderItemData> items)
        {
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            CustomerPhone = customerPhone;
            CustomerEmail = customerEmail;
            City = city;
            PostDepartment = postDepartment;
            PaymentType = paymentType;
            DeliveryType = deliveryType;
            UserEmail = userEmai;
            Items = items;
        }
    }
}
