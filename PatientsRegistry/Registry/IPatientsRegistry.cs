using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientsRegistry.Search;

namespace PatientsRegistry.Registry
{
    public interface IPatientsRegistry
    {
        Task<PatientDto> FindPatientAsync(Guid id);

        Task<IEnumerable<PatientDto>> FindPatientsAsync(string query);

        Task<PatientDto> RegisterPatientAsync(PatientCreate patientCreate);

        Task<PatientDto> UpdatePatientAsync(PatientUpdate patient);

        Task DeactivatePatientAsync(Guid id);
    }
}
