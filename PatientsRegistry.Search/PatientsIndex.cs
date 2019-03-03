using System;
using System.Collections.Generic;
using System.Linq;
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
            var index = "patients_registry";
            var settings = new ConnectionSettings(node)
                .DefaultIndex(index)
                .ThrowExceptions()
                ;
            _elasticClient = new ElasticClient(settings);

            if (!_elasticClient.IndexExists(index).Exists)
            {
                _elasticClient.CreateIndex(index, ms =>
                    ms.Mappings(s =>
                        s.Map<PatientDto>(p =>
                            p.AutoMap()
                            .Properties(
                                ps => ps.Keyword(t => t.Name(n => n.Phone))
                                    .Keyword(t => t.Name(n => n.Birthdate))
                            )
                        )
                    )
                );
            }
        }

        public async Task<IEnumerable<PatientDto>> SearchAsync(SearchParameters parameters)
        {
            var response = await _elasticClient.SearchAsync<PatientDto>(s =>
                s.Query(
                    q => q.Bool(
                        b => b.Filter(
                            c => c.MultiMatch(
                                m => m.Fields(f => f.Fields(p => p.Name, p => p.Lastname, p => p.Patronymic))
                                .Query(parameters.Name)
                                .Type(TextQueryType.CrossFields)
                                .Operator(Operator.And)
                            ),
                            c => c.Prefix(f => f.Phone, parameters.Phone),
                            c => c.Prefix(f => f.Birthdate, parameters.Birthdate)
                        )
                    )
                )
            );

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
