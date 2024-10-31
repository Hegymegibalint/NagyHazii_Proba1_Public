using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nagyhazii_Proba1.Model
{
    public class Contract
    {
        [Key]
        public int Id { get; set; } // Szerződés egyedi azonosítója

        [Required]
        public int PropertyId { get; set; } // Az ingatlan azonosítója

        [ForeignKey("PropertyId")]
        public Property Property { get; set; } // Kapcsolat az ingatlannal

        [Required]
        public int SellerId { get; set; } // Eladó azonosítója

        [ForeignKey("SellerId")]
        public Customer Seller { get; set; } // Kapcsolat az eladóval

        [Required]
        public int BuyerId { get; set; } // Vevő/bérlő azonosítója

        [ForeignKey("BuyerId")]
        public Customer Buyer { get; set; } // Kapcsolat a vevővel vagy bérlővel

        [Required]
        [Range(0.0, (double)decimal.MaxValue)]
        public decimal Price { get; set; } // Szerződés ára

        [Required]
        public DateTime SignDate { get; set; } // Szerződéskötés dátuma

        public DateTime? ContractExpiration { get; set; } // Bérleti szerződés lejáratának dátuma (opcionális)

        // Navigation properties: kapcsolatok más osztályokkal
    }
}
