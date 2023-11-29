using Employee.Application.Features.Permissions.Commands.CreatePermission;
using Employee.Application.Features.Permissions.Commands.UpdatePermission;
using Employee.Application.Features.Permissions.Queries.GetPermissionsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Api.Controllers
{
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllEvents", Name = "GetAllEvents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<PermissionVm>>> GetAllEvents()
        {
            var dtos = await _mediator.Send(new GetPermissionListQuery());
            return Ok(dtos);
        }

        [HttpPost("RequestPermission", Name = "RequestPermission")]
        public async Task<ActionResult<Guid>> RequestPermission([FromBody] CreatePermissionCommand createEventCommand)
        {
            var id = await _mediator.Send(createEventCommand);
            return Ok(id);
        }

        [HttpPut("ModifyPermission", Name = "ModifyPermission")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> ModifyPermission([FromBody] UpdatePermissionCommand updateEventCommand)
        {
            await _mediator.Send(updateEventCommand);
            return NoContent();
        }
    }
}
