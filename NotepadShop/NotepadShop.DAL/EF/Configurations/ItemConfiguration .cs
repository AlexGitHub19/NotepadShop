﻿using NotepadShop.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace NotepadShop.DAL.EF.Configurations
{
    public class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration()
        {
            Property(item => item.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(item => item.Code).IsRequired();
            Property(item => item.Price).IsRequired();
            Property(item => item.Category).IsRequired();
            Property(item => item.AddingTime).IsRequired();
            HasMany(item => item.Names);
        }
    }
}
