using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace API.Models
{
    public class Mersenne
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Es necesario la cantidad")]
        [Column(TypeName = "float")]
        public double Cantidad { get; set; }

        [Required(ErrorMessage= "Es necesario introduccir si esta activo")]
        public bool Activo { get; set; }

    }
    //public class MersenneRestult
    //{
    //    public int Id { get; set; }

    //    public int Cantidad { get; set; }

    //    public int Activo { get; set; }
    //}
}
