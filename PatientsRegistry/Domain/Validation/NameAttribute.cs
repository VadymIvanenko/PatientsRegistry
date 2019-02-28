using System.ComponentModel.DataAnnotations;

namespace PatientsRegistry.Domain.Validation
{
    public class NameAttribute : ValidationAttribute
    {
        private static readonly NameAttribute _instance = new NameAttribute();

        public static bool IsInvalid(object value)
        {
            return !_instance.IsValid(value);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var name = (string)value;
            if (name.Length > 128)
                return false;
            // todo: change to cyrylic
            return new RegularExpressionAttribute(@"^[a-zA-Z]+(([' -][a-zA-Z ])?[a-zA-Z]*)*$").IsValid(name);
            //return new RegularExpressionAttribute(@"^[а-щА-ЩЬьЮюЯяЇїІіЄєҐґ]+(([' -][а-щА-ЩЬьЮюЯяЇїІіЄєҐґ ])?[а-щА-ЩЬьЮюЯяЇїІіЄєҐґ]*)*$").IsValid(name);
        }
    }
}
