using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Common.Exceptions
{
    public class EntityNotFoundException:Exception
    {
        public EntityNotFoundException(string entityName,object key)
            :base($"{entityName} with key ({key}) was not found!")
        {
            
        }
    }
}
