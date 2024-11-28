using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validations
{
    public class ChangePasswordAttribute : ValidationAttribute
    {
        private bool _changePassword;
        public ChangePasswordAttribute(bool changePassword)
        {
            // Set a default error message to be displayed when validation fails.
            ErrorMessage = "Поле пароль должно быть заполнено и быть не больше 20 символов.";
            _changePassword = changePassword;
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
            if (value is string str)
            {
                // Compare the provided date with the current date and time.
                if (_changePassword && str.Length > 20)
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
