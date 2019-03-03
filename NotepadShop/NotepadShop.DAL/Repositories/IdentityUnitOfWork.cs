using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.EF;
using NotepadShop.DAL.Identity;
using NotepadShop.DAL.Identity.Entities;
using NotepadShop.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace NotepadShop.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext context;

        public IdentityUnitOfWork()
        {
            context = new ApplicationContext("DbConnection");
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
        }

        public ApplicationUserManager UserManager { get; private set; }

        public ApplicationRoleManager RoleManager { get; private set; }

        public int Save()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
