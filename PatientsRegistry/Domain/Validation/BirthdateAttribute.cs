using System;
using System.ComponentModel.DataAnnotations;

namespace PatientsRegistry.Domain.Validation
{
    public class BirthdateAttribute : ValidationAttribute
    {
        private static readonly BirthdateAttribute _instance = new BirthdateAttribute();

        public static bool IsInvalid(object value)
        {
            return !_instance.IsValid(value);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var date = (DateTime)value;
            return date <= DateTime.UtcNow && date >= new DateTime(1900, 01, 01);
        }
    }
}
