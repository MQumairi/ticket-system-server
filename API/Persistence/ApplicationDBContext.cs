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
    public DbSet<Photo> photos { get; set; }
    public DbSet<Avatar> profile_pics { get; set; }
    public DbSet<Attachment> attachments { get; set; }

    //Rest
    public DbSet<Product> products { get; set; }
    public DbSet<Status> status { get; set; }


    //Role
    public DbSet<Role> roles {get; set;}


    //App settings
    public DbSet<ACPSettings> acp_settings {get; set;}
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Ticket>()
        .HasMany(ticket => ticket.comments)
        .WithOne(comment => comment.ticket)
        .OnDelete(DeleteBehavior.NoAction);

        // builder.Entity<Comment>()
        // .HasOne(comment => comment.ticket)
        // .WithMany(ticket => ticket.comments)
        // .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Avatar>()
        .HasOne(avatar => avatar.user)
        .WithOne(user => user.avatar)
        .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Attachment>()
        .HasOne(attachment => attachment.post)
        .WithOne(post => post.attachment)
        .OnDelete(DeleteBehavior.SetNull);
    }
}