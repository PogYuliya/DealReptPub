using System;
using System.ComponentModel.DataAnnotations;

namespace DealRept.Models
{
    public class DateGreaterTodayAttributes:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (DateTime.TryParse(value.ToString(), out DateTime conclusionDate))
            {
                if (conclusionDate.Date<=DateTime.UtcNow.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Contract Date of Conclusion must be before or equale today`s date.");
                }
            }

            return new ValidationResult("Allowed: only dates.");
        }

    }
}
