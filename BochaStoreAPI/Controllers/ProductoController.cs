using BochaStoreAPI.Data;
using BochaStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace BochaStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly ProductoDbContext _db;
        private readonly IConfiguration _configuration;

    public ProductoController(ProductoDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    //Obtiene todos los productos
    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {

        // Busca el ID del usuario actual en los claims
        var claimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Si el ID no existe o está vacío se devuelve "Unauthorized"
        if (string.IsNullOrEmpty(claimUserId)) return Unauthorized();

        int userId;

        // Intenta convertir el ID del usuario a un número entero, si falla, devuelve un BadRequest
        if (!int.TryParse(claimUserId, out userId)) return BadRequest("Id de usuario inválido.");

        // Busca todos los productos
        List<Producto> productos = await _db.Productos
            .Include(productos=> productos.Imagenes)
            .ToListAsync();

        return Ok(productos);

    }

    // Obtiene los productos que son del usuario actual.
    [HttpGet("Cliente")]
    [Authorize]
    public async Task<IActionResult> ObtenerProductosCliente()
    {

        // Busca el ID del usuario actual en los claims
        var claimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Si el ID no existe o está vacío, devuelve un Unauthorized
        if (string.IsNullOrEmpty(claimUserId)) return Unauthorized();

        int userId;

        // Intenta convertir el ID del usuario a un número entero, si falla, devuelve BadRequest
        if (!int.TryParse(claimUserId, out userId)) return BadRequest("Id de usuario inválido.");

        // Ahora busca todos los productos que son propiedad del usuario actual
        List<Producto> productos = await _db.Productos
            .Where(productos => productos.PropietarioId == userId)
            .Include(productos => productos.Imagenes)
            .ToListAsync();

        return Ok(productos);

    }

    // Obtiene un producto específico por su ID.
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerProducto(int id)
    {

        var productoEncontrado = await _db.Productos
            .Include(l => l.Marcas)
            .Include(l => l.Categorias)
            .Include(l => l.Imagenes)
            .FirstOrDefaultAsync(l => l.IdProducto == id);

        if (productoEncontrado != null) return Ok(productoEncontrado);

        return NotFound();

    }

    // Crea un nuevo producto
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearProducto([FromBody] Producto producto)
    {

        // Agrega un nuevo producto a la base de datos
        await _db.Productos.AddAsync(producto);

        // Guarda los cambios en la base de datos
        await _db.SaveChangesAsync();

        return Ok(producto);

    }

    // Edita los datos de un producto existente.
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditarProducto([FromBody] Producto producto)
    {

        // Busca un producto por su ID
        var productoExistente = await _db.Productos.FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);
        if (productoExistente == null) return NotFound();

            // Actualiza los datos del producto con los datos nuevos
        productoExistente.Nombre = producto.Nombre != null ? producto.Nombre : productoExistente.Nombre;
        productoExistente.Descripcion = producto.Descripcion != null ? producto.Descripcion : productoExistente.Descripcion;
        productoExistente.Precio = producto.Precio!= 0.0 ? producto.Precio : productoExistente.Precio;
        productoExistente.Stock = producto.Stock!= 0 ? producto.Stock: productoExistente.Stock;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // Añade categorias al producto
    [HttpPost("AddCategoria/{productoId}")]
    [Authorize]
    public async Task<IActionResult> AddCategorias(int productoId, [FromBody] List<Categoria> categorias)
    {

        // Busca un producto por su ID
        var productoExistente = await _db.Productos
            .Include(l => l.Categorias)
            .FirstOrDefaultAsync(l => l.IdProducto == productoId);

        // Si no encuentra el local, devuelve NotFound
        if (productoExistente == null) return NotFound();

        // Agrega la catgoria al producto
        foreach (var categoria in categorias)
        {
            categoria.Producto = productoExistente;
            await _db.Categorias.AddAsync(categoria);
            productoExistente.Categorias.Add(categoria);
        }

        await _db.SaveChangesAsync();

        return Ok();
    }

        // Añade marca a los productos
        [HttpPost("AddMarca/{productoId}")]
        [Authorize]
        public async Task<IActionResult> AddMarcas(int productoId, [FromBody] List<Marca> marcas)
        {

            // Busca un producto por su ID
            var productoExistente = await _db.Productos
                .Include(l => l.Marcas)
                .FirstOrDefaultAsync(l => l.IdProducto == productoId);

            // Si no encuentra el local, devuelve NotFound
            if (productoExistente == null) return NotFound();

            // Agrega la marca al producto
            foreach (var marca in marcas)
            {
                marca.Producto = productoExistente;
                await _db.Marcas.AddAsync(marca);
                productoExistente.Marcas.Add(marca);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        // Añade imágenes al producto.
        [HttpPost("AddImagenes/{productoId}")]
    [Authorize]
    public async Task<IActionResult> AddImagenesProducto(int productoId, [FromBody] List<ImagenProducto> imagenes)
    {
        // Busca un producto por su ID
        var productoExistente = await _db.Productos
            .Include(l => l.Imagenes)
            .FirstOrDefaultAsync(l => l.IdProducto == productoId);

        // Si no encuentra el producto, devuelve NotFound
        if (productoExistente == null) return NotFound();

        // Agrega las imágenes al producto
        foreach (var imagen in imagenes)
        {
            imagen.Producto = productoExistente;
            await _db.ImagenesProducto.AddAsync(imagen);
            productoExistente.Imagenes.Add(imagen);
        }

        await _db.SaveChangesAsync();

        return Ok();

    }

    // Edita la imagen del producto.
    [HttpPut("Imagenes/Edit/{productoId}")]
    [Authorize]
    public async Task<IActionResult> EditarImagenProducto(int productoId, [FromBody] List<ImagenProducto> imagenesNuevas)
    {

        // Busca un producto por su ID
        var productoExistente = await _db.Productos
            .Include(l => l.Imagenes)
            .FirstOrDefaultAsync(l => l.IdProducto == productoId);

        // Si no encuentra el producto, devuelve NotFound
        if (productoExistente == null) return NotFound();

        // Elimina las imágenes anteriores del producto
        _db.ImagenesProducto.RemoveRange(productoExistente.Imagenes);

        // Agrega las imágenes nuevas al producto
        foreach (var imagen in imagenesNuevas)
        {
            imagen.Producto = productoExistente;
            await _db.ImagenesProducto.AddAsync(imagen);
        }

        await _db.SaveChangesAsync();

        return Ok();
    }

    // Elimina un producto por su ID.
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarProducto(int id)
    {

        // Busca un producto por su ID
        var productoExistente = await _db.Productos
            .FirstOrDefaultAsync(l => l.IdProducto == id);

        // Si no encuentra el producto, devuelve NotFound
        if (productoExistente == null) return NotFound();


        // Elimina el producto de la base de datos
        _db.Productos.Remove(productoExistente);

        await _db.SaveChangesAsync();

        // Devuelve un mensaje de éxito
        return Ok("Local eliminado con éxito.");
    }


}

}
