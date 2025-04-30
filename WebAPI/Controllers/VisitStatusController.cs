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
using Microsoft.Extensions.Caching.Memory;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitStatusController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<VisitStatusController> _logger;
        public VisitStatusController(HealthyTeethDbContext context, IMapper mapper, IMemoryCache memoryCache, ILogger<VisitStatusController> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = memoryCache;
            _logger = logger;
        }

        // GET: api/VisitStatus
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpGet]
        public async Task<IEnumerable<VisitStatusDTO>> GetVisitStatuses()
        {
            _cache.TryGetValue("visitStatuses", out IEnumerable<VisitStatusDTO> visitStatuses);
            try
            {
                await semaphore.WaitAsync();
                if (visitStatuses == null)
                {

                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Priority = CacheItemPriority.Normal,
                    };
                    visitStatuses = _mapper.Map<IEnumerable<VisitStatusDTO>>(_context.VisitStatuses.AsQueryable());
                    _cache.Set("visitStatuses", visitStatuses, cacheOptions);
                    _logger.LogInformation("Статусы посещения записаны в кэш");
                }
            }
            finally
            {
                semaphore.Release();
            }
            return visitStatuses;
        }
    }
}
