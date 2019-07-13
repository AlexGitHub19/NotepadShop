using NotepadShop.DAL.Entities;
using NotepadShop.DAL.Identity;
using System;

namespace NotepadShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IRepository<Item> ItemRepository { get; }
        IRepository<ItemCode> ItemCodeRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderNumber> OrderNumberRepository { get; }
        int Save();
    }
}
