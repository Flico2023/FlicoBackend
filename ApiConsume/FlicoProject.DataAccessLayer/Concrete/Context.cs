using FlicoProject.BusinessLayer.Concrete;
using FlicoProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.DataAccessLayer.Concrete
{
    public class Context:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Initial Catalog=FlicoDb;Integrated Security=True").EnableSensitiveDataLogging(); 
        }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockDetail> StockDetails { get; set; }
        public DbSet<Closet> Closets { get; set; }
        public DbSet<Outsource> Outsources { get; set; }
        public DbSet<OutsourceProduct> OutsourceProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

    }
}
