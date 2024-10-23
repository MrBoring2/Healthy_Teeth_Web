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
        public async Task<IEnumerable<SpecializationDTO>> GetSpecializations()
        {
            var specializations = _context.Specializations.AsQueryable();
            return _mapper.Map<IEnumerable<SpecializationDTO>>(specializations);
        }
    }
}
