using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FastNotesAPI.Models
{
    public partial class itesrcne_181g0250Context : DbContext
    {
        public itesrcne_181g0250Context()
        {
        }

        public itesrcne_181g0250Context(DbContextOptions<itesrcne_181g0250Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Fastnotesapi> Fastnotesapi { get; set; }
        public virtual DbSet<ProductoT> ProductoT { get; set; }
        public virtual DbSet<PuntuacionT> PuntuacionT { get; set; }
        public virtual DbSet<UsersT> UsersT { get; set; }
        public virtual DbSet<VideogameT> VideogameT { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=204.93.216.11;database=itesrcne_181g0250;user=itesrcne_jean;password=181G0250", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Fastnotesapi>(entity =>
            {
                entity.ToTable("fastnotesapi");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Eliminado).HasColumnType("bit(1)");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Titulo).HasMaxLength(45);
            });

            modelBuilder.Entity<ProductoT>(entity =>
            {
                entity.ToTable("producto_t");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.NombreProducto)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("nombre_producto");

                entity.Property(e => e.PrecioProducto)
                    .HasPrecision(5, 2)
                    .HasColumnName("precio_producto");

                entity.Property(e => e.TipoProducto)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("tipo_producto")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<PuntuacionT>(entity =>
            {
                entity.ToTable("puntuacion_t");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("nombre");

                entity.Property(e => e.Puntos)
                    .HasColumnType("int(11)")
                    .HasColumnName("puntos");
            });

            modelBuilder.Entity<UsersT>(entity =>
            {
                entity.ToTable("users_t");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("nombre_Usuario");

                entity.Property(e => e.PasswordUsuario)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("password_Usuario");
            });

            modelBuilder.Entity<VideogameT>(entity =>
            {
                entity.ToTable("videogame_t");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DescripcionVg)
                    .HasMaxLength(180)
                    .HasColumnName("descripcion_vg");

                entity.Property(e => e.FechaSalidaVg)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaSalida_vg");

                entity.Property(e => e.NombreVg)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("nombre_vg");

                entity.Property(e => e.PortadaVg)
                    .HasMaxLength(300)
                    .HasColumnName("portada_vg");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
