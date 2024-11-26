using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;
using Shared.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitStatusController : ControllerBase
    {
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;

        public VisitStatusController(HealthyTeethDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/VisitStatus
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpGet]
        public async Task<IEnumerable<VisitStatusDTO>> GetVisitStatuses()
        {
            var roles = _context.VisitStatuses.AsQueryable();
            return _mapper.Map<IEnumerable<VisitStatusDTO>>(roles);
        }
           }
}
