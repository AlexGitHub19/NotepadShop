using NotepadShop.BLL.DTO;
using System;
using System.Security.Claims;

namespace NotepadShop.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        RegisterOperationDetails Create(UserDTO userDto);
        ClaimsIdentity Authenticate(UserDTO userDto);
        LanguageType? GetUserLanguage(string email);
        bool ChangePassword(string userId, string currentPassword, string newPassword);
        void SetInitialData();
        bool IsInitialDataSet();
    }
}
