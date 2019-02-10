using Microsoft.AspNet.Identity;
using NotepadShop.DAL.Identity.Entities;

namespace NotepadShop.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}
