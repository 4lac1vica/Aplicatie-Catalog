using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AplicatieCatalog.Models;

namespace AplicatieCatalog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Profesor> Profesori { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Materie> Materii { get; set; }
        public DbSet<Nota> Grades { get; set; }

        public DbSet<Absenta> Absente { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Nota>()
                .HasOne(n => n.Student)
                .WithMany(s => s.Note)
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Nota>()
                .HasOne(n => n.Profesor)
                .WithMany()
                .HasForeignKey(n => n.ProfesorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Nota>()
                .HasOne(n => n.Materie)
                .WithMany(m => m.Note)
                .HasForeignKey(n => n.MaterieId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Profesor>()
                .HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Absenta>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Absente)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Absenta>()
                .HasOne(a => a.Profesor)
                .WithMany()
                .HasForeignKey(a => a.ProfesorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Absenta>()
                .HasOne(a => a.Materie)
                .WithMany(m => m.Absente)
                .HasForeignKey(a => a.MaterieId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}