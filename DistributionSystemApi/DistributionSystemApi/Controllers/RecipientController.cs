using Microsoft.AspNetCore.Mvc;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Models;
using DistributionSystemApi.Interfaces;

namespace DistributionSystemApi.Controllers
{
    [Route("api/Recipient")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipientService _recipientService;

        public RecipientController(IRecipientService recipientService)
        {
            _recipientService = recipientService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationPage<RecipientResponse>>> GetRecipients(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var pageResult = await _recipientService.GetRecipients(page, pageSize, cancellationToken);

            return pageResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientResponse>> GetRecipient(Guid id, CancellationToken cancellationToken)
        {
            var recipient = await _recipientService.GetRecipient(id, cancellationToken);

            if (recipient == null)
            {
                return NotFound();
            }

            return recipient;
        }

        [HttpPost]
        public async Task<ActionResult<RecipientResponse>> CreateRecipient(CreateRecipientRequest request, CancellationToken cancellationToken)
        {
            var createdRecipient = await _recipientService.CreateRecipient(request, cancellationToken);
            return CreatedAtAction("GetRecipient", new { id = createdRecipient.Id }, createdRecipient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipient(Guid id, CreateRecipientRequest recipient, CancellationToken cancellationToken)
        {
            var success = await _recipientService.UpdateRecipient(id, recipient, cancellationToken);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipient(Guid id, CancellationToken cancellationToken)
        {
            var success = await _recipientService.DeleteRecipient(id, cancellationToken);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}