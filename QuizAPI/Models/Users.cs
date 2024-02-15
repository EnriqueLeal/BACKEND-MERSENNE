using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Serialization;

namespace API.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es necesario.")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria.")]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo electrónico es necesario.")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es necesario.")]
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Buyer,
        Seller,
        Agent
    }
}
