using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Models;

namespace TodoApp.Infrastructure.Data
{
    /// <summary>
    /// DbContext principal de la aplicación, que representa la sesión con la base de datos y permite realizar operaciones CRUD sobre las entidades del dominio.
    /// Define todas las tablas y relaciones necesarias para la persistencia de los modelos de dominio.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //DbSets (Tablas)
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Importancia> Importancias { get; set; }

        ///<summary>
        ///Configuracion de modelo (relaciones, constraints, etc.)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //================CONFIGURACION DE ESTADOS=========================
            modelBuilder.Entity<Estado>().HasKey(e => e.Id);

            modelBuilder.Entity<Estado>().Property(e => e.Nombre).IsRequired()
                .HasMaxLength(50);

            //Seed: Datos iniciales
            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Nombre = "Pendiente" },
                new Estado { Id = 2, Nombre = "En Progreso" },
                new Estado { Id = 3, Nombre = "Completada" }
            );

            //================CONFIGURACION DE IMPORTANCIAS=========================
            modelBuilder.Entity<Importancia>().HasKey(i => i.Id);

            modelBuilder.Entity<Importancia>().Property(i => i.Nombre).IsRequired()
                .HasMaxLength(50);

            //Seed: Datos iniciales
            modelBuilder.Entity<Importancia>().HasData(
                new Importancia { Id = 1, Nombre = "Baja" },
                new Importancia { Id = 2, Nombre = "Media" },
                new Importancia { Id = 3, Nombre = "Alta" }
            );

            //================CONFIGURACION DE TAREAS=========================
            modelBuilder.Entity<Tarea>().HasKey(t => t.Id);

            modelBuilder.Entity<Tarea>().Property(t => t.Titulo).IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Tarea>().Property(t => t.Descripcion).HasMaxLength(5000);

            //Validacion a nivel de DB: Titulo minimo 5 caracteres
            modelBuilder.Entity<Tarea>().ToTable("Tareas", t => t.HasCheckConstraint("CK_Titulo_MinLength", "LEN(Titulo) >= 5"));

            //Relacion: Tarea - Estado
            modelBuilder.Entity<Tarea>().HasOne(t => t.Estado)
                .WithMany()
                .HasForeignKey(t => t.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            //Relacion: Tarea - importancia
            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Importancia)
                .WithMany()
                .HasForeignKey(t => t.IdImportancia)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Tarea → Subtareas (autorrelación para Fase 2)
            modelBuilder.Entity<Tarea>()
                .HasMany(t => t.subtarea)
                .WithOne()
                .HasForeignKey(t => t.IdTareaPadre)
                .OnDelete(DeleteBehavior.NoAction);  // Cambio: NoAction en lugar de Cascade

            // Índices para optimizar queries
            modelBuilder.Entity<Tarea>()
                .HasIndex(t => t.IdEstado)
                .HasDatabaseName("IX_Tarea_Estado");

            modelBuilder.Entity<Tarea>()
                .HasIndex(t => t.IdImportancia)
                .HasDatabaseName("IX_Tarea_Importancia");

            modelBuilder.Entity<Tarea>()
                .HasIndex(t => t.FechaLimite)
                .HasDatabaseName("IX_Tarea_FechaLimite");
        }
    }
}
