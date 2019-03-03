using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PatientsRegistry.Registry;
using PatientsRegistry.Search;

namespace PatientsRegistry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsRegistry _patientsRegistry;

        public PatientsController(IPatientsRegistry patientsRegistry)
        {
            _patientsRegistry = patientsRegistry;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]SearchParameters parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patients = await _patientsRegistry.FindPatientsAsync(parameters);

            return Ok(patients);
        }

        [HttpGet("{id}", Name = nameof(PatientsController.Get))]
        public async Task<IActionResult> Get(string id)
        {
            var patient = await _patientsRegistry.FindPatientAsync(Guid.Parse(id));

            if (patient == null) return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PatientCreate patientCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = await _patientsRegistry.RegisterPatientAsync(patientCreate);

            return CreatedAtRoute(nameof(PatientsController.Get), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] PatientUpdate patientUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = await _patientsRegistry.UpdatePatientAsync(patientUpdate);

            return Ok(patient);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _patientsRegistry.DeactivatePatientAsync(Guid.Parse(id));

            return Ok();
        }

        [HttpPut("{id}/contacts")]
        public async Task<IActionResult> PutContactAsync(string id, [FromBody] ContactDto contact)
        {
            contact = await _patientsRegistry.SetContact(Guid.Parse(id), contact);

            return Ok(contact);
        }

        [HttpDelete("{id}/contacts")]
        public async Task<IActionResult> DeleteContactAsync(string id, string type, string kind)
        {
            await _patientsRegistry.RemoveContact(Guid.Parse(id), type, kind);

            return Ok();
        }

        [HttpPost("/seed")]
        public async Task<IActionResult> Seed()
        {
            // dev seed
            var names = new[] { "Иван", "Константин", "Алексей", "Владислав" };
            var lastnames = new[] { "Яременко", "Смирнов", "Полищук", "Доронин", "Петренко" };
            var patronymics = new[] { "Александрович", "Семенович", "Алексеевич", "Петрович" };
            var rand = new Random();
            var requests = new int[10].Select(
                s => _patientsRegistry.RegisterPatientAsync(new PatientCreate
                {
                    Name = names[rand.Next(0, names.Length - 1)],
                    Lastname = lastnames[rand.Next(0, lastnames.Length - 1)],
                    Patronymic = patronymics[rand.Next(0, patronymics.Length - 1)],
                    Phone = "+380" + rand.Next(100000000, 999999999).ToString(),
                    Birthdate = new DateTime(rand.Next(1900, 2000), rand.Next(1, 12), rand.Next(1, 28)),
                    Gender = "Male"
                })
            );
            await Task.WhenAll(requests);

            return Ok();
        }
    }
}
