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
using Shared.DTO;
using AutoMapper;
using Shared.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;

        public SpecializationsController(HealthyTeethDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Specializations
        [Authorize]
        [HttpGet]
        public async Task<DataServiceResult<SpecializationDTO>> GetSpecializations()
        {
            var specializations = await _context.Specializations.ToListAsync();
            return new(_mapper.Map<IEnumerable<SpecializationDTO>>(specializations), specializations.Count);
        }

        // GET: api/Specializations/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

    }
}
