using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;
using Data;
using Shared.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.DTO;
using AutoMapper;
using WebAPI.SignalR;
using System.Reflection;
using System.Collections;
using WebAPI.Filters;
using WebAPI.Helpers;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(HealthyTeethDbContext context, IMapper mapper, IHubContext<MainHub, IMainHub> hubContext, ILogger<EmployeesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _logger = logger;
        }

        // GET: api/Employees
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DataServiceResult<EmployeeDTO>>> GetEmployees(string? search, string? orderBy, string? rolesIds, string? spesializationIds, string top, string skip)
        {
            //Console.WriteLine(search + " " + orderBy + " " + rolesIds + " " + spesializationIds + " " + top + " " + skip);
            //_logger.LogInformation("Переход к сотрудникам");
            var orderBySplit = orderBy?.Split(' ');
            var roleIds = rolesIds?.Split(',').Select(int.Parse);
            var spesIds = spesializationIds?.Split(',').Select(int.Parse);

            EmployeeFilter filter;
            try
            {
                filter = new EmployeeFilter(search, roleIds, spesIds, orderBySplit?[1], orderBySplit?[0], int.Parse(top), int.Parse(skip));
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

            IQueryable<Employee> employees;

            if (filter.OrderDirection == "asc")
            {
                employees = _context.Employees
                                  .Include(p => p.Specialization)
                                  .Include(p => p.Account)
                                  .ThenInclude(p => p.Role)
                                  .Where(filter.FilterExpression)
                                  .OrderBy(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                  .AsQueryable();
            }
            else
            {
                employees = _context.Employees
                                 .Include(p => p.Specialization)
                                 .Include(p => p.Account)
                                 .ThenInclude(p => p.Role)
                                 .Where(filter.FilterExpression)
                                 .OrderByDescending(p => GetPropertyHelper.GetPropertyValue(p, filter.OrderBy))
                                 .AsQueryable();
            }

            var count = employees.Count();

            employees = employees.Skip(filter.Skip).Take(filter.Top);

            return new DataServiceResult<EmployeeDTO>(_mapper.Map<IEnumerable<EmployeeDTO>>(employees), count);
        }

        // GET: api/Employees/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees.Include(p => p.Account).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }

        [Authorize]
        [HttpGet("/api/Employees/ForSchedule")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployeesForSchedule(string date, int specializationId)
        {
            var dateOnly = DateOnly.ParseExact(date, "dd.MM.yyyy");
            var employees = _context.Employees
                                     .Where(p => p.SpecializationId == specializationId)
                                     .Include(p => p.Schedules)
                                     .Include(p => p.Visits
                                     .Where(p => p.VisitDate == dateOnly))
                                     .ThenInclude(p => p.Patient)
                                     .AsQueryable();

            return Ok(employees);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            var employeeDb = await _context.Employees.Include(p => p.Account).Include(p => p.Schedules).FirstOrDefaultAsync(p => p.Id == id);
            employeeDb.FirstName = employee.FirstName;
            employeeDb.LastName = employee.LastName;
            employeeDb.MiddleName = employee.MiddleName;
            employeeDb.DateOfBirth = employee.DateOfBirth;
            employeeDb.Gender = employee.Gender;
            employeeDb.Phone = employee.Phone;
            employeeDb.SpecializationId = employee.SpecializationId;
            employeeDb.Account.Login = employee.Login;
            employeeDb.Account.RoleId = employee.RoleId;
            Console.WriteLine(employee.Schedules.Count);
            employeeDb.Schedules = _mapper.Map<IEnumerable<Schedule>>(employee.Schedules).ToList();
            if (employee.ChangePassword)
            {
                byte[] passwordHash, passwordSalt;
                PasswordHasher.CreatePasswordHash(employee.Password, out passwordHash, out passwordSalt);
                employeeDb.Account.PasswordHash = passwordHash;
                employeeDb.Account.PasswordSalt = passwordSalt;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await _hubContext.Clients.Group("Администратор").EmployeeAdded("Успешно");
            return Ok();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeViewModel employee)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash(employee.Password, out passwordHash, out passwordSalt);
            var dbEmployee = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                Phone = employee.Phone,
                SpecializationId = employee.SpecializationId,
                Account = new Account
                {
                    Login = employee.Login,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    RoleId = employee.RoleId
                }
            };
            _context.Employees.Add(dbEmployee);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("Администратор").EmployeeAdded("Успешно");

            return CreatedAtAction("GetEmployee", new { id = dbEmployee.Id }, dbEmployee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("Не удалось удалить пользователя: " + ex.Message);
            }

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
