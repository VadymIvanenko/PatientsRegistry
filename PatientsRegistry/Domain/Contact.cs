using System;

namespace PatientsRegistry.Domain
{
    public sealed class Contact
    {
        public string Value { get; private set; }

        public ContactKind Kind { get; private set; }

        public ContactType Type { get; private set; }

        public Contact(ContactType type, ContactKind kind, string value)
        {
            if (kind == ContactKind.Undefined)
                throw new ArgumentException("", nameof(kind));
            if (!type.IsValid(value))
                throw new ArgumentException("", nameof(value));

            Type = type;
            Kind = kind;
            Value = value;
        }
    }
}
