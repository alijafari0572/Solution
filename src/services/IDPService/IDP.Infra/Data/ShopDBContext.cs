using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IDP.Infra.Data
{
    public class ShopDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShopDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<User> Users_Tbl { get; set; }
    }
}