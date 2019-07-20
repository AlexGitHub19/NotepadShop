namespace NotepadShop.Models.Profile
{
    public class ChangePersonalInfoData
    {
        public ChangePersonalInfoItemData<string> NewFirstName { get; set; }
        public ChangePersonalInfoItemData<string> NewSurname { get; set; }
        public ChangePersonalInfoItemData<string> NewCity { get; set; }
        public ChangePersonalInfoItemData<string> NewPostDepartment { get; set; }
        public ChangePersonalInfoItemData<string> NewPhone { get; set; }
        public ChangePersonalInfoItemData<string> NewLanguage { get; set; }
    }
}