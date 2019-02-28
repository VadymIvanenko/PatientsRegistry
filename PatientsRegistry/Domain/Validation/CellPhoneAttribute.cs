using System.ComponentModel.DataAnnotations;

namespace PatientsRegistry.Domain.Validation
{
    public class CellPhoneAttribute : ValidationAttribute
    {
        private static readonly CellPhoneAttribute _instance = new CellPhoneAttribute();

        public static bool IsInvalid(object value)
        {
            return !_instance.IsValid(value);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var phoneNumber = (string)value;
            return new RegularExpressionAttribute(@"^\+[0-9]{12,12}$").IsValid(phoneNumber);
        }
    }
}
