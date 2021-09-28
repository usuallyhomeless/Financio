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
    public class IfscsController : ControllerBase
    {
        private readonly financioContext _context;

        public IfscsController(financioContext context)
        {
            _context = context;
        }        

        // * GET: api/getbybankid/5

        [HttpGet("getbybankid/{id}")]
        public async Task<ActionResult<IEnumerable<Ifsc>>> GetbyBankId(int id)
        {
            return await this._context.Ifsc.Where(x => x.Bankid == id).ToListAsync();
        }
    }
}
