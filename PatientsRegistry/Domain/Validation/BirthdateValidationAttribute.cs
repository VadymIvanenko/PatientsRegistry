using System;
using System.ComponentModel.DataAnnotations;

namespace PatientsRegistry.Domain.Validation
{
    public class BirthdateValidationAttribute : ValidationAttribute
    {
        private static readonly BirthdateValidationAttribute _instance = new BirthdateValidationAttribute();

        public static bool IsInvalid(object value)
        {
            return !_instance.IsValid(value);
        }

        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            return date <= DateTime.UtcNow && date >= new DateTime(1900, 01, 01);
        }
    }
}
