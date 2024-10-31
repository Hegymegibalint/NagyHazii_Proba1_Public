using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagyhazii_Proba1.Model
{
    public class PropertyOwner
    {
        [Key]
        public int Id { get; set; } // Egyedi azonosító a kapcsolathoz

        [Required]
        public int PropertyId { get; set; } // Ingatlan azonosítója

        [ForeignKey("PropertyId")]
        public Property Property { get; set; } // Kapcsolat az ingatlannal

        [Required]
        public int CustomerId { get; set; } // Ügyfél azonosítója

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } // Kapcsolat az ügyféllel
    }
}
