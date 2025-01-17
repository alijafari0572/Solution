using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth;
using MediatR;

namespace IDP.Application.Query.Auth
{
    public class AuthQuery : IRequest<JsonWebToken>
    {
        //public string UserName { get; set; }
        //public string Password { get; set; }
        public string MobileNumber { get; set; }

        public string OtpCode { get; set; }
    }
}