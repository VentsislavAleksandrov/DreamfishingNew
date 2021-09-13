using DreamFishingNew.Data;
using DreamFishingNew.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DreamFishingNew.Infrastructure
{
    using static WebConstants;
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {            
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var data = scopedServices.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            data.Database.Migrate();
            SeedAdministrator( serviceProvider);
            
            return app;
        }

        private static void SeedAdministrator ( IServiceProvider services)
        {            
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@mail.com";
                    const string adminPassword = "111111";

                    var user = new ApplicationUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,                        
                    };

                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }        
    }
}
