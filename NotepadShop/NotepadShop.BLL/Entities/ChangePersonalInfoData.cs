using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class ChangePersonalInfoData: IChangePersonalInfoData
    {
        public string Email { get; }
        public IChangePersonalInfoItemData<string> NewFirstName { get; }
        public IChangePersonalInfoItemData<string> NewSurname { get; }
        public IChangePersonalInfoItemData<string> NewCity { get; }
        public IChangePersonalInfoItemData<string> NewPostDepartment { get; }
        public IChangePersonalInfoItemData<string> NewPhone { get; }
        public IChangePersonalInfoItemData<LanguageType?> NewLanguage { get; }

        public ChangePersonalInfoData(string email,
            IChangePersonalInfoItemData<string> newFirstName,
            IChangePersonalInfoItemData<string> newSurname,
            IChangePersonalInfoItemData<string> newCity,
            IChangePersonalInfoItemData<string> newPostDepartment,
            IChangePersonalInfoItemData<string> newPhone,
            IChangePersonalInfoItemData<LanguageType?> newLanguage)
        {
            Email = email;
            NewFirstName = newFirstName;
            NewSurname = newSurname;
            NewCity = newCity;
            NewPostDepartment = newPostDepartment;
            NewPhone = newPhone;
            NewLanguage = newLanguage;
        }
    }
}
