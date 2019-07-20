using NotepadShop.BLL.Interfaces;

namespace NotepadShop.BLL.Entities
{
    public class PersonalInfo: IPersonalInfo
    {
        public string FirstName { get; }
        public string Surname { get; }
        public string City { get; }
        public string PostDepartment { get; }
        public string Phone { get; }
        public LanguageType? Language { get; }

        public PersonalInfo(string firstName, string surname, string city, string postDepartment, string phone, 
            LanguageType? language)
        {
            FirstName = firstName;
            Surname = surname;
            City = city;
            PostDepartment = postDepartment;
            Phone = phone;
            Language = language;
        }
    }
}
