using CMSCodeTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCodeTest.DBContext
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options) {}
        public DbSet<Users> User { get; set; }
        public DbSet<eVoucher> eVoucher { get; set; }
        public DbSet<eVoucherViewModel> eVoucherViewModel { get; set; }
        public DbSet<Listing> Listing { get; set; }
    }
}
