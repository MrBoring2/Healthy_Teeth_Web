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

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(HealthyTeethDbContext context, IMapper mapper, IHubContext<MainHub, IMainHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // GET: api/Employees
        [Authorize]
        [HttpGet]
        public async Task<DataServiceResult<EmployeeDTO>> GetEmployees(string? search, string? orderBy, string? rolesIds, string? spesializationIds, string top, string skip)
        {

            Console.WriteLine(search + " " + orderBy + " " + rolesIds + " " + spesializationIds + " " + top + " " + skip);

            var employees = _context.Employees.Include(p => p.Specialization).Include(p => p.Account).ThenInclude(p => p!.Role)
                .AsQueryable();
            if (string.IsNullOrEmpty(orderBy))
            {
                Console.WriteLine("Сортировка по умолчанию");
                employees = employees.OrderBy(p => p.Id);
            }
            else
            {
                Console.WriteLine("Другая сортировка");
                var order = orderBy.Split(' ');
                Console.WriteLine(order[0]);
                if (order[0].Contains("Id"))
                {
                    if (order[1] == "asc")
                    {
                        employees = employees.OrderBy(p => p.Id);
                    }
                    else
                    {
                        employees = employees.OrderByDescending(p => p.Id);
                    }
                }
                else if (order[0].Contains("FullName"))
                {
                    if (order[1] == "asc")
                    {
                        employees = employees.OrderBy(p => p.Id);
                    }
                    else
                    {
                        employees = employees.OrderByDescending(p => p.Id);
                    }
                }
                else if (order[0].Contains("Role/Title"))
                {
                    if (order[1] == "asc")
                    {
                        employees = employees.OrderBy(p => p.Account.Role.Title);
                    }
                    else
                    {
                        employees = employees.OrderByDescending(p => p.Account.Role.Title);
                    }
                }
                else if (order[0].Contains("Login"))
                {
                    if (order[1] == "asc")
                    {
                        employees = employees.OrderBy(p => p.Account.Login);
                    }
                    else
                    {
                        employees = employees.OrderByDescending(p => p.Account.Login);
                    }
                }
                else if (order[0].Contains("Gender"))
                {
                    if (order[1] == "asc")
                    {
                        employees = employees.OrderBy(p => p.Gender);
                    }
                    else
                    {
                        employees = employees.OrderByDescending(p => p.Gender);
                    }
                }

                Console.WriteLine("Не вышло");
            }

            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(p => p.FullName.ToLower().Contains(search.ToLower()));
            }


            if (!string.IsNullOrEmpty(rolesIds))
            {
                var roleIds = rolesIds.Split(',').Select(int.Parse);

                employees = employees.Where(p => roleIds.Contains(p.Account.RoleId));
            }
            if (!string.IsNullOrEmpty(spesializationIds))
            {
                var spesIds = spesializationIds.Split(',').Select(int.Parse);

                employees = employees.Where(p => spesIds.Contains(p.SpecializationId));
            }

            var count = employees.Count();

            employees = employees.Skip(int.Parse(skip)).Take(int.Parse(top));

            return new DataServiceResult<EmployeeDTO>(_mapper.Map<IEnumerable<EmployeeDTO>>(await employees.ToListAsync()), count);
        }

        // GET: api/Employees/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

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

            return NoContent();
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

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
