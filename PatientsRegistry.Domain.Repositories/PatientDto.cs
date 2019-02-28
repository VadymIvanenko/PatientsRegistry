using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientsRegistry.Domain.Repositories
{
    internal sealed class PatientDto
    {
        [BsonId]
        public string Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Patronymic { get; set; }

        public DateTime Birthdate { get; set; }

        public string Gender { get; set; }

        public IEnumerable<ContactDto> Contacts { get; set; }
    }
}
