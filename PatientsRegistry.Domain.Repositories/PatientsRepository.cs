using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace PatientsRegistry.Domain.Repositories
{
    public sealed class PatientsRepository : IPatientsRepository
    {
        private readonly IMongoCollection<PatientDto> _patientsCollection;

        public PatientsRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("PatientsRegistry");
            _patientsCollection = database.GetCollection<PatientDto>("Patients");
        }

        public async Task<Patient> FindPatientAsync(Guid id)
        {
            var sid = id.ToString();
            var dto = await _patientsCollection.Find(p => p.Id == sid && p.IsActive).FirstOrDefaultAsync();

            if (dto == null) return null;

            var patient = CreatePatient(dto);

            return patient;
        }

        public async Task SavePatientAsync(Patient patient)
        {
            var dto = MapToDTO(patient);

            await _patientsCollection.ReplaceOneAsync(p => p.Id == dto.Id, dto, new UpdateOptions { IsUpsert = true });
        }

        private static Patient CreatePatient(PatientDto patient)
        {
            return new Patient(Guid.Parse(patient.Id),
                new FullName(patient.Name, patient.Lastname, patient.Patronymic),
                patient.Birthdate, 
                Enum.Parse<Gender>(patient.Gender),
                patient.Contacts.Select(c => new Contact(Enum.Parse<ContactType>(c.Type), Enum.Parse<ContactKind>(c.Kind), c.Value))
            );
        }

        private static PatientDto MapToDTO(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id.ToString(),
                IsActive = patient.IsActive,
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
