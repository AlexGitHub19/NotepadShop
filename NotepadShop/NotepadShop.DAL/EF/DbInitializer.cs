using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.Identity;
using NotepadShop.DAL.Identity.Entities;
using System;
using System.Data.Entity;

namespace NotepadShop.DAL.EF
{
    class DbInitializer : DropCreateDatabaseAlways<ApplicationContext>
    {

        protected async override void Seed(ApplicationContext context)
        {
            ApplicationRole userRole = new ApplicationRole { Name = "user" };
            ApplicationRole adminRole = new ApplicationRole { Name = "admin" };
            ApplicationRoleManager roleManager = 
                new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            await roleManager.CreateAsync(userRole);
            await roleManager.CreateAsync(adminRole);

            ApplicationUser admin = new ApplicationUser { UserName = "admin",
                RegistrationDateTime = DateTime.UtcNow };
            ApplicationUserManager userManager = 
                new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            await userManager.CreateAsync(admin, "1234qwerA");
            await userManager.AddToRoleAsync(admin.Id, adminRole.Name);
            context.SaveChanges();
        }
    }
}
