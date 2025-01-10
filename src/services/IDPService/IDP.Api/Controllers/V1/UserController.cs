﻿using IDP.Api.Controllers.BaseController;
using IDP.Application.Command.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IDP.Api.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
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
        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody] UserCommand userCommand)
        {
            var res = await _mediator.Send(userCommand);
            return Ok(res);
        }
    }
}