using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
       
        [MaxLength(15)]
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
