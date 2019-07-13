using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface ICreateOrderData
    {
        string CustomerName { get; }
        string CustomerSurname { get; }
        string CustomerPhone { get; }
        string CustomerEmail { get; }
        string City { get; set; }
        string PostDepartment { get; }
        PaymentType PaymentType { get; }
        DeliveryType DeliveryType { get; }
        string UserEmail { get; }
        ICollection<ICreateOrderItemData> Items { get; }
    }
}
