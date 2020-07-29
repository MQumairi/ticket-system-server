using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDBContext : IdentityDbContext<User>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    { }
    public DbSet<Post> posts { get; set; }
    public DbSet<Ticket> tickets { get; set; }
    public DbSet<Comment> comments { get; set; }

    public DbSet<Product> products { get; set; }
    public DbSet<Status> status { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}