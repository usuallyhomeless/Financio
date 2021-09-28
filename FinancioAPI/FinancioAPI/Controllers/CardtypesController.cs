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
    public class CardtypesController : ControllerBase
    {
        private readonly financioContext _context;

        public CardtypesController(financioContext context)
        {
            _context = context;
        }

        // * GET: api/cardtypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cardtype>>> GetCardtype()
        {
            return await _context.Cardtype.ToListAsync();
        }        
    }
}
