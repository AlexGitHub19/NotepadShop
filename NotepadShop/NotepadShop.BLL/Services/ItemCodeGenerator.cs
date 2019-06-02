using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Entities;
using NotepadShop.DAL.Interfaces;
using System.Linq;

namespace NotepadShop.BLL.Services
{
    public class ItemCodeGenerator : IItemCodeGenerator
    {
        private IUnitOfWork repository { get; set; }

        public ItemCodeGenerator(IUnitOfWork repository)
        {
            this.repository = repository;
        }

        public string generateCode()
        {
            int newCode = 1;
            ItemCode itemCode = repository.ItemCodeRepository.GetAll().FirstOrDefault();
            if (itemCode != null)
            {
                string lastCode = itemCode.Code;
                newCode = int.Parse(lastCode) + 1;
                itemCode.Code = newCode.ToString();
            }
            else
            {
                ItemCode item = new ItemCode { Code = newCode.ToString() };
                repository.ItemCodeRepository.Create(item);
            }

            repository.Save();

            return newCode.ToString();
        }
    }
}
