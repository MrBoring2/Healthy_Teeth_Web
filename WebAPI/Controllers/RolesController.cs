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
using Shared.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly HealthyTeethDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RolesController> _logger;
        public RolesController(HealthyTeethDbContext context, IMapper mapper, IMemoryCache cache, ILogger<RolesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        // GET: api/Roles
        [Authorize(Roles = $"{Roles.ADMIN}, {Roles.REGISTRATOR}")]
        [HttpGet]
        public async Task<IEnumerable<RoleDTO>> GetRoles()
        {
            _cache.TryGetValue("roles", out IEnumerable<RoleDTO> roles);
            try
            {
                await semaphore.WaitAsync();
                if (roles == null)
                {

                    var cacheOptions = new MemoryCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        Priority = CacheItemPriority.Normal,
                    };
                    roles = _mapper.Map<IEnumerable<RoleDTO>>(_context.Roles.AsQueryable());
                    _cache.Set("roles", roles, cacheOptions);
                    _logger.LogInformation("Роли записаны в кэш");
                }
            }
            finally
            {
                semaphore.Release();
            }
            return roles;
        }
    }
}
