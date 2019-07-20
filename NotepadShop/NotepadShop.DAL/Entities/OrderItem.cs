using System;

namespace NotepadShop.DAL.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
    }
}
