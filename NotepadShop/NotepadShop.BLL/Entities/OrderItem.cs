using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class OrderItem: IOrderItem
    {
        public IItem Item { get; }
        public int Count { get; }
        public decimal Price { get; }

        public OrderItem(IItem item, int count, decimal price)
        {
            Item = item;
            Count = count;
            Price = price;
        }
    }
}
