using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace CostEstimate.Models.Table.MOLD
{
    [Table("mtMaster_Mold_Control")]
    public class ViewmtMaster_Mold_Control
    {
        [Key]
        public int mcNo { get; set; }
        public string mcMold_Control { get; set; }


        public string mcLedger_Number { get; set; }
        public string mcCUS { get; set; }
        public string mcMoldname { get; set; }
        public string mcCavity { get; set; }


    }
}
