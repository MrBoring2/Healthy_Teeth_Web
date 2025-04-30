using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validations
{
    public class TimeNotInFutureAttribute : ValidationAttribute
    {
        public TimeNotInFutureAttribute()
        {
            // Set a default error message to be displayed when validation fails.
            ErrorMessage = "Дата не может быть больше текущей и низе 1970 года.";
        }

        // Overrides the IsValid method to implement custom validation logic.
        // value: The value of the property being validated
        // validationContext: The Context information about the validation operation
        // ValidationResult indicating whether validation succeeded or failed
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // If the value is null, assume that another attribute (e.g., [Required]) handles it.
            if (value == null)
                return ValidationResult.Success;

            // Check if the value is of type DateTime.
            if (value is TimeOnly dateValue)
            {
                // Compare the provided date with the current date and time.
                if (dateValue > TimeOnly.FromTimeSpan(DateTime.Now.TimeOfDay))
                {
                    // If the date is in the future, return a validation error with the specified message.
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                // If the value is not a DateTime, return a validation error indicating improper usage.
                return new ValidationResult("Данные не являются датой.");
            }

            // If all checks pass, return success.
            return ValidationResult.Success;
        }
    }
}
