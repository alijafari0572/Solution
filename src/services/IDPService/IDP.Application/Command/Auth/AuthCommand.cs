using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace IDP.Application.Command.Auth
{
    public class AuthCommand : IRequest<bool>
    {
        public string MobileNumber { get; set; }
    }
}