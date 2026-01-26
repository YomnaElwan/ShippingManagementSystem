using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Common.Exceptions
{
    public class DuplicateEntityException:Exception
    {
        public DuplicateEntityException(string entityName, string fieldName, string value)
            :base($"{entityName} with {fieldName} '{value}' is already existing!")
        {
            
        }
    }
}
