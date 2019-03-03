using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.Identity.Entities;
using System.Data.Entity;

namespace NotepadShop.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString)
        {
            //Database.SetInitializer<ApplicationContext>(new DbInitializer());
        }
    }
}
