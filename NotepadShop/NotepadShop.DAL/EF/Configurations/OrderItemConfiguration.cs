using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class OrderItemConfiguration : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemConfiguration()
        {
            Property(item => item.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasRequired(orderItem => orderItem.Item);
            Property(item => item.Count).IsRequired();
            Property(item => item.Price).IsRequired();
        }
    }
}
