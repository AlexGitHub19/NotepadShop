using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace NotepadShop.DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDateTime { get; set; }
    }
}
