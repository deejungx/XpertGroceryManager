using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XpertGroceryManager.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}