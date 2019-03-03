using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientsRegistry.Search
{
    public interface IPatientsIndex
    {
        Task<IEnumerable<PatientDto>> SearchAsync(SearchParameters parameters);

        Task IndexAsync(PatientDto patient);

        Task DeleteAsync(string id);
    }
}
