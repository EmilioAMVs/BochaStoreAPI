using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BochaStoreAPI.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int PropietarioId { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public int Stock { get; set; }


        // Relaciones
        public Usuario? Propietario { get; set; }

        [JsonIgnore]
        public ICollection<Marca>? Marcas { get; set; }
        public ICollection<Categoria>? Categorias { get; set; }
        public ICollection<ImagenProducto>? Imagenes { get; set; }
    }


}
