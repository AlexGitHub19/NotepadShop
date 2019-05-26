using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class ItemNameConfiguration: EntityTypeConfiguration<ItemName>
    {
        public ItemNameConfiguration()
        {
            Property(itemName => itemName.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(itemName => itemName.LanguageType).IsRequired();
            Property(item => item.Name).IsRequired();
        }
    }
}
