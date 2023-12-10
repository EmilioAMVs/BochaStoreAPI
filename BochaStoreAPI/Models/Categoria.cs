using System.ComponentModel.DataAnnotations;

namespace BochaStoreAPI.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required]
        public string Nombre { get; set; }

        // Relaciones
        public Producto? Producto{ get; set; }
    }
}
