using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.EF;
using NotepadShop.DAL.Identity;
using NotepadShop.DAL.Identity.Entities;
using NotepadShop.DAL.Interfaces;
using System;

namespace NotepadShop.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("SqlLogger");

        private ApplicationContext context;

        public UnitOfWork()
        {
            context = new ApplicationContext("DbConnection");
            // context.Database.Log = LogSql;
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
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public static void LogSql(string message)
        {
            logger.Info(message);
        }
    }
}
