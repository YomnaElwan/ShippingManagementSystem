using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
        


    }
}
