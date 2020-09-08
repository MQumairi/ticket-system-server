using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Persistence
{
    public class Seed
    {
        public IConfiguration config { get; }

        public Seed(IConfiguration config)
        {
            this.config = config;
        }

        public async Task seedTickets(ApplicationDBContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            List<User> usersToAdd = new List<User>();

            //Seeding Roles
            if (!roleManager.Roles.Any())
            {
                var adminRole = new Role
                {
                    Name = "Admin",
                    color = "orange",
                    can_manage = true,
                    can_moderate = true
                };

                await roleManager.CreateAsync(adminRole);

                var devRole = new Role
                {
                    Name = "Developer",
                    color = "purple",
                    can_manage = true,
                    can_moderate = false
                };

                await roleManager.CreateAsync(devRole);
            }

            //Seeding Users
            if (!userManager.Users.Any())
            {
                User adminAccount = new User
                {
                    UserName = "MQumairi",
                    Email = "moh.alqumairi@gmail.com",
                    first_name = "Mohammed",
                    surname = "Alqumairi"
                };

                usersToAdd.Add(new User
                {
                    UserName = "BBob",
                    Email = "Bob@email.com",
                    first_name = "Billy",
                    surname = "Bob"
                });

                usersToAdd.Add(new User
                {
                    UserName = "ToshiToshi",
                    Email = "Toshi@email.com",
                    first_name = "Toshi",
                    surname = "Toshi"
                });

                usersToAdd.Add(adminAccount);

                foreach (var user in usersToAdd)
                {
                    string defaultPass = config["defaultPass"];
                    await userManager.CreateAsync(user, defaultPass);
                }

                await userManager.AddToRoleAsync(adminAccount, "Admin");


                if (!context.acp_settings.Any())
                {
                    ACPSettings aCPSettings = new ACPSettings
                    {
                        founder_id = adminAccount.Id,
                        registration_locked = false
                    };
                    context.acp_settings.Add(aCPSettings);
                }
            }
            else
            {
                usersToAdd = await context.Users.ToListAsync();
            }

            //Seeding Products
            if (!context.products.Any())
            {
                List<Product> productsToAdd = new List<Product>()
                {
                    new Product{
                        product_name = "MacOs",
                        version = "10.15.5"
                    },

                    new Product{
                        product_name = "iOS",
                        version = "14.0.0"
                    }
                };

                context.products.AddRange(productsToAdd);
                context.SaveChanges();
            }

            //Seeding Status
            if (!context.status.Any())
            {
                List<Status> statusToAdd = new List<Status>()
                {
                    new Status {
                        status_text = "Urgent",
                        status_color = "red"
                    },
                    new Status {
                        status_text = "Low",
                        status_color = "orange"
                    },
                    new Status {
                        status_text = "Pending",
                        status_color = "yellow"
                    },
                    new Status {
                        status_text = "Done",
                        status_color = "green",
                        is_default = true
                    }
                };

                context.status.AddRange(statusToAdd);
                context.SaveChanges();
            }

            //Seeding Tickets
            if (!context.tickets.Any())
            {
                List<Ticket> ticketsToAdd = new List<Ticket>()
                {
                    new Ticket {
                        author_id = usersToAdd[0].Id,
                        status_id = 1,
                        product_id = 1,
                        title = "Crashes after update",
                        date_time = DateTime.Now,
                        description = "The app installation crashes after updating to version 10.15.5.",
                        },

                    new Ticket {
                        author_id = usersToAdd[0].Id,
                        status_id = 1,
                        product_id = 1,
                        title = "Images not loading",
                        date_time = DateTime.Now,
                        description = "All images show up as question marks",
                        },

                    new Ticket {
                        author_id = usersToAdd[0].Id,
                        status_id = 1,
                        product_id = 1,
                        title = "Long loading time on startup",
                        date_time = DateTime.Now,
                        description = "Takes over 2 minutes to run the app after updating to 10.15.5!",
                        }
                };

                context.tickets.AddRange(ticketsToAdd);
                context.SaveChanges();

                //Seeding Comments
                if (!context.comments.Any())
                {
                    List<Comment> commentsToAdd = new List<Comment>()
                {
                    new Comment {
                        parent_post_id = ticketsToAdd[2].post_id,
                        author_id = usersToAdd[1].Id,
                        date_time = DateTime.Now,
                        description = "Working on it now"
                    },

                    new Comment {
                        parent_post_id = ticketsToAdd[2].post_id,
                        author_id = usersToAdd[1].Id,
                        date_time = DateTime.Now,
                        description = "You're having a networking problem"
                    }
                };

                    context.comments.AddRange(commentsToAdd);
                    context.SaveChanges();
                }
            }


        }
    }
}