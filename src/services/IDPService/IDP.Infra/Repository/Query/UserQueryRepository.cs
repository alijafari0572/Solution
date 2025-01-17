using IDP.Domain.IRepository.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Domain.Entities;
using IDP.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace IDP.Infra.Repository.Query
{
    public class UserQueryRepository : IUserQueryRepository
    {
        private readonly QueryDBConnection _queryDbConnection;

        public UserQueryRepository(QueryDBConnection queryDbConnection)
        {
            _queryDbConnection = queryDbConnection;
        }

        public async Task<User> GetUserAsync(string mobilenumber)
        {
            return await _queryDbConnection.Users_Tbl.Where(u => u.ID ==Convert.ToInt32( mobilenumber)).FirstOrDefaultAsync();
        }
    }
}