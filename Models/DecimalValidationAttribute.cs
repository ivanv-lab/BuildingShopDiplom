using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingShop.Models
{
    public class DecimalValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value == DBNull.Value)
            {
                return ValidationResult.Success;
            }

            decimal decimalValue;
            if (!decimal.TryParse(value.ToString(), out decimalValue))
            {
                return new ValidationResult("Введите корректное число.");
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Введите корректное число для поля {name}.";
        }
    }
}