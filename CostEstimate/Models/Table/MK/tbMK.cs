using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Models.Table.MK
{
    [Table("Login")]
    public class ViewLoginPgm
    {
        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Program { get; set; }
        public string Empcode { get; set; }
        public string Permission { get; set; }

    }
    [Table("ceMastSubMakerRequest")]
    public class ViewceMastSubMakerRequest
    {
        [Key]
        public string smDocumentNo { get; set; }
        public string smLotNo { get; set; }
        public string smOrderNo { get; set; }
        public string smRevision { get; set; }
        public string smCustomerName { get; set; }
        public string smMoldName { get; set; }
        public string smModelName { get; set; }
        public int smCavityNo { get; set; }
        public string smFunction { get; set; }
        public string smDevelopmentStage { get; set; }
        public string smMoldNo { get; set; }
        public string smMatOutDate { get; set; }
        public string smMatOutTime { get; set; }
        public string smDeliveryDate { get; set; }
        public string smDeliveryTime { get; set; }
        public string smRemark { get; set; }
        public string smRemarkOther { get; set; }
        public string smDetail { get; set; }
        public string smWeight { get; set; }
        public int smQty { get; set; }
        public double smTotalProcessWT { get; set; }
        public double smTotalProcessCost { get; set; }
        public double smOrderMatl { get; set; }
        public double smTotalCost { get; set; }
        public double smRoundUp { get; set; }
        public string smIssueDate { get; set; }
        public string smIssueDept { get; set; }
        public string smEmpCodeRequest { get; set; }
        public string smNameRequest { get; set; }
        public string smEmpCodeApprove { get; set; }
        public string smNameApprove { get; set; }
        public int smFlowNo { get; set; }
        public int smStep { get; set; }
        public string smStatus { get; set; }
        public double smTotalProCost { get; set; }

    }

    [Table("ceDetailSubMakerRequest", Schema = "dbo")]
    public class ViewceDetailSubMakerRequest
    {
        [Key]
        public string dsDocumentNo { get; set; }
        public int dsRunNo { get; set; }
        public string dsLotNo { get; set; }
        public string dsOrderNo { get; set; }
        public string dsRevision { get; set; }
        public string dsGroupName { get; set; }
        public string dsProcessName { get; set; }
        public double dsWT_Man { get; set; }
        public double dsWT_Auto { get; set; }

        public bool dsEnable_WTMan { get; set; }
        public bool dsEnable_WTAuto { get; set; }

        public double dsLabour_Rate { get; set; }
        public double dsLabour_Cost { get; set; }
        public double dsDP_Rate { get; set; }
        public double dsDP_Cost { get; set; }
        public double dsME_Rate { get; set; }
        public double dsME_Cost { get; set; }
        public double dsTotalCost { get; set; }


    }
    public class GroupViewceDetailSubMakerRequest
    {
        public string GroupName { get; set; }
        public List<ViewceDetailSubMakerRequest> DetailSubMakerRequest { get; set; }
    }
    public class GroupViewCostPlanning
    {
        public string GroupName { get; set; }
        public List<ViewceCostPlanning> ceCostPlanning { get; set; }
    }

    public class GroupViewceMastCostModel
    {
        public string CostPlanningNo { get; set; }
        public string Description { get; set; }

    }



    [Table("ceMastProcess")]
    public class ViewceMastProcess
    {
        [Key]
        public int mpNo { get; set; }
        public string mpGroupName { get; set; }
        public string mpProcessName { get; set; }
        public bool mpEnable_WTMan { get; set; }
        public bool mpEnable_WTAuto { get; set; }
        public string mpIssueBy { get; set; }
        public string mpUpdateBy { get; set; }

    }
    [Table("ceCostPlanning")]
    public class ViewceCostPlanning
    {
        [Key]
        public string cpCostPlanningNo { get; set; }
        public int cpNo { get; set; }
        public string cpDescription { get; set; }
        public string cpGroupName { get; set; }
        public string cpProcessName { get; set; }
        public double cpLabour_Rate { get; set; }
        // public float cpLabour_Cost { get; set; }
        public double cpDP_Rate { get; set; }
        // public float cpDP_Cost { get; set; }
        public double cpME_Rate { get; set; }
        //public float cpME_Cost { get; set; }
        public string cpIssueBy { get; set; }
        public string cpUpdateBy { get; set; }

    }
    [Table("ceMastCostModel")]
    public class ViewceMastCostModel
    {
        [Key]
        public string mcCostPlanningNo { get; set; }
        public string mcModelName { get; set; }
        public string mcDescription { get; set; }
        public string mcIssueBy { get; set; }
        public string mcUpdateBy { get; set; }
    }

    public class ViewGroupbyceMastCostModel
    {
        [Key]

        public string mcCostPlanningNo { get; set; }
        public string mcDescription { get; set; }

    }

    [Table("ceMastModel")]
    public class ViewceMastModel
    {
        [Key]

        public int mmNo { get; set; }
        public string mmModelName { get; set; }
        public string mcIssueBy { get; set; }
        public string mcUpdateBy { get; set; }
    }

    [Table("ceMastFlowApprove")]
    public class ViewceMastFlowApprove
    {
        [Key]
        public string mfFlowNo { get; set; }
        public int mfStep { get; set; }
        public string mfSubject { get; set; }
        public string mfTo { get; set; }
        public string mfCC { get; set; }
        public string mfIssueBy { get; set; }
        public string mfUpdateBy { get; set; }



    }
    [Table("ceHistoryApproved")]
    public class ViewceHistoryApproved
    {
        [Key]

        public int htNo { get; set; }
        public string htDocNo { get; set; }
        public int htStep { get; set; }
        public string htStatus { get; set; }
        public string htFrom { get; set; }
        public string htTo { get; set; }
        public string htCC { get; set; }
        public string htDate { get; set; }
        public string htTime { get; set; }
        public string htRemark { get; set; }



    }


    [Table("ceRunDocument")]
    public class ViewceRunDocument
    {
        [Key]
        public int rmRunNo { get; set; }
        public string rmDocCode { get; set; }
        public string rmDocSub { get; set; }
        public string rmYear { get; set; }
        public string rmMonth { get; set; }
        public string rmIssueBy { get; set; }
        public string rmUpdateBy { get; set; }


    }
    public class ViewSearchData
    {
        public string v_DocumentNo{ get; set; }
      
        public string v_LotNo { get; set; }
        public string v_MoldNo { get; set; }
        public string v_CusName { get; set; }
        public string v_MoldName { get; set; }
        public string v_ModelName { get; set; }
        public string v_CavityNo { get; set; }
        public string v_Function { get; set; }
        public string v_DevelopmentStage { get; set; }
        public string v_MaterialOutDateFrom { get; set; }
        public string v_MaterialOutDateTo { get; set; }
        public string v_DaliverryDateFrom { get; set; }
        public string v_DaliverryDateTo { get; set; }
        public string v_status { get; set; }

    }

    public class ViewNewRequestprocess
    {
        public string v_cpGroupName { get; set; }
        public string v_cpProcessName { get; set; }
        public float dsWT_Man { get; set; }
        public float dsWT_Auto { get; set; }
        public float dsLabour_Rate { get; set; }
        public float dsLabour_Cost { get; set; }
        public float dsDP_Rate { get; set; }
        public float dsDP_Cost { get; set; }
        public float dsME_Rate { get; set; }
        public float dsME_Cost { get; set; }


    }

    [Table("ceRunCostpalnning")]
    public class ViewcceRunCostpalnning
    {
        [Key]

        public int rcRunNo { get; set; }
        public string rcDocCode { get; set; }
        public string rcYear { get; set; }
        public string rcIssueBy { get; set; }
        public string rcUpdateBy { get; set; }
    }

}
