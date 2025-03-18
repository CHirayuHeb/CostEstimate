using Microsoft.EntityFrameworkCore;
using CostEstimate.Models.Table.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.DBConnect
{
    public class HRMS : DbContext
    {
        public HRMS(DbContextOptions<HRMS> options) : base(options)
        { }

        public DbSet<ViewAccEMPLOYEE> AccEMPLOYEE { get; set; }
        public DbSet<ViewAccDeptMast> AccDEPTMAST { get; set; }
        public DbSet<ViewAccDIVIMAST> AccDIVIMAST { get; set; }
        public DbSet<ViewAccGRPMAST> AccGROMAST { get; set; }
        public DbSet<ViewAccSECMAST> AccSECMAST { get; set; }

        public DbSet<ViewAccPOSMAST> AccPOSMAST { get; set; }
    }
}
