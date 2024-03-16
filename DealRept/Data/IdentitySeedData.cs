using DealRept.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DealRept.Data
{
    public class IdentitySeedData
    {
        public static void CreateAdminAccount(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
             CreateAdminAccountAsync(serviceProvider, configuration).Wait();
        }

        public static async Task CreateAdminAccountAsync(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            serviceProvider = serviceProvider.CreateScope().ServiceProvider;

            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            int employeeNumber=(int.TryParse(configuration["AdminConfiguration:EmployeeNumber"], out employeeNumber)==false)?10000:employeeNumber;
            string email = configuration["AdminConfiguration:Email"] ?? "test@test.com";
            string userName = email;
            string password = configuration["AdminConfiguration:Password"] ?? "" +
                "your_password";
            string role = configuration["AdminConfiguration:Role"] ?? "Administrator";

            if (await userManager.FindByNameAsync(userName) == null)
            {
                if (await roleManager.FindByNameAsync(role)==null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                User user = new User
                {
                    Email = email,
                    UserName=userName,
                    EmployeeNumber = employeeNumber,
                    FirstName="admin",
                    LastName = "admin",
                    MiddleName = "admin",
                    RegistrationDateUTC=DateTime.Now.ToUniversalTime(),
                    EmailConfirmed=true,
                    IsApproved=true
                };
                IdentityResult result = await userManager
                    .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }

            }
        }
    }
}
