using NotepadShop.BLL.DTO;
using System;
using System.Security.Claims;

namespace NotepadShop.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        RegisterOperationDetails Create(UserDTO userDto);
        ClaimsIdentity Authenticate(UserDTO userDto);
        void SetInitialData();
    }
}
