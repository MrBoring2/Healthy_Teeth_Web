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
using Shared.Constants;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<SpecializationsController> _logger;
        public SpecializationsController(HealthyTeethDbContext context, IMapper mapper, IMemoryCache cache, ILogger<SpecializationsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        // GET: api/Specializations
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}, {Roles.DOCTOR}")]
        [HttpGet]
        public async Task<IEnumerable<SpecializationDTO>> GetSpecializations()
        {
            _cache.TryGetValue("specializations", out IEnumerable<SpecializationDTO> specializations);
            try
            {
                await semaphore.WaitAsync();
                if (specializations == null)
                {

                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Priority = CacheItemPriority.Normal,
                    };
                    specializations = _mapper.Map<IEnumerable<SpecializationDTO>>(_context.Specializations.AsQueryable());
                    _cache.Set("specializations", specializations, cacheOptions);
                    _logger.LogInformation("Специализации записаны в кэш");
                }
            }
            finally
            {
                semaphore.Release();
            }
            return specializations;
        }
    }
}
