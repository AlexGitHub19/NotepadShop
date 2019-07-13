using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class OrderItem: IOrderItem
    {
        public IItem Item { get; }
        public int Count { get; }

        public OrderItem(IItem item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}
