using IDP.Infra.Repository.Command.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Domain.Entities;
using IDP.Domain.IRepository.Command.Base;
using IDP.Domain.IRepository.Command;
using IDP.Infra.Data;

namespace IDP.Infra.Repository.Command
{
    public class UserCommandRepository : CommandRepository<User>, IUserCommandRepository
    {
        // private readonly CommandDBContext _commandDbContext;

        public UserCommandRepository(CommandDBContext commandDbContext) : base(commandDbContext)
        {
        }
    }
}