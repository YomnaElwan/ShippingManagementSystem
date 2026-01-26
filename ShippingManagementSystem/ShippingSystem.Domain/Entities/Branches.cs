using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Branches
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You Must Enter Branch Name")]
        [MaxLength(30)]
        [MinLength(5)]
        public string Name { get; set; }
        [Required(ErrorMessage ="You Must Enter Branch Location")]
        [MaxLength(100)]
        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; }=DateTime.UtcNow.AddHours(2);
        [NotMapped]
        public List<Couriers>? Couriers { get; set; }
        [NotMapped]
        public List<Employees>? Employees { get; set; }
        [NotMapped]
        public List<Orders>? Orders { get; set; }
    }
}
