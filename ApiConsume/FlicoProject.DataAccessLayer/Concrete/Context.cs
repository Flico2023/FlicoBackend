using FlicoProject.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.Concrete
{
    public class Context:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Initial Catalog=FlicoDb;Integrated Security=True");
        }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
    }
}
