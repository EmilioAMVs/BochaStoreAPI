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
    public class MarcaController : Controller
    {
        private readonly ProductoDbContext _db;
        private readonly IConfiguration _configuration;
    public MarcaController(ProductoDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerMarcas()
    {
        // Busca el ID del usuario actual en los claims
        var claimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Si el ID no existe o está vacío se devuelve "Unauthorized"
        if (string.IsNullOrEmpty(claimUserId)) return Unauthorized();

        int userId;

        // Intenta convertir el ID del usuario a un número entero, si falla, devuelve un BadRequest
        if (!int.TryParse(claimUserId, out userId)) return BadRequest("Id de usuario inválido.");

        // Busca todas las categorias
        List<Marca> marcas= await _db.Marcas
            //.Include(marcas=> marcas.IdMarca)
            .ToListAsync();

        return Ok(marcas);
    }

  
    // Obtiene una categoria específica por su ID.
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerMarca(int id)
    {
        // Busca una marca con el ID específico
        var marcaEncontrado = await _db.Marcas
            .FirstOrDefaultAsync(l => l.IdMarca == id);

        // Si encuentra la marca, lo devuelve como respuesta válida
        if (marcaEncontrado != null) return Ok(marcaEncontrado);

        // Si no encuentra la marca, devuelve "NotFound"
        return NotFound();
    }

    // Crea una nueva marca.
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearMarca([FromBody] Marca marca)
    {

        // Agrega una nuevo marca a la base de datos
        await _db.Marcas.AddAsync(marca);

        // Guarda los cambios en la base de datos
        await _db.SaveChangesAsync();

        return Ok(marca);

    }

    // Edita los datos de una marca existente.
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditarMarca([FromBody] Marca marca)
    {

        // Busca una marca en la base de datos por su ID
        var marcaExistente = await _db.Marcas.FirstOrDefaultAsync(x => x.IdMarca == marca.IdMarca);
        if (marcaExistente == null) return NotFound();

        // Actualiza los datos de la marca con los datos nuevos
        marcaExistente.Nombre = marca.Nombre != null ? marca.Nombre : marcaExistente.Nombre;

        await _db.SaveChangesAsync();

        return NoContent();
    }



    // Elimina una marca por su ID.
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarMarca(int id)
    {

        // Busca una marca en la base de datos por su ID
        var marcaExistente = await _db.Marcas
            .FirstOrDefaultAsync(l => l.IdMarca == id);

        // Si no encuentra la marca, devuelve NotFound
        if (marcaExistente == null) return NotFound();

        // Elimina la marca de la base de datos
        _db.Marcas.Remove(marcaExistente);

        await _db.SaveChangesAsync();

        // Devuelve un mensaje de éxito
        return Ok("Marca eliminada con éxito.");
    }


}

}
