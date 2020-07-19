using API.Persistence;
using Microsoft.EntityFrameworkCore;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    { }

    public DbSet<Ticket> tickets { get; set; }

    // protected override void OnModelCreating(ModelBuilder builder){
        // Seed.seedTickets(this);
        // builder.Entity<Ticket>().HasData(
        //     new Ticket {
        //         Id = 1,
        //         authorId = 1,
        //         status = "Urgent",
        //         product = "Product 1",
        //         title = "Crashes after update",
        //         date = "2020-01-04",
        //         description = "The app installation crashes after updating to version 10.15.5.",
        //     },

        //     new Ticket {
        //         Id = 2,
        //         authorId = 2,
        //         status = "Low",
        //         product = "Product 1",
        //         title = "Not connecting to database",
        //         date = "2020-01-04",
        //         description = "Displays a 502 error w",
        //     },

        //     new Ticket {
        //         Id = 3,
        //         authorId = 2,
        //         status = "Low",
        //         product = "Product 2",
        //         title = "Crashes after update",
        //         date = "2020-01-04",
        //         description = "The app installation crashes after updating to version 10.15.5.",
        //     },

        //     new Ticket {
        //         Id = 4,
        //         authorId = 1,
        //         status = "Done",
        //         product = "Product 4",
        //         title = "Crashes after update",
        //         date = "2020-01-04",
        //         description = "The app installation crashes after updating to version 10.15.5.",
        //     }
        // );
    // }
}