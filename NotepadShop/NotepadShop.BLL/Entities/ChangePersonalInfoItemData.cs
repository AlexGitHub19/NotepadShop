using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class ChangePersonalInfoItemData<T>: IChangePersonalInfoItemData<T>
    {
        public T NewValue { get; }
        public bool IsChanged { get; }

        public ChangePersonalInfoItemData(T newValue, bool isChanged)
        {
            NewValue = newValue;
            IsChanged = isChanged;
        }
    }
}
