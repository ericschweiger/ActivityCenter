using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class ActivityContext : DbContext
    {
        public ActivityContext(DbContextOptions options) : base(options){}
        public DbSet<User> UserList {get; set;}
        public DbSet<ActivityEvent> ActivityList {get; set;}
        public DbSet<Join> Joiners {get; set;}
    }
}