using System;
using System.Collections.Generic;

namespace NotepadShop.BLL.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(ICreateOrderData createOrderData);
        IEnumerable<IOrder> GetOrdersByDateRange(DateTime dateTo, DateTime dateFrom);
        IOrder GetOrderByNumber(string number);
        IEnumerable<IOrder> GetOrdersByUser(string email);
        IEnumerable<IOrder> GetOrdersByPhoneNumber(string phoneNumber);
    }
}
