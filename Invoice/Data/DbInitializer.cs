using Invoice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Data
{
    //internal class DbInitializer
    //{
    //    internal static void Seed(IServiceScope scoped)
    //    {
    //        using (var _context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    //        {
    //            var manager = scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


    //            if (!manager.Users.Any())
    //            {
    //                var configuration = scoped.ServiceProvider.GetRequiredService<IConfiguration>();
    //                ApplicationUser user = new ApplicationUser
    //                {
    //                    UserName = configuration["User:username"],
    //                    Email = configuration["User:email"]

    //                };
    //                ApplicationUser user1 = new ApplicationUser
    //                {
    //                    UserName ="nurlan" ,
    //                    Email = "nurlan@gmail.com"

    //                };
    //                ApplicationUser user2 = new ApplicationUser
    //                {
    //                    UserName = "orxan",
    //                    Email = "orxan@gmail.com"

    //                };
    //                ApplicationUser user3 = new ApplicationUser
    //                {
    //                    UserName = "nigar",
    //                    Email = "nigar@gmail.com"

    //                };
    //                manager.CreateAsync(user, configuration["User:password"]).GetAwaiter().GetResult();
    //                manager.CreateAsync(user1, "user@123").GetAwaiter().GetResult();
    //                manager.CreateAsync(user2, "user@123").GetAwaiter().GetResult();
    //                manager.CreateAsync(user3, "user@123").GetAwaiter().GetResult();
    //                var roles = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    //                List<IdentityRole<int>> list = new List<IdentityRole<int>>();

    //                if (!roles.Roles.Any())
    //                {

    //                    string[] UserRoles = configuration["Roles"].Split(",");
    //                    foreach (var role in UserRoles)
    //                    {
    //                        IdentityRole<int> identityRole = new IdentityRole<int>
    //                        {
    //                            Name = role
    //                        };
    //                        list.Add(identityRole);
    //                        roles.CreateAsync(identityRole).GetAwaiter().GetResult();

    //                    }
    //                }
    //                manager.AddToRoleAsync(user, list[0].Name).GetAwaiter().GetResult();
    //                manager.AddToRoleAsync(user1, list[1].Name).GetAwaiter().GetResult();
    //                manager.AddToRoleAsync(user2, list[1].Name).GetAwaiter().GetResult();
    //                manager.AddToRoleAsync(user3, list[1].Name).GetAwaiter().GetResult();
    //            }
    //        }
    //    }
    //}

    internal class DbInitializer
    {

        internal static void Seed(IServiceScope scoped)
        {
            using (var _context = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {

                var manager = scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


                if (!manager.Users.Any())
                {
                    var configuration = scoped.ServiceProvider.GetRequiredService<IConfiguration>();
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = configuration["User:username"],
                        Email = configuration["User:email"]

                    };
                    ApplicationUser user1 = new ApplicationUser
                    {
                        UserName = "nurlan",
                        Email = "nurlan@gmail.com"

                    };
                    ApplicationUser user2 = new ApplicationUser
                    {
                        UserName = "orxan",
                        Email = "orxan@gmail.com"

                    };
                    ApplicationUser user3 = new ApplicationUser
                    {
                        UserName = "nigar",
                        Email = "nigar@gmail.com"

                    };

                    manager.CreateAsync(user, configuration["User:password"]).GetAwaiter().GetResult();
                    manager.CreateAsync(user1, "User@123").GetAwaiter().GetResult();
                    manager.CreateAsync(user2, "User@123").GetAwaiter().GetResult();
                    manager.CreateAsync(user3, "User@123").GetAwaiter().GetResult();

                    var roles = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                    List<IdentityRole<int>> list = new List<IdentityRole<int>>();

                    if (!roles.Roles.Any())
                    {

                        string[] UserRoles = configuration["Roles"].Split(",");
                        foreach (var role in UserRoles)
                        {
                            IdentityRole<int> identityRole = new IdentityRole<int>
                            {
                                Name = role
                            };
                            list.Add(identityRole);
                            roles.CreateAsync(identityRole).GetAwaiter().GetResult();

                        }
                    }
                    manager.AddToRoleAsync(user, list[0].Name).GetAwaiter().GetResult();
                    manager.AddToRoleAsync(user1, list[1].Name).GetAwaiter().GetResult();
                    manager.AddToRoleAsync(user2, list[1].Name).GetAwaiter().GetResult();
                    manager.AddToRoleAsync(user3, list[1].Name).GetAwaiter().GetResult();
                }

               

            }

        }
    }
}
