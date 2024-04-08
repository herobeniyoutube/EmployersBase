using EmployersBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployersBase
{
    public class BaseContext : DbContext
    {
        public DbSet<OrganizationsEntity> Organizations { get; set; }

        public BaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=OrganizationsBase;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
