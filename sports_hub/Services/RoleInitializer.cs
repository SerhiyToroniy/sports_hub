using Microsoft.AspNetCore.Identity;
using sports_hub.Models.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            Image DefaultImg = Image.FromFile("wwwroot\\imagesAdminPage\\user test icon.png");
            byte[] DefaultImgArr;
            using (MemoryStream ms = new MemoryStream())
            {
                DefaultImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                DefaultImgArr = ms.ToArray();
            }
            string adminEmail = "admin@gmail.com";
            string password = "123qaz123";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, FirstName = "Admin", LastName = "Admin",
                    ProfileImage = DefaultImgArr, Status = UserStatus.Active, Role = "admin", RegistrationDate = DateTime.Now
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
