using CostEstimate.Models.Table.MOLD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.DBConnect
{
    public class MOLD: DbContext
    {
        public MOLD(DbContextOptions<MOLD> options) : base(options)
        { }
        public DbSet<ViewmtMaster_Mold_Control> _ViewmtMaster_Mold_Control { get; set; }
    }
}
