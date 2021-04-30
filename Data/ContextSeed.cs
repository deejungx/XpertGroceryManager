using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpertGroceryManager.Models;

namespace XpertGroceryManager.Data
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            await roleManager.CreateAsync(new ApplicationRole(Models.Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new ApplicationRole(Models.Enums.Roles.Basic.ToString()));

            var defaultUser = new ApplicationUser 
            { 
                UserName = "adminuser", 
                Email = "admin@xpertmanager.com",
                FirstName = "Dipesh",
                LastName = "Jung Pandey",
                EmailConfirmed = true, 
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = userManager.FindByEmailAsync(defaultUser.Email);
                if(user==null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "adminpassword");
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Admin.ToString());
                }
               
            }
        }
    }
}
