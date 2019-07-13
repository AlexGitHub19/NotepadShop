using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Entities;
using NotepadShop.DAL.Interfaces;
using System.Linq;

namespace NotepadShop.BLL.Services
{
    public class OrderNumberGenerator: IOrderNumberGenerator
    {
        private IUnitOfWork repository { get; set; }

        public OrderNumberGenerator(IUnitOfWork repository)
        {
            this.repository = repository;
        }

        public string GenerateNumber()
        {
            int newNumber = 1;
            OrderNumber orderNumber = repository.OrderNumberRepository.GetAll().FirstOrDefault();
            if (orderNumber != null)
            {
                string currentNumber = orderNumber.Number;
                newNumber = int.Parse(currentNumber) + 1;
                orderNumber.Number = newNumber.ToString();
            }
            else
            {
                OrderNumber item = new OrderNumber { Number = newNumber.ToString() };
                repository.OrderNumberRepository.Create(item);
            }

            repository.Save();

            return newNumber.ToString();
        }
    }
}
