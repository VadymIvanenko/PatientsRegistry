using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientsRegistry.Domain.Repositories
{
    public interface IPatientsRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();

        Task<Patient> FindPatientAsync(Guid id);

        Task SavePatientAsync(Patient patient);
    }
}
