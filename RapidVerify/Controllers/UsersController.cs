using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassportVerification.Models;
using RapidVerify.Dto;

namespace PassportVerification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataConnect _context;

        public UsersController(DataConnect context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VerifiedUser>>> GetVerifiedUsers()
        {
            if (_context.VerifiedUsers == null)
            {
                return NotFound();
            }
            return await _context.VerifiedUsers.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VerifiedUser>> GetVerifiedUser(int id)
        {
            if (_context.VerifiedUsers == null)
            {
                return NotFound();
            }
            var verifiedUser = await _context.VerifiedUsers.FindAsync(id);

            if (verifiedUser == null)
            {
                return NotFound();
            }

            return verifiedUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVerifiedUser(int id, VerifiedUser verifiedUser)
        {
            if (id != verifiedUser.VId)
            {
                return BadRequest();
            }

            _context.Entry(verifiedUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerifiedUserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VerifiedUser>> PostVerifiedUser(VerifiedUser verifiedUser)
        {
            if (_context.VerifiedUsers == null)
            {
                return Problem("Entity set 'DataConnect.VerifiedUsers'  is null.");
            }
            _context.VerifiedUsers.Add(verifiedUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVerifiedUser", new { id = verifiedUser.VId }, verifiedUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVerifiedUser(int id)
        {
            if (_context.VerifiedUsers == null)
            {
                return NotFound();
            }
            var verifiedUser = await _context.VerifiedUsers.FindAsync(id);
            if (verifiedUser == null)
            {
                return NotFound();
            }

            _context.VerifiedUsers.Remove(verifiedUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VerifiedUserExists(int id)
        {
            return (_context.VerifiedUsers?.Any(e => e.VId == id)).GetValueOrDefault();
        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] EmailVerifyDto userLogin)
        {
            var user = _context.VerifiedUsers.FirstOrDefault(u => u.Email == userLogin.email);
            if (user == null || user.Email != userLogin.email)
            {
                return BadRequest(new { success = false, message = "invalid email" });
            }
            return Ok(new { success = true });
        }


        // GET: api/Users/5
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<VerifiedUser>> GetVerifiedUserByEmail([FromRoute] string email)
        {
            var verifiedUser = await _context.VerifiedUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (verifiedUser == null)
            {
                return NotFound();
            }

            return verifiedUser;
        }
    }
}
