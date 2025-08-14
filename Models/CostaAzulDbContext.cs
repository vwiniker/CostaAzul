using CostaAzul.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CostaAzul.API.Models
{
    public class CostaAzulContext : DbContext
    {
        public CostaAzulContext(DbContextOptions<CostaAzulContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }

        public virtual DbSet<Facturacion> Facturaciones { get; set; }

        public virtual DbSet<Habitacion> Habitaciones { get; set; }

        public virtual DbSet<Hotel> Hoteles { get; set; }

        public virtual DbSet<MetodoPago> MetodosPagos { get; set; }

        public virtual DbSet<Opinion> Opiniones { get; set; }

        public virtual DbSet<Pago> Pagos { get; set; }

        public virtual DbSet<Reservacion> Reservaciones { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        public virtual DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Hotel>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Habitacion>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Opinion>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<MetodoPago>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Facturacion>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<DetalleFactura>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Pago>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Reservacion>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Roles>()
               .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Habitacion>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Hotel>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Pago>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<DetalleFactura>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Roles>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Reservacion>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<MetodoPago>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Opinion>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY
            modelBuilder.Entity<Facturacion>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Configuración de IDENTITY

            // Relación Usuario - Roles
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Roles)
                .WithMany()
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Facturación - Reservación
            modelBuilder.Entity<Facturacion>()
                .HasOne(f => f.Reservacion)
                .WithMany(r => r.Facturaciones)
                .HasForeignKey(f => f.ReservacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Facturación - DetalleFactura
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Facturacion)
                .WithMany(f => f.DetalleFacturas)
                .HasForeignKey(df => df.FacturacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Pago - Reservación
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reservacion)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ReservacionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Pago - MetodoPago
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.MetodoPago)
                .WithMany()
                .HasForeignKey(p => p.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Opinión - Usuario
            modelBuilder.Entity<Opinion>()
                .HasOne(o => o.Usuario)
                .WithMany(u => u.Opiniones)
                .HasForeignKey(o => o.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Opinión - Hotel
            modelBuilder.Entity<Opinion>()
                .HasOne(o => o.Hotel)
                .WithMany(h => h.Opiniones)
                .HasForeignKey(o => o.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Reservación - Habitacion
            modelBuilder.Entity<Reservacion>()
                .HasOne(r => r.Habitacion)
                .WithMany(h => h.Reservaciones)
                .HasForeignKey(r => r.HabitacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Reservación - Usuario
            modelBuilder.Entity<Reservacion>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservaciones)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reservacion)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ReservacionId)
                .OnDelete(DeleteBehavior.Restrict); // Evita eliminar pagos al eliminar reservación

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.MetodoPago)
                .WithMany()
                .HasForeignKey(p => p.MetodoPagoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita eliminar el método de pago si hay pagos asociados

            modelBuilder.Entity<Facturacion>()
                .HasMany(f => f.Pagos)
                .WithOne(p => p.Facturacion)
                .HasForeignKey(p => p.FacturacionId)
                .OnDelete(DeleteBehavior.Cascade); // Eliminar los pagos al eliminar la factura

            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Facturacion)
                .WithMany(f => f.DetalleFacturas)
                .HasForeignKey(df => df.FacturacionId)
                .OnDelete(DeleteBehavior.Cascade); // Eliminar los detalles al eliminar la factura


            modelBuilder.Entity<Usuario>()
                    .Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Pago>()
                .Property(p => p.FechaPago)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Facturacion>()
                .Property(f => f.Fecha)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
