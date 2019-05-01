using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void createItem(IBriefItem item)
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
        }

        public IItem getItemByCode(string code)
        {
            DAL.Entities.Item found = repository.ItemRepository.GetAll().Where(item => item.Code == code).FirstOrDefault();
            return Assembler.Assemble(found);
        }

        public IEnumerable<IItem> getItemsByCategory(ItemCategory category)
        {
            DAL.Entities.ItemCategory assembledCategory = Assembler.Assemble(category);
            IEnumerable<DAL.Entities.Item> found = repository.ItemRepository.GetAll().Where(item => item.Category == assembledCategory).ToList();
            IEnumerable<IItem> result = new List<IItem>();
            if (found.Count() > 0)
            {
                result = Assembler.Assemble(found);
            }

            return result;
        }
    }
}
