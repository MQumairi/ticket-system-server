using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Persistence
{
    public class Seed
    {
        public static async Task seedTickets(ApplicationDBContext context, UserManager<User> userManager)
        {

            //Seeding Users
            if (!userManager.Users.Any())
            {
                List<User> usersToAdd = new List<User>()
                {
                    new User{
                        UserName = "Bob",
                        Email = "Bob@email.com"
                    },
                    new User{
                        UserName = "Billy",
                        Email = "Billy@email.com"
                    }
                };

                foreach (var user in usersToAdd)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
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
                List<Status> status = new List<Status>()
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
                        status_color = "green"
                    }
                };
            }
            
            //Seeding Tickets
            if (!context.tickets.Any())
            {
                List<Ticket> ticketsToAdd = new List<Ticket>()
                {
                    new Ticket {
                        author_id = 1,
                        status_id = 1,
                        product_id = 1,
                        title = "Crashes after update",
                        date_time = DateTime.Now,
                        description = "The app installation crashes after updating to version 10.15.5.",
                        },

                    new Ticket {
                        author_id = 1,
                        status_id = 1,
                        product_id = 1,
                        title = "Images not loading",
                        date_time = DateTime.Now,
                        description = "All images show up as question marks",
                        },

                    new Ticket {
                        author_id = 1,
                        status_id = 1,
                        product_id = 1,
                        title = "Long loading time on startup",
                        date_time = DateTime.Now,
                        description = "Takes over 2 minutes to run the app after updating to 10.15.5!",
                        }
                };

                context.tickets.AddRange(ticketsToAdd);
                context.SaveChanges();
            }

            //Seeding Comments
            if (!context.comments.Any())
            {
                List<Comment> commentsToAdd = new List<Comment>()
                {
                    new Comment {
                        parent_post_id = 1,
                        author_id = 2,
                        date_time = DateTime.Now,
                        description = "Working on it now"
                    },

                    new Comment {
                        parent_post_id = 2,
                        author_id = 2,
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