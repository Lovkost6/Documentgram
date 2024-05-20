using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Document.Models;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Message> Messages { get; set; }

    public DbSet<MessageRecipient> MessageRecipients { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageRecipient>(entity =>
        {
            entity.HasKey(e => new { e.MessageId, UsertId = e.UserId  });
        });
    }
}