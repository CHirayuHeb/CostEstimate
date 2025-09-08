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
            modelBuilder.Entity<ViewceItemModifyRequest>(entity =>
            {
                entity.HasKey(k => new { k.imCENo, k.imItemNo });
            });
            modelBuilder.Entity<ViewceMastSubHistorySum>(entity =>
            {
                entity.HasKey(k => new { k.shDocNo, k.shLotNo });
            });
            modelBuilder.Entity<ViewceMastSubDetailHistorySum>(entity =>
            {
                entity.HasKey(k => new { k.sdDocNo, k.sdRunNo, k.sdLotNo });
            });
            modelBuilder.Entity<ViewceItemPartName>(entity =>
            {
                entity.HasKey(k => new { k.ipDocumentNo, k.ipPartName, k.ipCavityNo, k.ipTypeCavity });
            });
            modelBuilder.Entity<ViewceMastWorkingTimeRequest>(entity =>
            {
                entity.HasKey(k => new { k.wrDocumentNo, k.wrDocumentNoSub });
            });
            modelBuilder.Entity<ViewceMastMaterialRequest>(entity =>
            {
                entity.HasKey(k => new { k.mrDocumentNo, k.mrDocumentNoSub });
            });
            modelBuilder.Entity<ViewceMastToolGRRequest>(entity =>
            {
                entity.HasKey(k => new { k.trDocumentNo, k.trDocumentNoSub });
            });

            modelBuilder.Entity<ViewceMastInforSpacMoldRequest>(entity =>
            {
                entity.HasKey(k => new { k.irDocumentNo, k.irDocumentNoSub });
            });
            modelBuilder.Entity<ViewceItemWorkingTimePartName>(entity =>
            {
                entity.HasKey(k => new { k.wpDocumentNoSub, k.wpRunNo, k.wpPartName, k.wpCavityNo, k.wpTypeCavity });
            });
            modelBuilder.Entity<ViewceItemWorkingTimeSizeProduct>(entity =>
            {
                entity.HasKey(k => new { k.wsDocumentNoSub, k.wsPartName, k.wsCavityNo, k.wsTypeCavity });
            });
            modelBuilder.Entity<ViewceItemMaterialRequestPartName>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.mpDocumentNoSub,
                    k.mpRunNo,
                    k.mpPartName,
                    k.mpCavityNo,
                    k.mpTypeCavity
                });
            });
            modelBuilder.Entity<ViewceItemToolGRRequestPartName>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.tpDocumentNoSub,
                    k.tpRunNo,
                    k.tpPartName,
                    k.tpCavityNo,
                    k.tpTypeCavity
                });
            });
            modelBuilder.Entity<ViewceItemInforRequestPartName>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.ipDocumentNoSub,
                    k.ipRunNo,
                    k.ipPartName,
                    k.ipCavityNo,
                    k.ipTypeCavity
                });
            });
            modelBuilder.Entity<ViewceItemInforSlideSystem>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.isDocumentNoSub,
                    k.isRunNo,
                    k.isPartName,
                    k.isCavityNo,
                    k.isTypeCavity
                });
            });
            modelBuilder.Entity<ViewceItemInforTypeOfCut>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.icDocumentNoSub,
                    k.icRunNo,
                    k.icPartName,
                    k.icCavityNo,
                    k.icTypeCavity
                });
            });

            modelBuilder.Entity<ViewceItemInforShibo>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.ibDocumentNoSub,
                    k.ibRunNo,
                    k.ibPartName,
                    k.ibCavityNo,
                    k.ibTypeCavity
                });
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


        //Mold Modify
        public DbSet<ViewceMastModifyRequest> _ViewceMastModifyRequest { get; set; }
        public DbSet<ViewceItemModifyRequest> _ViewceItemModifyRequest { get; set; }


        //admin mold
        public DbSet<ViewceHourChangeCategory> _ViewceHourChangeCategory { get; set; }
        public DbSet<ViewceHourChangeEntry> _ViewceHourChangeEntry { get; set; }


        public DbSet<ViewceHourChangeEntryMonth> _ViewceHourChangeEntryMonth { get; set; }
        public DbSet<ViewceMastHourChage> _ViewceMastHourChage { get; set; }

        public DbSet<ViewceMastType> _ViewceMastType { get; set; }


        public DbSet<ViewceMastSubHistorySum> _ViewceMastSubHistorySum { get; set; } //for report history sum 
        public DbSet<ViewceMastSubDetailHistorySum> _ViewceMastSubDetailHistorySum { get; set; } //for report history sum 


        //mold Other
        public DbSet<ViewceMastMoldOtherRequest> _ViewceMastMoldOtherRequest { get; set; }
        public DbSet<ViewceItemPartName> _ViewceItemPartName { get; set; }

        //working time
        public DbSet<ViewceMastWorkingTimeRequest> _ViewceMastWorkingTimeRequest { get; set; }
        public DbSet<ViewceItemWorkingTimePartName> _ViewceItemWorkingTimePartName { get; set; }
        public DbSet<ViewceItemWorkingTimeSizeProduct> _ViewceItemWorkingTimeSizeProduct { get; set; }


        //material
        public DbSet<ViewceMastMaterialRequest> _ViewceMastMaterialRequest { get; set; }
        public DbSet<ViewceItemMaterialRequestPartName> _ViewceItemMaterialRequestPartName { get; set; }

        //Tool & GR
        public DbSet<ViewceMastToolGRRequest> _ViewceMastToolGRRequest { get; set; }
        public DbSet<ViewceItemToolGRRequestPartName> _ViewceItemToolGRRequestPartName { get; set; }


        //information spac
        public DbSet<ViewceMastInforSpacMoldRequest> _ViewceMastInforSpacMoldRequest { get; set; }
        public DbSet<ViewceItemInforRequestPartName> _ViewceItemInforRequestPartName { get; set; }
        public DbSet<ViewceItemInforSlideSystem> _ViewceItemInforSlideSystem { get; set; }
        public DbSet<ViewceItemInforTypeOfCut> _ViewceItemInforTypeOfCut { get; set; }
        public DbSet<ViewceItemInforShibo> _ViewceItemInforShibo { get; set; }


        //public DbSet<GroupedResult> _GroupedResult { get; set; }
        //public DbSet<ProcessResult> _ProcessResult { get; set; }

    }
}
