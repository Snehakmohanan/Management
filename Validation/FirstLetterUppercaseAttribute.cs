using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Librarymanagement.Validation
{
    public class FirstLetterUppercaseAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value==null|| string.IsNullOrEmpty(value.ToString()))
            {
           return ValidationResult.Success;
            }
            var firstLetter=value.ToString()[0].ToString();
            
            if(firstLetter !=firstLetter.ToUpper())
            {
            return new ValidationResult("First Letter Should be Uppercase");
            }
            return ValidationResult.Success;
            
        }
    }
}