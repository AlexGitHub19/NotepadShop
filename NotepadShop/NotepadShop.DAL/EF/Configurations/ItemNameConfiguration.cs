using NotepadShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
