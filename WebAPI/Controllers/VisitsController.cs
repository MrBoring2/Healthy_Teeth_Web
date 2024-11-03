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
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet]
        public async Task<ActionResult<DataServiceResult<VisitDTO>>> GetVisits(string? search, string? orderBy, string top, string skip)
        {
            var orderBySplit = orderBy?.Split(' ');

            VisitFilter filter;
            try
            {
                filter = new VisitFilter(search, orderBySplit?[1], orderBySplit?[0], int.Parse(top), int.Parse(skip));
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
                                  .Where(filter.FilterExpression)
                                  .OrderBy(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                  .AsQueryable();
            }
            else
            {
                visits = _context.Visits
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
            var visit = await _context.Visits.FirstOrDefaultAsync(p => p.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VisitDTO>(visit));
        }
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisit(int id, VisitDTO visit)
        {
            if (id != visit.Id)
            {
                return BadRequest();
            }

            var visitDb = await _context.Visits.FirstOrDefaultAsync(p => p.Id == id);
            visitDb.VisitPurpose = visit.VisitPurpose;
            visitDb.VisitDiagnos = visit.VisitDiagnos;
            visitDb.VisitObjectively = visit.VisitObjectively;
            visitDb.VisitDate = visit.VisitDate;
            visitDb.VisitStatusId = visit.VisitStatusId;
            visitDb.EmployeeId = visit.EmployeeId;
            visitDb.VisirtTime = visit.VisirtTime;

            try
            {
                await _context.SaveChangesAsync();
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
            await _hubContext.Clients.Group("Администратор").VisitsChanged("Успешно");
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
                VisitStatusId = visit.VisitStatusId

            };
            _context.Visits.Add(visitDb);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("Администратор").VisitsChanged("Успешно");

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

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}
