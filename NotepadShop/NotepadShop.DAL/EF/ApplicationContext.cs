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
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemName> ItemNames { get; set; }
        public DbSet<ItemCode> ItemCodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ItemConfiguration());
            modelBuilder.Configurations.Add(new ItemNameConfiguration());
            modelBuilder.Configurations.Add(new ItemCodeConfiguration());
        }
    }
}
