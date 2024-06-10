using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Attributes
{
    public class ValidSubjectNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;

            if (string.IsNullOrEmpty(name))
            {
                return new ValidationResult("Name is required.");
            }

            if (name.Any(char.IsDigit))
            {
                return new ValidationResult("Name cannot contain digits.");
            }

            if (char.IsLower(name[0]))
            {
                return new ValidationResult("Name cannot start with a lowercase letter.");
            }

            return ValidationResult.Success;
        }
    }
}
