using Microsoft.AspNetCore.Mvc;
using DistributionSystemApi.Interfaces;
using DistributionSystemApi.DistributionSystemApi.Services.Models;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Requests;
using AutoMapper;

namespace DistributionSystemApi.Controllers
{
    [Route("api/Recipient")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipientService _recipientService;
        private readonly IMapper _mapper;

        public RecipientController(IRecipientService recipientService, IMapper mapper)
        {
            _recipientService = recipientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationPage<RecipientResponse>>> GetRecipients(uint page = 1, uint pageSize = 10, CancellationToken cancellationToken = default)
        {
            var pageResult = await _recipientService.GetRecipients(page, pageSize, cancellationToken);
            var response = _mapper.Map<PaginationPage<RecipientResponse>>(pageResult);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientResponse>> GetRecipient(Guid id, CancellationToken cancellationToken)
        {
            var recipient = await _recipientService.GetRecipient(id, cancellationToken);

            if (recipient == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<RecipientResponse>(recipient);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<RecipientResponse>> CreateRecipient(CreateRecipientRequest request, CancellationToken cancellationToken)
        {
            var serviceModel = _mapper.Map<CreateRecipient>(request);
            var createdRecipientId = await _recipientService.CreateRecipient(serviceModel, cancellationToken);
            var createdRecipient = await _recipientService.GetRecipient(createdRecipientId, cancellationToken);

            if (createdRecipient == null)
            {
                return BadRequest();
            }

            var response = _mapper.Map<RecipientResponse>(createdRecipient);
            return CreatedAtAction("GetRecipient", new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipient(Guid id, CreateRecipientRequest request, CancellationToken cancellationToken)
        {
            var serviceModel = _mapper.Map<CreateRecipient>(request);
            await _recipientService.UpdateRecipient(id, serviceModel, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipient(Guid id, CancellationToken cancellationToken)
        {
            await _recipientService.DeleteRecipient(id, cancellationToken);

            return NoContent();
        }
    }
}