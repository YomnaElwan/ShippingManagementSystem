using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Common
{
    public class NormalizeName
    {
        public static string Normalize(string name)
        {
            return name.ToLower().Replace(" ", "").Trim();
        }
    }
}
