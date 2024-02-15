using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class RealEstateAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgentID { get; set; }

        [Required(ErrorMessage = "El nombre es necesario.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El correo electrónico es necesario.")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; }
    }
}
