using Microsoft.AspNetCore.Mvc;
using DistributionSystemApi.Interfaces;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Requests;
using AutoMapper;
using DistributionSystemApi.Services.Models;

namespace DistributionSystemApi.Controllers
{
    [Route("api/RecipientGroup")]
    [ApiController]
    public class RecipientGroupController : ControllerBase
    {
        private readonly IRecipientGroupService _recipientGroupService;
        private readonly IMapper _mapper;

        public RecipientGroupController(IRecipientGroupService recipientGroupService, IMapper mapper)
        {
            _recipientGroupService = recipientGroupService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipientGroupResponse>>> GetRecipientGroups(CancellationToken cancellationToken)
        {
            var recipientGroups = await _recipientGroupService.GetRecipientGroups(cancellationToken);
            var responseModels = _mapper.Map<List<RecipientGroupResponse>>(recipientGroups);
            return responseModels;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientGroupResponse>> GetRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _recipientGroupService.GetRecipientGroup(id, cancellationToken);

            if (recipientGroup == null)
            {
                return NotFound();
            }

            var responseModel = _mapper.Map<RecipientGroupResponse>(recipientGroup);
            return responseModel;
        }

        [HttpPost]
        public async Task<ActionResult<CreateRecipientGroupRequest>> CreateRecipientGroup(CreateRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            var serviceModel = _mapper.Map<CreateRecipientGroup>(request);
            var createdRecipientGroup = await _recipientGroupService.CreateRecipientGroup(serviceModel, cancellationToken);

            var responseModel = _mapper.Map<RecipientGroupResponse>(createdRecipientGroup);
            return CreatedAtAction("GetRecipientGroup", new { id = responseModel.Id }, responseModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipientGroup(Guid id, CreateRecipientGroupRequest recipientGroup, CancellationToken cancellationToken)
        {
            var serviceModel = _mapper.Map<CreateRecipientGroup>(recipientGroup);
            await _recipientGroupService.UpdateRecipientGroup(id, serviceModel, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            await _recipientGroupService.DeleteRecipientGroup(id, cancellationToken);

            return NoContent();
        }
    }
}