using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Shared.DTO;
using Shared.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;

        public RolesController(HealthyTeethDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Roles
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<RoleDTO>> GetRoles()
        {
            var roles =  _context.Roles.AsQueryable();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }
    }
}
