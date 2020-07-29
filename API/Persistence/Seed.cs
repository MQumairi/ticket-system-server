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
        }
    }
}