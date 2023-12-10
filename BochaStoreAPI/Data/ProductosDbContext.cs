using BochaStoreAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BochaStoreAPI.Data
{
    public class ProductoDbContext : DbContext
    {
        public ProductoDbContext(DbContextOptions<ProductoDbContext> options) : base(options)
        { }

        //Tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<ImagenProducto> ImagenesProducto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    IdUsuario = 1,
                    Username = "admin",
                    Password = "admin123",
                    Nombre = "Admin Bocha",
                    Email = "admin@bocha.com"
                    
                }
            );

            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    IdProducto = 1,  
                    PropietarioId = 1,  // Puedes usar el ID del usuario 'admin' que ya está definido
                    Nombre = "Coca Cola",  
                    Descripcion = "Bebida Gaseosa",  
                    Stock = 50  
                }
            );

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria 
                { 
                    IdCategoria=1,
                    Nombre="Camisetas"
                }
                );

            modelBuilder.Entity<Marca>().HasData(
               new Marca
               {
                   IdMarca = 1,
                   Nombre = "Adidas"
               }
               );


            // Relación entre Producto y Usuario
            modelBuilder.Entity<Producto>()
            .HasOne(l => l.Propietario)
            .WithMany(u => u.Productos)  // Aquí especificas la relación inversa
            .HasForeignKey(l => l.PropietarioId)
            .OnDelete(DeleteBehavior.Restrict);


        }
    }
}