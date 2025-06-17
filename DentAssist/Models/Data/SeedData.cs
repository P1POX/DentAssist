using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task InitializeRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = new string[] { "Administrador", "Recepcionista", "Odontologo", "Paciente" };

        foreach (string role in roles)
        {
            bool exists = await roleManager.RoleExistsAsync(role);
            if (!exists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task InitializeAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        string adminEmail = "admin@sonrisaplena.com";
        string adminPassword = "Pass123.";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
        }
    }
}
