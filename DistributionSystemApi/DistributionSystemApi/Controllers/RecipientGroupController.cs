using Microsoft.AspNetCore.Mvc;
using DistributionSystemApi.Services;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Interfaces;

namespace DistributionSystemApi.Controllers
{
    [Route("api/RecipientGroup")]
    [ApiController]
    public class RecipientGroupController : ControllerBase
    {
        private readonly IRecipientGroupService _recipientGroupService;

        public RecipientGroupController(IRecipientGroupService recipientGroupService)
        {
            _recipientGroupService = recipientGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipientGroupResponse>>> GetRecipientGroups(CancellationToken cancellationToken)
        {
            var recipientGroups = await _recipientGroupService.GetRecipientGroups(cancellationToken);
            return recipientGroups;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientGroupResponse>> GetRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _recipientGroupService.GetRecipientGroup(id, cancellationToken);

            if (recipientGroup == null)
            {
                return NotFound();
            }

            return recipientGroup;
        }

        [HttpPost]
        public async Task<ActionResult<CreateRecipientGroupRequest>> CreateRecipientGroup(CreateRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            var createdRecipientGroup = await _recipientGroupService.CreateRecipientGroup(request, cancellationToken);
            return CreatedAtAction("GetRecipientGroup", new { id = createdRecipientGroup.Id }, createdRecipientGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipientGroup(Guid id, CreateRecipientGroupRequest recipientGroup, CancellationToken cancellationToken)
        {
            var success = await _recipientGroupService.UpdateRecipientGroup(id, recipientGroup, cancellationToken);

            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var success = await _recipientGroupService.DeleteRecipientGroup(id, cancellationToken);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}