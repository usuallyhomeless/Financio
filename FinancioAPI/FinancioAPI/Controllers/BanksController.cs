using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancioAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly financioContext _context;

        public BanksController(financioContext context)
        {
            _context = context;
        }        

        // * GET: api/banks

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBank()
        {
            return await _context.Bank.ToListAsync();
        }
    }
}
