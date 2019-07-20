namespace NotepadShop.BLL.Interfaces
{
    public interface IChangePersonalInfoItemData<T>
    {
        T NewValue { get; }
        bool IsChanged { get; }
    }
}
