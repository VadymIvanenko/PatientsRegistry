using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PatientsRegistry.Registry;

namespace PatientsRegistry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        public sealed class SearchParameters
        {
            [Required]
            [MinLength(3)]
            [MaxLength(256)]
            public string Query { get; set; }
        }

        private readonly IPatientsRegistry _patientsRegistry;

        public PatientsController(IPatientsRegistry patientsRegistry)
        {
            _patientsRegistry = patientsRegistry;
        }

        [HttpGet]
        public async Task<IActionResult> GetByText([FromQuery]SearchParameters parameters)
        {
            var patients = await _patientsRegistry.FindPatientsAsync(parameters.Query);

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
    }
}
