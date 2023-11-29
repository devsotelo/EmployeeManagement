﻿using Employee.Application.Features.Permissions.Commands.CreatePermission;
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

        [Route("api/GetPermissions")]
        [HttpGet(Name = "GetAllEvents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<PermissionVm>>> GetAllEvents()
        {
            var dtos = await _mediator.Send(new GetPermissionListQuery());
            return Ok(dtos);
        }

        [Route("api/RequestPermission")]
        [HttpPost(Name = "AddEvent")]
        public async Task<ActionResult<Guid>> Create([FromBody] CreatePermissionCommand createEventCommand)
        {
            var id = await _mediator.Send(createEventCommand);
            return Ok(id);
        }

        [Route("api/ModifyPermission")]
        [HttpPut(Name = "UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatePermissionCommand updateEventCommand)
        {
            await _mediator.Send(updateEventCommand);
            return NoContent();
        }
    }
}