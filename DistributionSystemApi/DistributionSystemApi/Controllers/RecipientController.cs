using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistributionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly ContentDistributionSystemContext _context;

        public RecipientController(ContentDistributionSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipient>>> GetRecipients()
        {
            return await _context.Recipient.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipient>> GetRecipient(Guid id)
        {
            var recipient = await _context.Recipient
                .Include(r => r.Group)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipient == null)
            {
                return NotFound();
            }

            return recipient;
        }

        [HttpPost]
        public async Task<ActionResult<Recipient>> CreateRecipient(Recipient recipient)
        {
            recipient.Id = Guid.NewGuid();
            _context.Recipient.Add(recipient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipient", new { id = recipient.Id }, recipient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipient(Guid id, Recipient recipient)
        {
            if (id != recipient.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipient).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipient(Guid id)
        {
            var recipient = await _context.Recipient.FindAsync(id);

            if (recipient == null)
            {
                return NotFound();
            }

            _context.Recipient.Remove(recipient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}