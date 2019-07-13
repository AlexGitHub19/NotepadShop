using Microsoft.AspNet.Identity.EntityFramework;
using NotepadShop.DAL.Entities;
using System;
using System.Collections.Generic;

namespace NotepadShop.DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDateTime { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public ApplicationUser()
        {
            Orders = new List<Order>();
        }
    }
}
