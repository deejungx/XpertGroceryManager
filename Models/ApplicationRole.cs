using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XpertGroceryManager.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        { 
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}