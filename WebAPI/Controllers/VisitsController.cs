using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Data;
using Humanizer;
using Shared.DTO;
using Shared.Models;
using WebAPI.Filters;
using WebAPI.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using WebAPI.SignalR;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly HealthyTeethDbContext _context;

        public VisitsController(HealthyTeethDbContext context, IMapper mapper, IHubContext<MainHub, IMainHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpGet]
        public async Task<ActionResult<DataServiceResult<VisitDTO>>> GetVisits(string? patient, string? doctor, string? statusesIds, string startDate, string endDate, string? orderBy, string top, string skip)
        {
            var orderBySplit = orderBy?.Split(' ');
            var startDateOnly = DateOnly.ParseExact(startDate, "dd.MM.yyyy");
            var endDateOnly = DateOnly.ParseExact(endDate, "dd.MM.yyyy");
            var statusIds = statusesIds?.Split(',').Select(int.Parse);
            VisitsFilter filter;
            try
            {
                filter = new VisitsFilter(patient, doctor, statusIds, startDateOnly, endDateOnly, orderBySplit?[1], orderBySplit?[0], int.Parse(top), int.Parse(skip));
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

            IQueryable<Visit> visits;

            if (filter.OrderDirection == "asc")
            {
                visits = _context.Visits
                                  .Include(p => p.Patient)
                                  .Include(p => p.Employee)
                                  .Include(p => p.VisitStatus)
                                  .AsNoTracking()
                                  .Where(filter.FilterExpression)
                                  .OrderBy(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                  .AsQueryable();
            }
            else
            {
                visits = _context.Visits
                                 .Include(p => p.Patient)
                                 .Include(p => p.Employee)
                                 .Include(p => p.VisitStatus)
                                 .AsNoTracking()
                                 .Where(filter.FilterExpression)
                                 .OrderByDescending(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                 .AsQueryable();
            }

            var count = visits.Count();

            visits = visits.Skip(filter.Skip).Take(filter.Top);

            return new DataServiceResult<VisitDTO>(_mapper.Map<IEnumerable<VisitDTO>>(visits), count);
        }


        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetVisit(int id)
        {
            var visit = await _context.Visits.Include(p => p.Employee).ThenInclude(p => p.Specialization)
                .Include(p => p.Patient)
                .Include(p => p.VisitStatus)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VisitDTO>(visit));
        }

        [Authorize(Roles = $"{Roles.DOCTOR}")]
        [HttpGet("GetForDoctor")]
        public async Task<ActionResult<ICollection<Visit>>> GetVisitsForDoctor(int doctorId, string startDate, string endDate)
        {
            var startDateOnly = DateOnly.ParseExact(startDate, "dd.MM.yyyy");
            var endDateOnly = DateOnly.ParseExact(endDate, "dd.MM.yyyy");
            if (doctorId == 0)
            {
                return NotFound();
            }

            var visit = _context.Visits.Include(p => p.Patient)
                                                      .Include(p => p.VisitStatus)
                                                      .Where(p => p.EmployeeId == doctorId && p.VisitDate >= startDateOnly && (p.VisitDate == endDateOnly || p.VisitDate < endDateOnly))
                                                      .AsQueryable();

            if (visit == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ICollection<VisitDTO>>(visit));
        }

        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisit(int id, VisitDTO visit)
        {
            if (id != visit.Id)
            {
                return NotFound();
            }

            var visitDb = await _context.Visits.FirstOrDefaultAsync(p => p.Id == id);
            visitDb.VisitPurpose = visit.VisitPurpose;
            visitDb.VisitDiagnos = visit.VisitDiagnos;
            visitDb.VisitObjectively = visit.VisitObjectively;
            visitDb.VisitDate = visit.VisitDate;
            visitDb.VisitStatusId = visit.VisitStatusId;
            visitDb.EmployeeId = visit.EmployeeId;
            visitDb.VisirtTime = visit.VisirtTime;
            visitDb.ServiceToVisits = visit.ServiceToVisits;

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).VisitsChanged("Успешно");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
         
            return Ok();
        }


        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpPost("ChangeStatus")]
        public async Task<IActionResult> ChangeVisitStatus(VisitStatusChangeViewModel data)
        {

            Console.WriteLine(data.VisitStatusId);

            var visitDb = await _context.Visits.FirstOrDefaultAsync(p => p.Id == data.Id);

            if (visitDb == null)
                return NotFound();

            if (data.VisitStatusId == (int)VisitStatuses.Waiting && visitDb.VisitStatusId != (int)VisitStatuses.Written)
                return BadRequest("Для подтверждения записи статус записи должен быть 'Ожидание'");

            if (data.VisitStatusId == (int)VisitStatuses.Compleated && visitDb.VisitStatusId != (int)VisitStatuses.Waiting)
                return BadRequest("Для завершения записи статус записи записи должен быть 'Ожидание'");

            if (data.VisitStatusId == (int)VisitStatuses.Canceled && (visitDb.VisitStatusId != (int)VisitStatuses.Written && visitDb.VisitStatusId != (int)VisitStatuses.Waiting))
                return BadRequest("Для отмены записи статус записи должен быть 'Записан' или 'Ожидание'");

            if (data.VisitStatusId == (int)VisitStatuses.NotCome && visitDb.VisitStatusId != (int)VisitStatuses.Written)
                return BadRequest("Для пометки записи как 'Не пришёл' статус записи должен быть 'Записан'");

            visitDb.VisitStatusId = data.VisitStatusId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).VisitsChanged("Успешно");
            return Ok();
        }



        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpPost]
        public async Task<ActionResult<Visit>> PostVisit(VisitDTO visit)
        {
            var visitDb = new Visit
            {
                EmployeeId = visit.EmployeeId,
                PatientId = visit.PatientId,
                VisirtTime = visit.VisirtTime,
                VisitDate = visit.VisitDate,
                VisitPurpose = visit.VisitPurpose,
                VisitStatusId = 1
            };
            _context.Visits.Add(visitDb);
            await _context.SaveChangesAsync();
            var userId = await _context.Employees.Include(p => p.Account).FirstOrDefaultAsync(p => p.Id == visit.EmployeeId);
            await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).VisitsChanged("Успешно");
            await _hubContext.Clients.User(userId.Account.Login).VisitsChanged("Успешно");

            return CreatedAtAction("GetVisit", new { id = visit.Id }, visit);
        }
        [Authorize]
        // DELETE: api/Visits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            if (visit.VisitStatusId == (int)VisitStatuses.Compleated)
                return BadRequest("Нельзя удалить завершённую запись");

            if (visit.VisitStatusId == (int)VisitStatuses.Waiting)
                return BadRequest("Нельзя удалить ожидающую запись");

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).VisitsChanged("Успешно");
            return Ok();
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}
