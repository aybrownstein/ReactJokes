using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactJokes.Data
{
    public class JokeContext : DbContext
    {
        private string _connectionString;

        public JokeContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //by default Entity Framework sets all foreign key relationship delete rules
            //to be Cascade delete. This code changes it to be Restrict which is more in line
            //of what we're used to in that it will fail deleting a parent, if there are still
            //any children
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<UserLikedJokes>()
                .HasKey(qt => new { qt.UserId, qt.JokeId });

            modelBuilder.Entity<UserLikedJokes>()
                .HasOne(qt => qt.User)
                .WithMany(q => q.UserLikedJokes)
                .HasForeignKey(q => q.UserId);

            modelBuilder.Entity<UserLikedJokes>()
                .HasOne(qt => qt.Joke)
                .WithMany(t => t.UserLikedJokes)
                .HasForeignKey(q => q.JokeId);

          


            base.OnModelCreating(modelBuilder);
        }

      public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserLikedJokes> UserLikedJokes { get; set; }
    }
}

