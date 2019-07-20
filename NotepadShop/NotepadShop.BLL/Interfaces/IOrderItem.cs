namespace NotepadShop.BLL.Interfaces
{
    public interface IOrderItem
    {
        IItem Item { get; }
        int Count { get; }
        decimal Price { get; }
    }
}
