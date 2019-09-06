using NotepadShop.BLL.Entities;
using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace NotepadShop.BLL.Services
{
    public class ItemService : IItemService
    {
        private IUnitOfWork repository { get; set; }
        private IItemCodeGenerator itemCodeGenerator { get; set; }

        public ItemService(IUnitOfWork repository, IItemCodeGenerator itemCodeGenerator)
        {
            this.repository = repository;
            this.itemCodeGenerator = itemCodeGenerator;
        }

        public string createItem(IBriefItem item)
        {
            DAL.Entities.Item assembled = new DAL.Entities.Item
            {
                Code = itemCodeGenerator.generateCode(),
                Price = item.Price,
                Category = Assembler.Assemble(item.Category),
                AddingTime = DateTime.UtcNow,
                Names = Assembler.Assemble(item.Names)
            };
            repository.ItemRepository.Create(assembled);
            repository.Save();

            return assembled.Code;
        }

        public IItem getItemByCode(string code)
        {
            DAL.Entities.Item found = repository.ItemRepository.GetAll().
                Where(item => item.Code == code).
                Include(item => item.Names).
                FirstOrDefault();
            return Assembler.Assemble(found);
        }

        public IItemsData getItemsByCategory(ItemCategory category, int countOnPage, int page)
        {
            DAL.Entities.ItemCategory assembledCategory = Assembler.Assemble(category);

            IEnumerable<DAL.Entities.Item> found = repository.ItemRepository.
                GetAll().Where(item => item.Category == assembledCategory).
                OrderByDescending(item => item.AddingTime).
                Skip((page-1) * countOnPage).
                Take(countOnPage).
                Include(item => item.Names).
                ToList();

            IEnumerable<IItem> items = Assembler.Assemble(found.ToList());

            int itemsTotalCount = repository.ItemRepository.GetAll().Where(item => item.Category == assembledCategory).Count();

            return new ItemsData(itemsTotalCount, items);
        }

        public void deleteItemByCode(string code)
        {
            DAL.Entities.Item foundItem = repository.ItemRepository.GetAll().First(item =>item.Code == code);
            repository.ItemRepository.Delete(foundItem.Id);
            repository.Save();
        }

        public void changeItem(IChangeItemData itemData)
        {
            DAL.Entities.Item foundItem = repository.ItemRepository.GetAll().Include(item => item.Names).
                First(item => item.Code == itemData.Code);

            if (itemData.NewPrice != null)
            {
                foundItem.Price = itemData.NewPrice.Value;
            }
            if (itemData.NewCategory != null)
            {
                foundItem.Category = Assembler.Assemble(itemData.NewCategory.Value);
            }            
            foreach (DAL.Entities.ItemName itemName in foundItem.Names)
            {
                switch (itemName.LanguageType)
                {
                    case DAL.Entities.LanguageType.English:
                        if (!string.IsNullOrEmpty(itemData.NewEnName))
                        {
                            itemName.Name = itemData.NewEnName;
                        }                        
                        break;
                    case DAL.Entities.LanguageType.Russian:
                        if (!string.IsNullOrEmpty(itemData.NewRuName))
                        {
                            itemName.Name = itemData.NewRuName;
                        }
                        break;
                    case DAL.Entities.LanguageType.Ukrainian:
                        if (!string.IsNullOrEmpty(itemData.NewUkName))
                        {
                            itemName.Name = itemData.NewUkName;
                        }
                        break;
                }
            }

            repository.ItemRepository.Update(foundItem);
            repository.Save();
        }

        public IEnumerable<IItem> getMostPopularItems(int count)
        {
            IEnumerable<DAL.Entities.Item> found = repository.ItemRepository.
                GetAll().OrderBy(item => item.OrderItems.Count).
                Take(count).
                Include(item => item.OrderItems).
                Include(item => item.Names).
                ToList();

            return Assembler.Assemble(found.ToList());
        }
    }
}
