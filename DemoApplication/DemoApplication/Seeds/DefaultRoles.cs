using DemoApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace DemoApplication.Seeds
{
	public static class DefaultRoles
	{
		public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
		}
	}
}
