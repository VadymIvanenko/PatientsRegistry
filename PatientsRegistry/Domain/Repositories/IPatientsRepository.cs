using System;
using System.Threading.Tasks;

namespace PatientsRegistry.Domain.Repositories
{
    public interface IPatientsRepository
    {
        Task<Patient> FindPatientAsync(Guid id);

        Task SavePatientAsync(Patient patient);
    }
}
