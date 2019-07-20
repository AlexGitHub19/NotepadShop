namespace NotepadShop.Models.Profile
{
    public class ChangePersonalInfoItemData<T>
    {
        public T NewValue { get; set; }
        public bool IsChanged { get; set; }
    }
}