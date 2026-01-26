using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Common
{
    public class UniquenessChecker
    {
        public static bool CheckDuplication<T>(
            IEnumerable<T> items,
            Func<T,string> nameSelector,
            string inputName,
            Func<T,bool>? extraCondition=null
            ) {
            return items.Any(item => NormalizeName.Normalize(nameSelector(item)) == NormalizeName.Normalize(inputName) &&
                            (extraCondition==null || extraCondition(item))
            );
        
        }
       
       
    }
}
