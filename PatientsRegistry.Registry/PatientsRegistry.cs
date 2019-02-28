using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientsRegistry.Domain;
using PatientsRegistry.Domain.Repositories;
using PatientsRegistry.Search;

namespace PatientsRegistry.Registry
{
    public sealed class PatientsRegistry : IPatientsRegistry
    {
        private readonly IPatientsRepository _patientsRepository;
        private readonly IPatientsIndex _patientsIndex;

        public PatientsRegistry(IPatientsRepository patientsRepository, IPatientsIndex patientsIndex)
        {
            _patientsRepository = patientsRepository;
            _patientsIndex = patientsIndex;
        }

        public async Task<IEnumerable<PatientDto>> FindPatientsAsync(string query)
        {
            var patients = await _patientsIndex.SearchAsync(query);

            return patients;
        }

        public async Task<PatientDto> FindPatientAsync(Guid id)
        {
            var patient = await _patientsRepository.FindPatientAsync(id);

            if (patient == null) return null;

            var dto = MapToDto(patient);

            return dto;
        }

        public async Task<PatientDto> RegisterPatientAsync(PatientCreate patientCreate)
        {
            var entity = new Patient(Guid.NewGuid(),
                new FullName(patientCreate.Name, patientCreate.Lastname, patientCreate.Patronymic),
                patientCreate.Birthdate, Enum.Parse<Gender>(patientCreate.Gender), patientCreate.MainPhoneNumber
            );

            await _patientsRepository.SavePatientAsync(entity);

            var dto = MapToDto(entity);

            await _patientsIndex.IndexAsync(dto);

            return dto;
        }

        public async Task<PatientDto> UpdatePatientAsync(PatientUpdate patientUpdate)
        {
            var id = patientUpdate.Id;
            var patient = await _patientsRepository.FindPatientAsync(id);

            patient.Name = new FullName(patientUpdate.Name, patientUpdate.Lastname, patientUpdate.Patronymic);
            patient.Birthdate = patientUpdate.Birthdate;
            patient.Gender = Enum.Parse<Gender>(patientUpdate.Gender);
            // todo: update contacts
            await _patientsRepository.SavePatientAsync(patient);

            var dto = MapToDto(patient);

            await _patientsIndex.IndexAsync(dto);

            return dto;
        }

        public async Task DeactivatePatientAsync(Guid id)
        {
            var patient = await _patientsRepository.FindPatientAsync(id);

            patient.Deactivate();

            await _patientsRepository.SavePatientAsync(patient);
            await _patientsIndex.DeleteAsync(id.ToString());
        }

        private static PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name.Name,
                Lastname = patient.Name.Lastname,
                Patronymic = patient.Name.Patronymic,
                Birthdate = patient.Birthdate,
                Gender = patient.Gender.ToString(),
                Contacts = patient.Contacts.Select(c => new ContactDto
                {
                    Type = c.Type.ToString(),
                    Kind = c.Kind.ToString(),
                    Value = c.Value
                })
            };
        }
    }
}
