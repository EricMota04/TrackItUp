using Microsoft.EntityFrameworkCore;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Context
{
    public class TrackItUpContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitTracking> HabitTrackings { get; set; }

        public TrackItUpContext(DbContextOptions<TrackItUpContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Habit>()
                .HasKey(h => h.HabitId); 

            modelBuilder.Entity<Habit>()
                .Property(h => h.HabitId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<HabitTracking>()
                .HasKey(ht => ht.HabitTrackingId);

            modelBuilder.Entity<HabitTracking>()
                .Property(ht => ht.HabitTrackingId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasKey(h => h.UserId);

            modelBuilder.Entity<User>()
                .Property(h => h.UserId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Habit>()
                .HasMany(h => h.HabitTrackings) 
                .WithOne(ht => ht.Habit) 
                .HasForeignKey(ht => ht.HabitId); 

            modelBuilder.Entity<User>()
                .HasMany(u => u.Habits) 
                .WithOne(h => h.User) 
                .HasForeignKey(h => h.UserId); 

            
        }
    }
}
