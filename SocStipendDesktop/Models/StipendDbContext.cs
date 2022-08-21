using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocStipendDesktop.Models
{
    public class StipendDbContext : DbContext
    {
        public DbSet<Stipend> Stipends { get; set; }
        public DbSet<Student> Students { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=SocStipendDB.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Stipend>().ToTable("Stipend", "test");
            modelBuilder.Entity<Stipend>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Student>().ToTable("Student", "test");
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
