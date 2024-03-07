using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Properties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyID { get; set; }

        [Required(ErrorMessage = "El título es necesario.")]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "El precio es necesario.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "La latitud es necesaria.")]
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "La longitud es necesaria.")]
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Longitude { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public int? Size { get; set; }

        public int UserID { get; set; }

        // Navigation property
        public virtual Users User { get; set; }
    }
}
