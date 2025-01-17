using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Application;
using IDP.Application.DTO;
using IDP.Domain.IRepository.Command.Base;

namespace IDP.Application.Handler.Command
{
    public interface IOtpRedisRepository : ICommandRepository<Otp>
    {
        Task<Otp> Getdata(string mobile);
    }
}