using System;
using System.ComponentModel.DataAnnotations;
using PatientsRegistry.Domain;
using PatientsRegistry.Domain.Validation;

namespace PatientsRegistry.Registry
{
    public sealed class PatientCreate
    {
        [Required]
        [Name]
        public string Name { get; set; }

        [Required]
        [Name]
        public string Lastname { get; set; }

        [Name]
        public string Patronymic { get; set; }

        [BirthdateValidation]
        public DateTime Birthdate { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public string Gender { get; set; }

        [Required]
        [CellPhone]
        public string MainPhoneNumber { get; set; }
    }
}
