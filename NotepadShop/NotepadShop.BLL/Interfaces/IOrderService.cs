using System;
using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(ICreateOrderData createOrderData);
        IEnumerable<IOrder> getOrdersByDateRange(DateTime dateTo, DateTime dateFrom);
        IOrder getOrderByNumber(string number);
        IEnumerable<IOrder> getOrdersByUserEmail(string email);
        IEnumerable<IOrder> getOrdersByPhoneNumber(string phoneNumber);
    }
}
