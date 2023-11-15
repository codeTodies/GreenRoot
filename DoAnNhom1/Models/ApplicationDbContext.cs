using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace DoAnNhom1.Models
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Tree> Trees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderPro> OrderPro { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<Info> Infos { get; set; }

        public System.Data.Entity.DbSet<DoAnNhom1.Models.QuantityTree> QuantityTrees { get; set; }
    }
}