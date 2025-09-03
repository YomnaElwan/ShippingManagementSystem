using Microsoft.EntityFrameworkCore;
using ShippingSystem.Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Regions
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter region name !")]
        [UniqueRegion]
        [MinLength(4)]
        [MaxLength(20)]
        public string Name { get; set; }
        public List<Governorates>?  Governorates { get; set; }
    }
}
