using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Data;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using WebAPI.SignalR;
using Shared.DTO;
using Shared.Models;
using WebAPI.Filters;
using WebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(HealthyTeethDbContext context, IMapper mapper, IHubContext<MainHub, IMainHub> hubContext, ILogger<PatientsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet]
        public async Task<ActionResult<DataServiceResult<PatientDTO>>> GetPatients(string? search, string? orderBy, string top, string skip)
        {
            var orderBySplit = orderBy?.Split(' ');

            PatientFilter filter;
            try
            {
                filter = new PatientFilter(search, orderBySplit?[1], orderBySplit?[0], int.Parse(top), int.Parse(skip));
            }
            catch (Exception ex)
            {
                return BadRequest("Неккоректно заданные параметры");
            }
            if (orderBySplit == null)
            {
                filter.OrderBy = "Id";
                filter.OrderDirection = "asc";
            }

            IQueryable<Patient> patients;

            if (filter.OrderDirection == "asc")
            {
                patients = _context.Patients
                                  .Where(filter.FilterExpression)
                                  .OrderBy(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                  .AsQueryable();
            }
            else
            {
                patients = _context.Patients
                                 .Where(filter.FilterExpression)
                                 .OrderByDescending(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                 .AsQueryable();
            }

            var count = patients.Count();

            patients = patients.Skip(filter.Skip).Take(filter.Top);

            return new DataServiceResult<PatientDTO>(_mapper.Map<IEnumerable<PatientDTO>>(patients), count);
        }
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientDTO>(patient));
        }
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientDTO patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            var patientDb = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            patientDb.FirstName = patient.FirstName;
            patientDb.LastName = patient.LastName;
            patientDb.MiddleName = patient.MiddleName;
            patientDb.DateOfBirth = patient.DateOfBirth;
            patientDb.Gender = patient.Gender;
            patientDb.Phone = patient.Phone;
            patientDb.Address = patient.Address;
            patientDb.City = patient.City;
            patientDb.PassportNumber = patient.PassportNumber;
            patientDb.PassportCode = patient.PassportCode;
            patientDb.MedicalPolicy = patient.MedicalPolicy;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await _hubContext.Clients.Group("Администратор").PatientsChanged("Успешно");
            return Ok();
        }
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(PatientDTO patient)
        {

            var dbPatient = new Patient()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Phone = patient.Phone,
                Address = patient.Address,
                City = patient.City,
                MedicalPolicy = patient.MedicalPolicy,
                PassportCode = patient.PassportCode,
                PassportNumber = patient.PassportNumber
              };
            _context.Patients.Add(dbPatient);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("Администратор").PatientsChanged("Успешно");

            return CreatedAtAction("GetPatient", new { id = dbPatient.Id }, dbPatient);
        }

        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
