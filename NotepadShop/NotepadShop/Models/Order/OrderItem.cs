using NotepadShop.Models.ItemModels;

namespace NotepadShop.Models.Order
{
    public class OrderItem
    {
        public Item Item { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}