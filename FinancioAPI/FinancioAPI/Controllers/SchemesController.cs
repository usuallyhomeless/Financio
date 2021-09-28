using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancioAPI.Models;

namespace FinancioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemesController : ControllerBase
    {
        private readonly financioContext _context;

        public SchemesController(financioContext context)
        {
            _context = context;
        }

        // * GET: api/schemes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scheme>>> GetScheme()
        {
            return await _context.Scheme.ToListAsync();
        }        
    }
}