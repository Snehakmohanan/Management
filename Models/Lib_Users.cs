using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Librarymanagement.Validation;

namespace Librarymanagement.Models
{
    public class Lib_Users
    {
        public int LoginId { get; set; }

        [Required(ErrorMessage = "The Field {0} is required")]
        // [StringLength(50)]
        [FirstLetterUppercase]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Field {0} is required")]
        // [StringLength(50)]

        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "The Field {0} is required")]
        public string Password { get; set; } = string.Empty;
        public string EncryptPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Field {0} is required")]
        public DateTime CreateDate { get; set; }
        [Required(ErrorMessage = "The Field {0} is required")]
        public int Status { get; set; }
         public DateTime LoginDate { get; set; }

         public int LoginCount { get; set; }

        




        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("First letter should be uppercase", new string[] { nameof(Name) });
                }
            }
        }
    }
}