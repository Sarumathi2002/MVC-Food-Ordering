using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foodordering.Common{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;
        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain=allowedDomain;
        }
        
        public override bool IsValid(object? value)
        {
           string[] domain= value.ToString().Split('@');
           return domain[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}