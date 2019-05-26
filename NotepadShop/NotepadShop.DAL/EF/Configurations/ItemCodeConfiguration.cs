using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class ItemCodeConfiguration: EntityTypeConfiguration<ItemCode>
    {
        public ItemCodeConfiguration()
        {
            Property(item => item.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(item => item.Code).IsRequired();
        }
    }
}
