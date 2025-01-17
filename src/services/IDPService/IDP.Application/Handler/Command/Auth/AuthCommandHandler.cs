using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Application.Command.Auth;
using IDP.Application.DTO;
using MediatR;
using AutoMapper;
using DotNetCore.CAP;
using EventMessages.Events;
using IDP.Domain.IRepository.Command;
using IDP.Domain.IRepository.Query;
using MassTransit;

namespace IDP.Application.Handler.Command.Auth
{
    public class AuthCommandHandler : IRequestHandler<AuthCommand, bool>
    {
        private readonly IOtpRedisRepository _otpRedisRepository;
        private readonly IMapper _autoMapper;
        private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly ICapPublisher _capPublisher;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuthCommandHandler(IOtpRedisRepository otpRedisRepository, IMapper autoMapper, IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository, ICapPublisher capPublisher, IPublishEndpoint publishEndpoint)
        {
            _otpRedisRepository = otpRedisRepository;
            _autoMapper = autoMapper;
            _userCommandRepository = userCommandRepository;
            _userQueryRepository = userQueryRepository;
            _capPublisher = capPublisher;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userobj = _autoMapper.Map<Domain.Entities.User>(request);
                var user = await _userQueryRepository.GetUserAsync(request.MobileNumber);
                if (user == null)
                {
                    Random random = new Random();
                    var code = random.Next(1000, 10000);
                    //ارسال پیامک
                    //CAP
                    //await _capPublisher.PublishAsync<AuthCommand>("otpevent", new AuthCommand
                    //{
                    //    MobileNumber = request.MobileNumber
                    //});
                    //Masstransit
                    await _publishEndpoint.Publish<OtpEvent>(new OtpEvent()
                    {
                        //CreateDate = DateTime.Now,
                        MobileNumber = request.MobileNumber,
                        OtpCode = code.ToString()
                    });
                    var res = await _userCommandRepository.Insert(userobj);
                    _otpRedisRepository.Insert(new Otp { UserId = res.ID, OtpCode = "500", IsUse = false });
                    return true;
                }
                else
                {
                    Random random = new Random();
                    var code = random.Next(1000, 10000);
                    //ارسال پیامک
                    await _capPublisher.PublishAsync<AuthCommand>("otpevent", new AuthCommand
                    {
                        MobileNumber = request.MobileNumber
                    });

                    userobj.UserName = request.MobileNumber;
                    _otpRedisRepository.Insert(new Otp { UserId = user.ID, OtpCode = "500", IsUse = false });
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}