using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BochaStoreAPI.Models
{
    public class ImagenProducto
    {
        
            [Key]
            public int IdImagen { get; set; }
            [Required]
            public string Url { get; set; } 
            [Required]
            public int ProductoId { get; set; } 

            // Relación
            [JsonIgnore]
            public Producto? Producto{ get; set; }
    }
}
