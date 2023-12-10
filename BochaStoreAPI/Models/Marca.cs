using System.ComponentModel.DataAnnotations;

namespace BochaStoreAPI.Models
{
    public class Marca
    {
        [Key]
        public int IdMarca { get; set; }
        [Required]
        public string Nombre { get; set; }


        // Relaciones
        public Producto? Producto{ get; set; }
    }
}
