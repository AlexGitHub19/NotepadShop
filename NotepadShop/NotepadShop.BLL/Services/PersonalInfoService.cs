using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using NotepadShop.DAL.Identity.Entities;
using NotepadShop.BLL.Entities;

namespace NotepadShop.BLL.Services
{
    public class PersonalInfoService : IPersonalInfoService
    {
        private IUnitOfWork repository { get; set; }

        public PersonalInfoService(IUnitOfWork repository)
        {
            this.repository = repository;
        }

        public void ChangePersonalInfoData(IChangePersonalInfoData data)
        {
            ApplicationUser user = repository.UserManager.FindByEmail(data.Email);
            if (data.NewFirstName.IsChanged)
            {
                user.FirstName = data.NewFirstName.NewValue;
            }
            if (data.NewSurname.IsChanged)
            {
                user.Surname = data.NewSurname.NewValue;
            }
            if (data.NewCity.IsChanged)
            {
                user.City = data.NewCity.NewValue;
            }
            if (data.NewPostDepartment.IsChanged)
            {
                user.PostDepartment = data.NewPostDepartment.NewValue;
            }
            if (data.NewPhone.IsChanged)
            {
                user.PhoneNumber = data.NewPhone.NewValue;
            }
            if (data.NewLanguage.IsChanged)
            {
                user.Language = data.NewLanguage.NewValue == null ? 
                    default(DAL.Entities.LanguageType?) : Assembler.Assemble(data.NewLanguage.NewValue.Value);
            }


            repository.UserRepository.Update(user);
            repository.Save();
        }

        public IPersonalInfo GetPersonalInfo(string email)
        {
            ApplicationUser user = repository.UserManager.FindByEmail(email);
            LanguageType? userLanguage = user.Language != null ? Assembler.Assemble(user.Language.Value) : default(LanguageType?);
            return new PersonalInfo(user.FirstName, user.Surname, user.City, user.PostDepartment, user.PhoneNumber, userLanguage);
        }
    }
}
