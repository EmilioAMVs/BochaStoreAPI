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
    public class CategoriaController : Controller
    {
        private readonly ProductoDbContext _db;
        private readonly IConfiguration _configuration;

    public CategoriaController(ProductoDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerCategorias()
    {

        // Busca el ID del usuario actual en los claims
        var claimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Si el ID no existe o está vacío se devuelve "Unauthorized"
        if (string.IsNullOrEmpty(claimUserId)) return Unauthorized();

        int userId;

        // Intenta convertir el ID del usuario a un número entero, si falla, devuelve un BadRequest
        if (!int.TryParse(claimUserId, out userId)) return BadRequest("Id de usuario inválido.");

        // Busca todas las categorias
        List<Categoria> categorias= await _db.Categorias
            //.Include(categorias=> categorias.IdCategoria)
            .ToListAsync();

        return Ok(categorias);

    }

   

    // Obtiene una categoria específica por su ID.
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerCategoria(int id)
    {

        // Busca una categoria con el ID específico
        var categoriaEncontrado = await _db.Categorias
            .FirstOrDefaultAsync(l => l.IdCategoria == id);

        // Si encuentra la categoria, lo devuelve como respuesta válida
        if (categoriaEncontrado != null) return Ok(categoriaEncontrado);

        // Si no encuentra la categoria, devuelve "NotFound"
        return NotFound();

    }

    // Crea una nueva categoria.
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
    {

        // Agrega una nuevo categoria a la base de datos
        await _db.Categorias.AddAsync(categoria);

        // Guarda los cambios en la base de datos
        await _db.SaveChangesAsync();

        return Ok(categoria);

    }

    // Edita los datos de una categoria existente.
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditarCategoria([FromBody] Categoria categoria)
    {

        // Busca una categoria en la base de datos por su ID
        var categoriaExistente = await _db.Categorias.FirstOrDefaultAsync(x => x.IdCategoria == categoria.IdCategoria);
        if (categoriaExistente == null) return NotFound();

            // Actualiza los datos de la categoria con los datos nuevos
        categoriaExistente.Nombre = categoria.Nombre != null ? categoria.Nombre : categoriaExistente.Nombre;

        await _db.SaveChangesAsync();

        return NoContent();
    }



    // Elimina una categoria por su ID.
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Eliminarcategoria(int id)
    {

        // Busca una categoria en la base de datos por su ID
        var categoriaExistente = await _db.Categorias
            .FirstOrDefaultAsync(l => l.IdCategoria == id);

        // Si no encuentra la categoria, devuelve NotFound
        if (categoriaExistente == null) return NotFound();


        // Elimina la categoria de la base de datos
        _db.Categorias.Remove(categoriaExistente);

        await _db.SaveChangesAsync();

        // Devuelve un mensaje de éxito
        return Ok("Categoria eliminada con éxito.");
    }


}

}
