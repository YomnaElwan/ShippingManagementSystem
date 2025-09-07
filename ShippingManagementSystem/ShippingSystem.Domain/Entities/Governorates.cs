using ShippingSystem.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Governorates
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You must enter governorate name!")]
        [UniqueGov]
        [MinLength(4)]
        [MaxLength(15)]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Cities>? Cities { get; set; }
        public List<Couriers>? Couriers { get; set; }
        public List<Orders>? Orders { get; set; }
        [ForeignKey("Region")]
        [Required(ErrorMessage = "You must choose a region!")]

        public int RegionId { get; set; }
        public Regions Region { get; set; }
    }
}
