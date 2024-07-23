using DemoApplication;
using Microsoft.AspNetCore.Identity;

public class Program
{
	public async static Task Main(string[] args)
	{
		var host = CreateHostBuilder(args).Build();

		using (var scope = host.Services.CreateScope())
		{
			var services = scope.ServiceProvider;
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger("app");
			try
			{
				var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await DemoApplication.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
				await DemoApplication.Seeds.DefaultUsers.SeedBasicUserAsync(userManager, roleManager);
				await DemoApplication.Seeds.DefaultUsers.SeedAdminAsync(userManager, roleManager);
				logger.LogInformation("Finished Seeding Default Data");
				logger.LogInformation("Application Starting");
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "An error occurred seeding the DB");
			}
		}
		host.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
}