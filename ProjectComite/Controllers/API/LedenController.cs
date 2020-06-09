using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectComite.Models;
using ProjectComite.data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProjectComite.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedenController : ControllerBase
    {
        private readonly ComiteContext _context;

        public LedenController(ComiteContext context)
        {
            _context = context;
        }

        // GET: api/Leden
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IEnumerable<Lid> Getleden()
        {
            return _context.leden;/*.Include(l => l.gemeente).ThenInclude(g => g.leden).ToList();*/
            //return _context.leden.Include(l => l.actieleden).ToList();
        }

        // GET: api/Leden/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]        
        public async Task<IActionResult> GetLid([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lid = await _context.leden.FindAsync(id);

            if (lid == null)
            {
                return NotFound();
            }

            return Ok(lid);
        }

        // PUT: api/Leden/5
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLid([FromRoute] int id, [FromBody] Lid lid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lid.lidId)
            {
                return BadRequest();
            }

            _context.leden.Update(lid);
            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LidExists(id))
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

        // POST: api/Leden
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostLid([FromBody] Lid lid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.leden.Add(lid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLid", new { id = lid.lidId }, lid);
        }

        // DELETE: api/Leden/5
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLid([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lid = await _context.leden.FindAsync(id);
            if (lid == null)
            {
                return NotFound();
            }

            _context.leden.Remove(lid);
            await _context.SaveChangesAsync();

            return Ok(lid);
        }

        private bool LidExists(int id)
        {
            return _context.leden.Any(e => e.lidId == id);
        }
    }
}