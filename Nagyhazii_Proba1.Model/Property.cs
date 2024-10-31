using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Nagyhazii_Proba1.Model
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Range(1, 23)]
        [Required]
        public int District { get; set; }

        [Range(1, 20)]
        [Required]
        public int Rooms { get; set; }

        [Range(0.0, double.MaxValue)]
        [Required]
        public double Area { get; set; }

        [Range(0.0, (double)decimal.MaxValue)]
        public decimal? SellingPrice { get; set; }

        [Range(0.0, (double)decimal.MaxValue)]
        public decimal? RentPrice { get; set; }

        // Kapcsolat a PropertyOwner entitással
        public ICollection<PropertyOwner> PropertyOwners { get; set; } = new List<PropertyOwner>();
    }
}
