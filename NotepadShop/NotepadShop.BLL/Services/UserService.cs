using NotepadShop.BLL.DTO;
using NotepadShop.BLL.Interfaces;
using NotepadShop.DAL.Identity.Entities;
using NotepadShop.DAL.Interfaces;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System;
using NotepadShop.DAL.Identity;

namespace NotepadShop.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork repository { get; set; }

        public UserService(IUnitOfWork repository)
        {
            this.repository = repository;
        }

        public RegisterOperationDetails Create(UserDTO userDto)
        {
            ApplicationUser user = repository.UserManager.FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email,
                    RegistrationDateTime = DateTime.UtcNow };
                var result = repository.UserManager.Create(user, userDto.Password);
                if (result.Errors.Count() > 0)
                {
                    return new RegisterOperationDetails(false, ErrorType.IdentityErrorWhileCreating);

                }
                repository.UserManager.AddToRole(user.Id, userDto.Role);
                repository.Save();
                return new RegisterOperationDetails(true, ErrorType.None);
            }
            else
            {
                return new RegisterOperationDetails(false, ErrorType.UserWithSuchEmailAlreadyExists);
            }
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = repository.UserManager.Find(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = repository.UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public void SetInitialData()
        {
            ApplicationRole userRole = new ApplicationRole { Name = "user" };
            ApplicationRole adminRole = new ApplicationRole { Name = "admin" };
            ApplicationRoleManager roleManager = repository.RoleManager;
            roleManager.Create(userRole);
            roleManager.Create(adminRole);

            ApplicationUser admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin",
                RegistrationDateTime = DateTime.UtcNow
            };
            ApplicationUserManager userManager = repository.UserManager;
            userManager.Create(admin, "1234qwerA");
            userManager.AddToRole(admin.Id, adminRole.Name);
            repository.Save();
        }

        public void Dispose()
        {
            repository.Dispose();
        }
    }
}
