using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nagyhazii_Proba1.Model
{
    public class Customer
    {
        [Key]
        [XmlElement("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [XmlElement("Name")]
        public string Name { get; set; }

        // Kapcsolat a PropertyOwner entitással
        [XmlIgnore]
        public ICollection<PropertyOwner> OwnedProperties { get; set; } = new List<PropertyOwner>();

        [Required]
        [Range(1, 20)]
        [XmlElement("MinRooms")]
        public int MinRooms { get; set; }

        [Required]
        [Range(1, 20)]
        [XmlElement("MaxRooms")]
        public int MaxRooms { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        [XmlElement("MinArea")]
        public double MinArea { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        [XmlElement("MaxArea")]
        public double MaxArea { get; set; }

        [Range(0.0, (double)decimal.MaxValue)]
        [XmlElement("MinPrice")]
        public decimal? MinPrice { get; set; }

        [Range(0.0, (double)decimal.MaxValue)]
        [XmlElement("MaxPrice")]
        public decimal? MaxPrice { get; set; }

        [XmlElement("LookingForRent")]
        public bool LookingForRent { get; set; }

        [XmlElement("LookingForPurchase")]
        public bool LookingForPurchase { get; set; }

        [XmlArray("DistrictPreferences")]
        [XmlArrayItem("District")]
        public ICollection<int> DistrictPreferences { get; set; } = new List<int>();

        /// <summary>
        /// Ellenőrzi, hogy a Customer adatai érvényesek-e.
        /// </summary>
        /// <returns>True, ha az adatok érvényesek, különben False.</returns>
        public bool IsValid()
        {
            // Ellenőrizd, hogy a MinRooms nem nagyobb-e, mint a MaxRooms
            if (MinRooms > MaxRooms)
            {
                return false;
            }

            // Ellenőrizd, hogy a MinArea nem nagyobb-e, mint a MaxArea
            if (MinArea > MaxArea)
            {
                return false;
            }

            // Ellenőrizd, hogy a MinPrice nem nagyobb-e, mint a MaxPrice (ha mindkettő meg van adva)
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ellenőrzi, hogy a vásárlási és bérlési preferenciák nem ütköznek-e.
        /// </summary>
        /// <returns>True, ha csak az egyik preferencia van megadva vagy egyik sem, különben False.</returns>
        public bool IsPreferenceValid()
        {
            // Csak az egyik preferencia lehet igaz, vagy egyik sem.
            return !(LookingForRent && LookingForPurchase);
        }
    }
}
