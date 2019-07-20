﻿using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.EF;
using NotepadShop.DAL.Entities;
using NotepadShop.DAL.Identity;
using NotepadShop.DAL.Identity.Entities;
using NotepadShop.DAL.Interfaces;
using System;

namespace NotepadShop.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger("SqlLogger");

        private bool disposed = false;

        private ApplicationContext context;

        private IRepository<Item> itemRepository;
        private IRepository<ItemCode> itemCodeRepository;
        private IRepository<Order> orderRepository;
        private IRepository<OrderNumber> orderNumberRepository;
        private IRepository<ApplicationUser> userRepository;

        public IRepository<Item> ItemRepository
        {
            get
            {
                if (itemRepository == null)
                    itemRepository = new Repository<Item>(context);
                return itemRepository;
            }
        }

        public IRepository<ItemCode> ItemCodeRepository
        {
            get
            {
                if (itemCodeRepository == null)
                    itemCodeRepository = new Repository<ItemCode>(context);
                return itemCodeRepository;
            }
        }

        public IRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                    orderRepository = new Repository<Order>(context);
                return orderRepository;
            }
        }

        public IRepository<OrderNumber> OrderNumberRepository
        {
            get
            {
                if (orderNumberRepository == null)
                    orderNumberRepository = new Repository<OrderNumber>(context);
                return orderNumberRepository;
            }
        }

        public IRepository<ApplicationUser> UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new Repository<ApplicationUser>(context);
                return userRepository;
            }
        }

        public ApplicationUserManager UserManager { get; private set; }

        public ApplicationRoleManager RoleManager { get; private set; }


        public UnitOfWork()
        {
            context = new ApplicationContext("DbConnection");
            // context.Database.Log = LogSql;
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
