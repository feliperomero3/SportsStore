using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsStore.Contexts;

namespace SportsStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    ApplicationDbInitializer.Initialize(context);

                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await IdentitySeedData.SeedAsync(userManager);
                }
                catch (SqlException e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(e, "An error occurred creating the DB.");

                    throw;
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
