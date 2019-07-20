namespace NotepadShop.BLL.Interfaces
{
    public interface IPersonalInfoService
    {
        IPersonalInfo GetPersonalInfo(string email);
        void ChangePersonalInfoData(IChangePersonalInfoData data);
    }
}
