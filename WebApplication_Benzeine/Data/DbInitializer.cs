using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data.Models.Domain;

namespace WebApplication_Benzeine.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            #region Seed Roles

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new AppRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new AppRole("User"));

            #endregion

            #region Seed User Admin 

            var userManager = serviceProvider.GetService<UserManager<AppUser>>();
            var admin = (await userManager.GetUsersInRoleAsync("Admin")).FirstOrDefault();

            if (admin == null)
            {
                admin = new AppUser() { UserName = "admin@admin.com", Email = "admin@admin.com" };

                if (!(await userManager.CreateAsync(admin)).Succeeded)
                    throw new Exception("Could create Admin user");

                await userManager.AddToRoleAsync(admin, "Admin");
            }

            await userManager.AddPasswordAsync(admin, "Admin1414@");

            #endregion

            using (var context = serviceProvider.GetRequiredService<DataContext>())
            {
                #region Product Seeding

                if (!context.Set<Product>().Any())
                {
                    var random = new Random();

                    var bookCat = new Category() { Name = "Book" };
                    var albumCat = new Category() { Name = "Album"};
                    var drinkCat = new Category() { Name = "Drinks", };

                    await context.AddRangeAsync(bookCat, albumCat, drinkCat);
                    await context.SaveChangesAsync();

                    var products = new Product[]
                    {
                        new Product()
                        {
                            CategoryId = bookCat.Id,
                            UserId = admin.Id,
                            Name = "Ernest Hamingway - For who the beel calls",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },
                        new Product()
                        {
                            CategoryId = bookCat.Id,
                            UserId = admin.Id,
                            Name = "Chares Darwin - On the origin of species",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },
                        new Product()
                        {
                            CategoryId = albumCat.Id,
                            UserId = admin.Id,
                            Name = "Pink Floyd - On the dark side of the moon",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },
                        new Product()
                        {
                            CategoryId = albumCat.Id,
                            UserId = admin.Id,
                            Name = "Creedence - Fortunate Son",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },
                        new Product()
                        {
                            CategoryId = albumCat.Id,
                            UserId = admin.Id,
                            Name = "Creedence - Fortunate Son",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },

                        new Product()
                        {
                            CategoryId = drinkCat.Id,
                            UserId = admin.Id,
                            Name = "Hennessey Brandy",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        },
                        new Product()
                        {
                            CategoryId = drinkCat.Id,
                            UserId = admin.Id,
                            Name = "Bombay Gin",
                            CreatedAt = DateTime.UtcNow,
                            Price = random.Next(0,100)
                        }
                  };

                    context.AddRange(products);
                    context.SaveChanges();
                }

                #endregion
            }
        }
    }
}

