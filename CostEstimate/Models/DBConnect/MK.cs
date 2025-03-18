using Microsoft.EntityFrameworkCore;
using CostEstimate.Models.Table.MK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CostEstimate.Models.DBConnect
{
    public class MK : DbContext
    {
        public MK(DbContextOptions<MK> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewceMastSubMakerRequest>(entity =>
            {
                entity.HasKey(k => new { k.smDocumentNo });
            });
            modelBuilder.Entity<ViewceDetailSubMakerRequest>(entity =>
            {
                entity.HasKey(k => new { k.dsDocumentNo, k.dsRunNo });
            });
            modelBuilder.Entity<ViewceMastCostModel>(entity =>
            {
                entity.HasKey(k => new { k.mcCostPlanningNo, k.mcModelName });
            });
            modelBuilder.Entity<ViewceMastFlowApprove>(entity =>
            {
                entity.HasKey(k => new { k.mfFlowNo, k.mfStep });
            });
            modelBuilder.Entity<ViewceCostPlanning>(entity =>
            {
                entity.HasKey(k => new { k.cpCostPlanningNo, k.cpNo });
            });
            modelBuilder.Entity<ViewceRunDocument>(entity =>
            {
                entity.HasKey(k => new { k.rmRunNo, k.rmDocCode, k.rmDocSub, k.rmYear, k.rmMonth });
            });
        }

        public DbSet<ViewLoginPgm> _ViewLoginPgm { get; set; }
        public DbSet<ViewceMastSubMakerRequest> _ViewceMastSubMakerRequest { get; set; }
        public DbSet<ViewceDetailSubMakerRequest> _ViewceDetailSubMakerRequest { get; set; }
        public DbSet<ViewceMastProcess> _ViewceMastProcess { get; set; }
        public DbSet<ViewceCostPlanning> _ViewceCostPlanning { get; set; }
        public DbSet<ViewceMastCostModel> _ViewceMastCostModel { get; set; }
        public DbSet<ViewceMastModel> _ViewceMastModel { get; set; }
        public DbSet<ViewceMastFlowApprove> _ViewceMastFlowApprove { get; set; }
        public DbSet<ViewceHistoryApproved> _ViewceHistoryApproved { get; set; }
        public DbSet<ViewceRunDocument> _ViewceRunDocument { get; set; }
        public DbSet<ViewcceRunCostpalnning> _ViewcceRunCostpalnning { get; set; }



    }
}
