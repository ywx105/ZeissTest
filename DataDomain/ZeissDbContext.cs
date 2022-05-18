using DataDomain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain
{
    public class ZeissDbContext : DbContext
    {
        public ZeissDbContext(DbContextOptions<ZeissDbContext> options)
           : base(options)
        {
        }

        public DbSet<MachinesPayLoad> MachinesReponse { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ZeissDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
