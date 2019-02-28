using System;
using System.Collections.Generic;
using System.Linq;
using PatientsRegistry.Domain.Validation;

namespace PatientsRegistry.Domain
{
    public sealed class Patient
    {
        public Guid Id { get; private set; }

        public bool IsActive { get; private set; } = true;

        private FullName _name;
        public FullName Name
        {
            get { return _name; }
            set
            {
                _name = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        private DateTime _birthdate;
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set {
                if (BirthdateValidationAttribute.IsInvalid(value))
                    throw new ArgumentException("", nameof(value));

                _birthdate = value;
            }
        }

        private Gender _gender;
        public Gender Gender
        {
            get { return _gender; }
            set {
                if (value == Gender.Undefined)
                    throw new ArgumentException("", nameof(value));

                _gender = value;
            }
        }

        private readonly Dictionary<string, Contact> _contacts = new Dictionary<string, Contact>();
        public IEnumerable<Contact> Contacts => _contacts.Values;

        //public string Phone => Contacts.First(c => c.Type == ContactType.Phone && c.Kind == ContactKind.Main).Value;


        public Patient(Guid id, FullName name, DateTime birthdate, Gender gender, string mainPhoneNumber)
            : this(id, name, birthdate, gender)
        {
            var phoneNumber = new Contact(ContactType.Phone, ContactKind.Main, mainPhoneNumber);
            SetContact(phoneNumber);
        }

        public Patient(Guid id, FullName name, DateTime birthdate, Gender gender, IEnumerable<Contact> contacts)
            : this(id, name, birthdate, gender)
        {
            if (!contacts.Any(c => c.Type == ContactType.Phone && c.Kind == ContactKind.Main))
                throw new ArgumentException("Contacts must contain at least one main phone number.", nameof(contacts));

            foreach (var contact in contacts)
            {
                SetContact(contact);
            }
        }

        private Patient(Guid id, FullName name, DateTime birthdate, Gender gender)
        {
            Id = id;
            Name = name;
            Birthdate = birthdate;
            Gender = gender;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void SetContact(Contact contact)
        {
            var key = string.Concat(contact.Type, contact.Kind);
            if (_contacts.ContainsKey(key))
                _contacts[key] = contact;
            else
                _contacts.Add(key, contact);
        }

        public void RemoveContact(Contact contact)
        {
            if (contact.Type == ContactType.Phone && contact.Kind == ContactKind.Main)
                throw new Exception("Main phone number is required contact and cannot be deleted.");

            var key = string.Concat(contact.Type, contact.Kind);

            if (!_contacts.ContainsKey(key))
                throw new Exception("Contact was not found.");

            _contacts.Remove(key);
        }
    }
}
