using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FastNotesAPI.Models.Notas
{
    public partial class progmovilContext : DbContext
    {
        public progmovilContext()
        {
        }

        public progmovilContext(DbContextOptions<progmovilContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fastnotesapi> Fastnotesapi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4");

            modelBuilder.Entity<Fastnotesapi>(entity =>
            {
                entity.ToTable("fastnotesapi");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Titulo).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
