using Microsoft.AspNetCore.Mvc;
using CostEstimate.Models.Common;
using CostEstimate.Models.Table.LAMP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.New
{
    public class autocompleteEmpCode
    {
        public string EmpCode { get; set; }
        public string FullNameAndDept { get; set; }

    }

    public class autocompleteEmail
    {
        public string Mail { get; set; }
        public string EmpCode { get; set; }
        public string FullNameAndDept { get; set; }
    }
}
