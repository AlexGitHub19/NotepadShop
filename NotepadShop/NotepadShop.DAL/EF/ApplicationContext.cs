using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.EF.Configurations;
using NotepadShop.DAL.Entities;
using NotepadShop.DAL.Identity.Entities;
using System.Data.Entity;

namespace NotepadShop.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString)
        {
            //Database.SetInitializer<ApplicationContext>(new DbInitializer());
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemName> ItemNames { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderNumber> OrderNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ItemConfiguration());
            modelBuilder.Configurations.Add(new ItemNameConfiguration());
            modelBuilder.Configurations.Add(new ItemCodeConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderItemConfiguration());
            modelBuilder.Configurations.Add(new OrderNumberConfiguration());
        }
    }
}
