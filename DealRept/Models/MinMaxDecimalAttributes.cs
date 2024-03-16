using System.ComponentModel.DataAnnotations;

namespace DealRept.Models
{
    public class MinMaxDecimalAttributes:ValidationAttribute
    {
        private readonly decimal _minValue;
        private readonly decimal _maxValue;

        public MinMaxDecimalAttributes(int minValue,
            int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (decimal.TryParse(value.ToString(),out decimal amount))
            {
                if (amount>_minValue&&amount<_maxValue)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"Allowed range: {_minValue+1} - {_maxValue-1}.");
                }
            }

            return new ValidationResult("Allowed: only decimal.");
        }
    }
}
