using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;

namespace NotepadShop.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork repository { get; set; }
        private IOrderNumberGenerator orderNumberGenerator { get; set; }
        private static readonly string IncludeNamesPath =
            $"{nameof(DAL.Entities.Order.Items)}.{nameof(DAL.Entities.OrderItem.Item)}.{nameof(DAL.Entities.Item.Names)}";

        public OrderService(IUnitOfWork repository, IOrderNumberGenerator orderNumberGenerator)
        {
            this.repository = repository;
            this.orderNumberGenerator = orderNumberGenerator;
        }

        public void CreateOrder(ICreateOrderData createOrderData)
        {
            DAL.Entities.Order assembledOrder = AssembleOrder(createOrderData, orderNumberGenerator.GenerateNumber());
            repository.OrderRepository.Create(assembledOrder);
            repository.Save();
        }

        public IEnumerable<IOrder> getOrdersByDateRange(DateTime dateTo, DateTime dateFrom)
        {
            IEnumerable<DAL.Entities.Order> foundOrders = repository.OrderRepository.
                GetAll().Where(order => order.CreatingDateTime >= dateTo && order.CreatingDateTime <= dateFrom).
                Include(order => order.User).
                Include(order => order.Items.Select(orderItem => orderItem.Item)).
                Include(IncludeNamesPath).
                OrderByDescending(order => order.CreatingDateTime).ToList();

            return Assembler.Assemble(foundOrders);
        }


        public IOrder getOrderByNumber(string number)
        {
            DAL.Entities.Order found = repository.OrderRepository.
                GetAll().Where(order => order.Number == number).
                Include(order => order.User).
                Include(order => order.Items.Select(orderItem => orderItem.Item)).
                Include(IncludeNamesPath).
                FirstOrDefault();

            return found == null ? null : Assembler.Assemble(found);
        }

        public IEnumerable<IOrder> getOrdersByUserEmail(string email)
        {
            IEnumerable<DAL.Entities.Order> foundOrders = repository.OrderRepository.
                GetAll().Where(order => order.CustomerEmail == email || (order.User == null ? false : order.User.Email == email)).
                Include(order => order.User).
                Include(order => order.Items.Select(orderItem => orderItem.Item)).
                Include(IncludeNamesPath);

            return Assembler.Assemble(foundOrders.ToList());
        }

        public IEnumerable<IOrder> getOrdersByPhoneNumber(string phoneNumber)
        {
            IEnumerable<DAL.Entities.Order> foundOrders = repository.OrderRepository.
                GetAll().Where(order => order.CustomerPhone == phoneNumber).
                Include(order => order.User).
                Include(order => order.Items.Select(orderItem => orderItem.Item)).
                Include(IncludeNamesPath);

            return Assembler.Assemble(foundOrders.ToList());
        }


        private DAL.Entities.Order AssembleOrder(ICreateOrderData createOrderData, string newOrderNumber)
        {
            IEnumerable<string> itemCodes = createOrderData.Items.Select(item => item.Code).ToList();
            IEnumerable<DAL.Entities.Item> items = repository.ItemRepository.
                GetAll().Where(item => itemCodes.Contains(item.Code)).ToList();
            ICollection<DAL.Entities.OrderItem> assembledItems = createOrderData.Items.Select(item => AssembleItem(item, items)).ToList();
            return new DAL.Entities.Order
            {
                Number = newOrderNumber,
                CustomerName = createOrderData.CustomerName,
                CustomerSurname = createOrderData.CustomerSurname,
                CustomerPhone = createOrderData.CustomerPhone,
                CustomerEmail = createOrderData.CustomerEmail,
                City = createOrderData.City,
                PostDepartment = createOrderData.PostDepartment,
                PaymentType = Assembler.Assemble(createOrderData.PaymentType),
                DeliveryType = Assembler.Assemble(createOrderData.DeliveryType),
                User = createOrderData.UserEmail == null ? null : repository.UserManager.FindByEmail(createOrderData.UserEmail),
                OrderStatus = DAL.Entities.OrderStatus.Ordered,
                CreatingDateTime = DateTime.UtcNow,
                Items = assembledItems
            };
        }

        private DAL.Entities.OrderItem AssembleItem(ICreateOrderItemData item, IEnumerable<DAL.Entities.Item> items)
        {
            return new DAL.Entities.OrderItem
            {
                Item = items.First(foundItem => foundItem.Code == item.Code),
                Count = item.Count
            };
        }
    }
}
