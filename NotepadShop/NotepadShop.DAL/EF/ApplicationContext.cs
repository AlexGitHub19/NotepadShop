using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.Identity.Entities;

namespace NotepadShop.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString)
        {
        }
    }
}
