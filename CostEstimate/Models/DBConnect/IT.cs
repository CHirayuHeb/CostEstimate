using Microsoft.EntityFrameworkCore;
using CostEstimate.Models.Table.IT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.DBConnect
{
    public class IT : DbContext
    {
        public IT(DbContextOptions<IT> options) : base(options){ }

        public DbSet<ViewrpEmail> rpEmails { get; set; }
        public DbSet<ViewAttachment> Attachment { get; set; }
        public List<ViewAttachment> _listAttachment { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ViewAttachment>(entity =>
            {
                entity.HasKey(k => new { k.fnNo, k.fnPath });
            });
            
        }
    }
}
