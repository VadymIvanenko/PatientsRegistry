using System;
using System.Collections.Generic;

namespace PatientsRegistry.Search
{
    public sealed class PatientDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Patronymic { get; set; }

        public DateTime Birthdate { get; set; }

        public string Gender { get; set; }

        public IEnumerable<ContactDto> Contacts { get; set; }
    }
}
