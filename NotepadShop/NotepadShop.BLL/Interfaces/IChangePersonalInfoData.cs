namespace NotepadShop.BLL.Interfaces
{
    public interface IChangePersonalInfoData
    {
        string Email { get; }
        IChangePersonalInfoItemData<string> NewFirstName { get; }
        IChangePersonalInfoItemData<string> NewSurname { get; }
        IChangePersonalInfoItemData<string> NewCity { get; }
        IChangePersonalInfoItemData<string> NewPostDepartment { get; }
        IChangePersonalInfoItemData<string> NewPhone { get; }
        IChangePersonalInfoItemData<LanguageType?> NewLanguage { get; }
    }
}
