using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth;
using IDP.Application.Handler.Command;
using IDP.Application.Query.Auth;
using IDP.Domain.IRepository.Query;
using MediatR;

namespace IDP.Application.Handler.Query
{
    public class AuthHandler : IRequestHandler<AuthQuery, JsonWebToken>
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IOtpRedisRepository _otpRedisRepository;
        private IUserQueryRepository _userQueryRepository;

        public AuthHandler(IJwtHandler jwtHandler, IOtpRedisRepository otpRedisRepository, IUserQueryRepository userQueryRepository)
        {
            _jwtHandler = jwtHandler;
            _otpRedisRepository = otpRedisRepository;
            _userQueryRepository = userQueryRepository;
        }

        public async Task<JsonWebToken> Handle(AuthQuery request, CancellationToken cancellationToken)
        {
            var res = await _otpRedisRepository.Getdata(request.MobileNumber);
            if (res == null) return null;
            if (res.OtpCode == request.OtpCode)
            {
                var user = await _userQueryRepository.GetUserAsync(request.MobileNumber);
                var token = _jwtHandler.Create(user.ID);
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}