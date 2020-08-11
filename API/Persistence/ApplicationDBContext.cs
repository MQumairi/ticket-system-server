using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDBContext : IdentityDbContext<User>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    { }

    //Posts
    public DbSet<Post> posts { get; set; }
    public DbSet<Ticket> tickets { get; set; }
    public DbSet<Comment> comments { get; set; }

    //Photos 
    public DbSet<Photo> photos {get; set;}
    public DbSet<Avatar> profile_pics {get; set;}
    public DbSet<Attachment> attachments {get; set;}

    //Rest
    public DbSet<Product> products { get; set; }
    public DbSet<Status> status { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}