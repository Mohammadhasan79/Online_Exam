using Microsoft.AspNetCore.Identity;
using OnlineExam.DbContext;
using OnlineExam.Entity;

namespace OnlineExam.Seader
{
    public static class Seed
    {
        public static async Task RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }
            if (!await roleManager.RoleExistsAsync("Prof"))
            {
                await roleManager.CreateAsync(new IdentityRole("Prof"));
            }
        }
    }
}
