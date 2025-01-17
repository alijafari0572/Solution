using IDP.Domain.IRepository.Command.Base;
using IDP.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Infra.Repository.Command.Base
{
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        private readonly CommandDBContext _commandDbContext;

        public CommandRepository(CommandDBContext commandDbContext)
        {
            _commandDbContext = commandDbContext;
        }

        public async Task<T> Insert(T entity)
        {
            await _commandDbContext.Set<T>().AddAsync(entity);
            await _commandDbContext.SaveChangesAsync();
            return entity;
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}