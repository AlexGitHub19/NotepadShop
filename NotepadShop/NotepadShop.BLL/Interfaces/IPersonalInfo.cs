namespace NotepadShop.BLL.Interfaces
{
    public interface IPersonalInfo
    {
        string FirstName { get; }
        string Surname { get; }
        string City { get; }
        string PostDepartment { get; }
        string Phone { get; }
        LanguageType? Language { get; }
    }
}
