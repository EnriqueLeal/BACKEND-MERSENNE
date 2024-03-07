
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Transactions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int PropertyID { get; set; }

        [ForeignKey("Buyer")]
        public int BuyerID { get; set; }

        [ForeignKey("Seller")]
        public int SellerID { get; set; }

        [Required(ErrorMessage = "El precio es necesario.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public DateTime? TransactionDate { get; set; }

        // Navigation properties
        public virtual Properties Property { get; set; }

        public virtual Users Buyer { get; set; }

        public virtual Users Seller { get; set; }
    }
}
