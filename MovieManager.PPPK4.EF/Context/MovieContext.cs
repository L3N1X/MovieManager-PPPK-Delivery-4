using MovieManager.PPPK4.EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieManager.PPPK4.EntityFramework
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("Name=MoviesCS")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Movie>()
                .HasRequired(m => m.Director)
                .WithMany(p => p.DirectedMovies)
                .HasForeignKey(m => m.DirectorId);

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(p => p.ActedMovies)
                .Map(ma =>
                {
                    ma.MapLeftKey("MovieId");
                    ma.MapRightKey("PersonId");
                    ma.ToTable("MovieActor");
                });

            modelBuilder.Entity<Person>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.DirectedMovies)
                .WithRequired(m => m.Director)
                .HasForeignKey(m => m.DirectorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}