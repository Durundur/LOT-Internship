using Microsoft.AspNetCore.Identity;
using LOT_TASK.Models;

namespace LOT_TASK.Helpers
{
    public static class UserSampleData
    {
        public static async Task SeedUsers(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var cntx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
                if (!await cntx.Database.EnsureCreatedAsync())
                {
                    var adminRole = new RoleModel()
                    {
                        Name = "admin",
                    };
                    var userRole = new RoleModel()
                    {
                        Name= "user",
                    };

                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);
                    var adminUser = new UserModel() { UserName = "admin@test.com", Email = "admin@test.com"};
                    var user = new UserModel() { UserName = "user@test.com", Email = "user@test.com" };
                    await userManager.CreateAsync(adminUser, "123");
                    await userManager.CreateAsync(user, "123");
                    await userManager.AddToRoleAsync(adminUser, "admin");
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
