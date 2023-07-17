using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DistributionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipientGroupController : ControllerBase
    {
        private readonly ContentDistributionSystemContext _context;

        public RecipientGroupController(ContentDistributionSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipientGroup>>> GetRecipientGroups()
        {
            return await _context.RecipientGroup.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientGroup>> GetRecipientGroup(Guid id)
        {
            var recipientGroup = await _context.RecipientGroup.FindAsync(id);

            if (recipientGroup == null)
            {
                return NotFound();
            }

            return recipientGroup;
        }

        [HttpPost]
        public async Task<ActionResult<RecipientGroup>> CreateRecipientGroup(RecipientGroup recipientGroup)
        {
            recipientGroup.Id = Guid.NewGuid();
            _context.RecipientGroup.Add(recipientGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipientGroup", new { id = recipientGroup.Id }, recipientGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipientGroup(Guid id, RecipientGroup recipientGroup)
        {
            if (id != recipientGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipientGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipientGroup(Guid id)
        {
            var recipientGroup = await _context.RecipientGroup.FindAsync(id);

            if (recipientGroup == null)
            {
                return NotFound();
            }

            _context.RecipientGroup.Remove(recipientGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}