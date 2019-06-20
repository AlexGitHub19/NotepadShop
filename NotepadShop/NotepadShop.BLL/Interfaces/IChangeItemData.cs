namespace NotepadShop.BLL.Interfaces
{
    public interface IChangeItemData
    {
        string Code { get; }
        decimal? NewPrice { get; }
        ItemCategory? NewCategory { get; }
        string NewRuName { get; }
        string NewUkName { get; }
        string NewEnName { get; }
    }
}
