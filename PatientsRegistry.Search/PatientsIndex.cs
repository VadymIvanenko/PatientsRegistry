using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace PatientsRegistry.Search
{
    public sealed class PatientsIndex : IPatientsIndex
    {
        private readonly IElasticClient _elasticClient;

        public PatientsIndex(string connectionString)
        {
            var node = new Uri(connectionString);
            var settings = new ConnectionSettings(node).DefaultIndex("patients_registry");
            _elasticClient = new ElasticClient(settings);
        }

        public async Task<IEnumerable<PatientDto>> SearchAsync(string query)
        {
            // todo: find patients by query using es
            var response = await _elasticClient.SearchAsync<PatientDto>(s => s.Query(q => q.QueryString(d => d.Query(query))));

            return response.Documents;
        }

        public async Task IndexAsync(PatientDto patient)
        {
            await _elasticClient.IndexDocumentAsync(patient);
        }

        public async Task DeleteAsync(string id)
        {
            await _elasticClient.DeleteAsync(new DeleteRequest<PatientDto>(id));
        }
    }
}
