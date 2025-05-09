using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.DataModels
{
    public class DbContextFactory : IDesignTimeDbContextFactory<PizzeriaApiDBContext>
    {
        public PizzeriaApiDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PizzeriaApiDBContext>();

            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=PizzeriaDB;Integrated Security=SSPI; TrustServerCertificate=True;");

            return new PizzeriaApiDBContext(optionsBuilder.Options);
        }
    }
}
