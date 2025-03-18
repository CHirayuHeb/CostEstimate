using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.Table.IT
{
    [Table("rpEmail")]
    public class ViewrpEmail
    {
        [Key]
        public string emEmpcode { get; set; }
        public string emEmail { get; set; }
        public string emDeptCode { get; set; }
        public string emEmail_M365 { get; set; }
        public string emDept_M365 { get; set; }
        public string emName_M365 { get; set; }
    }
    [Table("Attachment")]
    public class ViewAttachment
    {
      
        public string fnNo { get; set; }
        [Key]
        public string fnPath { get; set; }
        public string fnFilename { get; set; }
        public string fnIssueBy { get; set; }
        public string fnUpdateBy { get; set; }
        public string fnType { get; set; }
        public string fnProgram { get; set; }

    }

}
