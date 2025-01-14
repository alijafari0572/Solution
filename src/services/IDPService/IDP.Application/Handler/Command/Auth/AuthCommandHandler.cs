using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Application.Command.Auth;
using IDP.Application.DTO;
using MediatR;

namespace IDP.Application.Handler.Command.Auth
{
    public class AuthCommandHandler : IRequestHandler<AuthCommand, bool>
    {
        private readonly IOtpRedisRepository _otpRedisRepository;

        public AuthCommandHandler(IOtpRedisRepository otpRedisRepository)
        {
            _otpRedisRepository = otpRedisRepository;
        }

        public async Task<bool> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            _otpRedisRepository.Insert(new Otp { UserId = 230, OtpCode = "500", IsUse = false });
            return true;
        }
    }
}