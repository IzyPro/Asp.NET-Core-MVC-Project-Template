using System;
using System.Threading.Tasks;
using Template.Enums;
using Template.Models;
using Microsoft.AspNetCore.Identity;

namespace Template.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(UserRolesEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRolesEnum.User.ToString()));
        }
    }
}
