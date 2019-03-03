using NotepadShop.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace NotepadShop.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        int Save();
    }
}
