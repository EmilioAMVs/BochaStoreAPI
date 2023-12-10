using System.ComponentModel.DataAnnotations;

namespace BochaStoreAPI.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; } 
        [Required]
        public string Username { get; set; } 
        [Required]
        public string Password { get; set; } 
        [Required]
        public string Nombre { get; set; } 
        [Required]
        public string Email { get; set; } 

        public ICollection<Producto> Productos { get; set; }

    }
}
