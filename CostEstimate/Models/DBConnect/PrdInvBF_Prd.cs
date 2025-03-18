using Microsoft.EntityFrameworkCore;
using CostEstimate.Models.Table.PrdInvBF_Prd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.DBConnect
{
    public class PrdInvBF_Prd : DbContext
    {
        public PrdInvBF_Prd(DbContextOptions<PrdInvBF_Prd> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewProdLine>(entity =>
            {
                entity.HasKey(k => new { k.prodline, k.plant });
            });
        }

        public DbSet<ViewProdLine> ProdLines { get; set; }
    }
}
