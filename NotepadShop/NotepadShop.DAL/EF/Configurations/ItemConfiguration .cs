using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class ItemConfiguration: EntityTypeConfiguration<Item>
    {
        public ItemConfiguration()
        {
            Property(item => item.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(item => item.Code).IsRequired().HasMaxLength(100)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute { IsUnique = true }));
            Property(item => item.Price).IsRequired();
            Property(item => item.Category).IsRequired();
            Property(item => item.AddingTime).IsRequired();
            HasMany(item => item.Names).WithRequired(name => name.Item);
            HasMany(item => item.OrderItems).WithRequired(orderItem => orderItem.Item);
        }
    }
}
