using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            Property(item => item.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(item => item.CustomerName).IsRequired();
            Property(item => item.CustomerSurname).IsRequired();
            Property(item => item.CustomerPhone).IsRequired();
            Property(item => item.PaymentType).IsRequired();
            Property(item => item.DeliveryType).IsRequired();
            Property(item => item.OrderStatus).IsRequired();
            Property(item => item.CreatingDateTime).IsRequired();
            HasMany(item => item.Items).WithRequired(orderItem => orderItem.Order);
        }
    }
}
