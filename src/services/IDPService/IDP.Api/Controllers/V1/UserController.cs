﻿using Asp.Versioning;
using IDP.Api.Controllers.BaseController;
using IDP.Application.Command.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IDP.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class UserController : IBaseController
    {
        public readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// ورود اطلاعات کاربر
        /// </summary>
        /// <returns></returns>
       // [MapToApiVersion(1)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody] UserCommand userCommand)
        {
            var res = await _mediator.Send(userCommand);
            return Ok(res);
        }
    }
}