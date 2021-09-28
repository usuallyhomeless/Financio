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
    public class CardsController : ControllerBase
    {
        private readonly financioContext _context;

        public CardsController(financioContext context)
        {
            _context = context;
        }

        // * GET: api/cards

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Card>>> GetCard()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            if (SubjectUser.Isadmin == null || SubjectUser.Isadmin == false) return null;
            return await _context.Card.ToListAsync();
        }
        
        // *  GET: /api/cards/user/2

        [HttpGet("user/{id}")]
        public async Task<ActionResult<Card>> GetUserCard(int id)
        {
            var card = await _context.Card.Where(c => c.Financiouser == id).FirstOrDefaultAsync();

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        // * PUT: api/cards/5        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCard(int id, Card card)
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            if (SubjectUser.Isadmin == null || SubjectUser.Isadmin == false) return null;
            if (id != card.Financiouser)
            {
                return BadRequest();
            }

            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
       
        private bool CardExists(int id)
        {
            return _context.Card.Any(e => e.Financiouser == id);
        }
    }
}
