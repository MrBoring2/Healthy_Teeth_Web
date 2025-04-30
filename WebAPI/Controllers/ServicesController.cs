using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.DTO;
using AutoMapper;
using Shared.Models;
using WebAPI.SignalR;
using WebAPI.Filters;
using WebAPI.Helpers;
using Shared.Constants;
using System.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly HealthyTeethDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public ServicesController(HealthyTeethDbContext context, IMapper mapper, IHubContext<MainHub, IMainHub> hubContext, ILogger<EmployeesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        // GET: api/Services
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet]
        public async Task<ActionResult<DataServiceResult<ServiceDTO>>> GetServices(string? search, string? spesializationIds, string? orderBy, string top, string skip)
        {
            var orderBySplit = orderBy?.Split(' ');
            var spesIds = spesializationIds?.Split(',').Select(int.Parse);

            ServiceFilter filter;
            try
            {
                filter = new ServiceFilter(search, spesIds, orderBySplit?[1], orderBySplit?[0], int.Parse(top), int.Parse(skip));
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

            IQueryable<Service> services;

            if (filter.OrderDirection == "asc")
            {
                services = _context.Services
                                  .Include(p => p.Specialization)
                                  .Where(filter.FilterExpression)
                                  .OrderBy(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                  .AsQueryable();
            }
            else
            {
                services = _context.Services
                                 .Include(p => p.Specialization)
                                 .Where(filter.FilterExpression)
                                 .OrderByDescending(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                 .AsQueryable();
            }

            var count = services.Count();

            services = services.Skip(filter.Skip).Take(filter.Top);

            return new DataServiceResult<ServiceDTO>(_mapper.Map<IEnumerable<ServiceDTO>>(services), count);
        }
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet("GetForSpecialization/{spesializationid}")]
        public async Task<ActionResult<Service>> GetServicesForSpeciazliation(int spesializationid)
        {
            Console.WriteLine("dasdasdasd " + spesializationid);
            var services = _context.Services.Where(p => p.SpecializationId == spesializationid).AsQueryable();

            if (services == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<ServiceDTO>>(services));
        }

        // GET: api/Services/5
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(p => p.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ServiceDTO>(service));
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.ADMIN}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, ServiceDTO service)
        {
            if (ModelState.IsValid)
            {
                if (id != service.Id)
                {
                    return NotFound();
                }

                var serviceDb = await _context.Services.FirstOrDefaultAsync(p => p.Id == id);
                serviceDb.Title = service.Title;
                serviceDb.Price = service.Price;
                serviceDb.SpecializationId = service.SpecializationId;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).ServicesChanged("Успешно");
                _logger.LogInformation($"Пользователь {HttpContext.User.Identity.Name} обновил услугу с id {service.Id}");
                return Ok("Услуга успешно изменена");
            }
            else
            {
                return BadRequest("Данные не прошли проверку");
            }
        }

        // POST: api/Services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = $"{Roles.ADMIN}")]
        [HttpPost]
        public async Task<ActionResult<ServiceDTO>> PostService(ServiceDTO service)
        {
            if (ModelState.IsValid)
            {
                var serviceDb = new Service
                {
                    Price = service.Price,
                    SpecializationId = service.SpecializationId,
                    Title = service.Title
                };
                _context.Services.Add(serviceDb);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).ServicesChanged("Успешно");
                _logger.LogInformation($"Пользователь {HttpContext.User.Identity.Name} создал услугу с id {service.Id}");
                return CreatedAtAction("GetService", new { id = service.Id }, service);
            }
            else
            {
                return BadRequest("Данные не прошли проверку");
            }
        }

        // DELETE: api/Services/5
        [Authorize(Roles = $"{Roles.ADMIN}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            try
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Groups(Roles.ADMIN, Roles.REGISTRATOR, Roles.DOCTOR).ServicesChanged("Успешно");
                _logger.LogWarning($"Пользователь {HttpContext.User.Identity.Name} удалил услугу с id {service.Id}");
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить услугу: " + ex.Message);
            }

            return Ok();
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
