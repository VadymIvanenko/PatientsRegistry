using System;
using PatientsRegistry.Domain.Validation;

namespace PatientsRegistry.Domain
{
    public sealed class FullName
    {
        public string Name { get; private set; }

        public string Lastname { get; private set; }

        public string Patronymic { get; private set; }

        public FullName(string name, string lastname, string patronymic = null)
        {
            if (name == null || NameAttribute.IsInvalid(name))
                throw new ArgumentException("", nameof(name));
            if (lastname == null || NameAttribute.IsInvalid(lastname))
                throw new ArgumentException("", nameof(lastname));
            if (NameAttribute.IsInvalid(patronymic))
                throw new ArgumentException("", nameof(patronymic));

            Name = name;
            Lastname = lastname;
            Patronymic = patronymic;
        }
    }
}
