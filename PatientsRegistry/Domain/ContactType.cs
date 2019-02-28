using System;
using PatientsRegistry.Domain.Validation;

namespace PatientsRegistry.Domain
{
    public enum ContactType
    {
        Undefined = 0,
        Phone,
        Other
    }

    public static class ContactTypeExtensions
    {
        public static bool IsValid(this ContactType type, string value)
        {
            switch (type)
            {
                case ContactType.Undefined:
                    throw new ArgumentException("Contact type is undefined.", nameof(type));
                case ContactType.Phone:
                    return !CellPhoneAttribute.IsInvalid(value);
                case ContactType.Other:
                    return string.IsNullOrWhiteSpace(value);
                default:
                    throw new Exception($"Validation rule was not found for {type} value.");
            }
        }
    }
}
