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
        public string smCavityNo { get; set; }
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

        public string smTypeCavity { get; set; }

        public string smIcsName { get; set; } //30/06/2025

        public bool smReqStatus { get; set; } = true; //30/06/2025


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
        public string mpType { get; set; } //subMaker,MoldModify


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
        public string mmType { get; set; } //subMaker,MoldModify
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
        public string v_DocumentNo { get; set; }
        public string v_OrderNo { get; set; }

        public string v_LotNo { get; set; }
        public string v_MoldNo { get; set; }
        public string v_CusName { get; set; }
        public string v_MoldName { get; set; }
        public string v_MoldMass { get; set; }
        public string v_ModelName { get; set; }
        public string v_CavityNo { get; set; }
        public string v_Function { get; set; }
        public string v_DevelopmentStage { get; set; }
        public string v_MaterialOutDateFrom { get; set; }
        public string v_MaterialOutDateTo { get; set; }
        public string v_DaliverryDateFrom { get; set; }
        public string v_DaliverryDateTo { get; set; }

        public string v_DateIssueFrom { get; set; }
        public string v_DateIssueTo { get; set; }

        public string v_status { get; set; }

        public string v_TypeofCavity { get; set; }
        public string v_Revision { get; set; }

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
    public class ViewOperaterCP
    {
        public string IssueBy { get; set; }
        public string CheckedByTL { get; set; }
        public string CheckedByTM { get; set; }
        public string ApproveBy { get; set; }

        public string empIssueBy { get; set; }
        public string empCheckedByTL { get; set; }
        public string empCheckedByTM { get; set; }
        public string empApproveBy { get; set; }
    }



    //Modify table
    [Table("ceMastModifyRequest")]
    public class ViewceMastModifyRequest
    {
        [Key]
        public string mfCENo { get; set; }
        public string mfRevision { get; set; }
        public string mfRefNo { get; set; }
        public string mfLotNo { get; set; }

        public string mfIssueRate { get; set; }
        public string mfCostRate { get; set; }
        public string mfCostType { get; set; }
        public string mfType { get; set; }
        public double mfESTCost { get; set; }
        public double mfResultCost { get; set; }
        public double mfMKPrice { get; set; }
        public string mfCustomerName { get; set; }
        public string mfMoldNoOrMoldName { get; set; }
        public string mfFunction { get; set; }
        public string mfModelName { get; set; }
        public string mfMoldMass { get; set; }
        public int mfCavityNo { get; set; }
        public string mfTypeCavity { get; set; }
        public int mfLeadTime { get; set; }
        public double mfLabourCost { get; set; }
        public double mfDPCost { get; set; }
        public double mfMECost { get; set; }
        public double mfCostUntilSH { get; set; }
        public double mfMatCost { get; set; }
        public double mfRateCostUntilSH { get; set; }
        public double mfTotalESTCost { get; set; }
        public double mfRoundupotalESTCost { get; set; }
        public string mfDetail { get; set; }
        public string mfProcess { get; set; }
        public string mfIssueDate { get; set; }
        public string mfIssueDept { get; set; }
        public string mfEmpCodeRequest { get; set; }
        public string mfNameRequest { get; set; }
        public string mfEmpCodeApprove { get; set; }
        public string mfNameApprove { get; set; }
        public int mfFlowNo { get; set; }
        public int mfStep { get; set; }
        public string mfStatus { get; set; }

        public double mfMtTool { get; set; }
        public double mfTotalMt { get; set; }


        public string mfIcsname { get; set; } //30/06/2025 


    }
    [Table("ceItemModifyRequest")]
    public class ViewceItemModifyRequest
    {
        [Key]
        public string imCENo { get; set; }
        public int imItemNo { get; set; }
        public string imItemName { get; set; }
        public double imPCS { get; set; }
        public double imAmount { get; set; }
    }




    [Table("ceHourChangeCategory")]
    public class ViewceHourChangeCategory
    {
        [Key]
        public int hcId { get; set; }
        public string hcYear { get; set; }
        public int hcRev { get; set; } //vision
        public string hcGroupMain { get; set; }
        public string hcGroupSub { get; set; }
        public string hcProcessName { get; set; }
        public string hcIssue { get; set; }

    }

    public class GroupViewceHourChangeCategory
    {
        public string GroupName { get; set; }
        public List<ViewceHourChangeCategory> ceHourChangeCategory { get; set; }
    }

    public class GroupViewceHourChangeEntry
    {
        public string GroupName { get; set; }
        public List<ViewceHourChangeEntry> ceHourChangeEntry { get; set; }
    }


    public class GroupedResult
    {
        public string HcGroupMain { get; set; }
        public List<ProcessResult> Processes { get; set; }
    }

    public class ProcessResult
    {
        public string HcGroupMain { get; set; }
        public string HcProcessName { get; set; }
        public string HcType { get; set; }
        public List<ViewceHourChangeEntry> SubItems { get; set; }
    }

    public class ProcessResultType
    {
        public string HcProcessName { get; set; }
        public string HcType { get; set; }
        public List<ViewceHourChangeEntry> SubItems { get; set; }
    }


    public class GroupedResult1
    {
        public string HcGroupMain { get; set; }
        public List<ProcessResult1> Processes { get; set; }
    }

    public class ProcessResult1
    {
        public string HcProcessName { get; set; }

        // Structure: Type ("PLAN", "RESULTS/FORECAST") => Month => Amount
        public Dictionary<string, Dictionary<string, decimal>> MonthlyData { get; set; }
    }

    public class ViewceHourChangeEntryMonth
    {
        [Key]
        //public int heId { get; set; }
        public string heYear { get; set; }
        public string heMonth { get; set; }
        public string heProcessName { get; set; }
        public string heType { get; set; }
        public double valueAmount1 { get; set; }
        public double valueAmount2 { get; set; }
        public double valueAmount3 { get; set; }
        public double valueAmount4 { get; set; }
        public double valueAmount5 { get; set; }
        public double valueAmount6 { get; set; }
        public double valueAmount7 { get; set; }
        public double valueAmount8 { get; set; }
        public double valueAmount9 { get; set; }
        public double valueAmount10 { get; set; }
        public double valueAmount11 { get; set; }
        public double valueAmount12 { get; set; }

    }


    public class GroupMain
    {
        public string HcGroupMain { get; set; }
        public List<ProcessResult> Processes { get; set; }
    }

    public class GroupSub
    {
        public string HcProcessName { get; set; }
        public string HcType { get; set; }
        public List<GroupSubMonth> SubMont { get; set; }
    }
    public class GroupSubMonth
    {
        public string HcProcessName { get; set; }
        public string HcType { get; set; }
        public List<ViewceHourChangeEntry> ceHourChangeEntry { get; set; }
    }

    [Table("ceHourChangeEntry")]
    public class ViewceHourChangeEntry
    {
        [Key]
        public int heId { get; set; }
        public string heYear { get; set; }
        public int heRev { get; set; }
        public string heMonth { get; set; }
        public string heGroupMain { get; set; }
        public string heProcessName { get; set; }
        public string heType { get; set; }
        public double heAmount { get; set; }
    }

    public class MonthData
    {
        public string HeMonth { get; set; }
        public double HeAmount { get; set; }
    }



    public class GroupMainData
    {
        public string HcGroupMain { get; set; }
        public List<GroupedData> Processes { get; set; }
    }

    public class GroupedData
    {
        public string HcGroupMain { get; set; }
        public string HeProcessName { get; set; }
        public string HeType { get; set; }
        public List<MonthData> Months { get; set; }
    }

    public class MlistMonth
    {
        public string Month { get; set; }

    }
    public class typeDetail
    {
        public string type { get; set; }

    }

    public class YeartHourChange
    {
        public string year { get; set; }
        public int Rev { get; set; }
        public string issueDate { get; set; }

    }

    [Table("ceMastHourChage")]
    public class ViewceMastHourChage
    {
        [Key]
        public int mhId { get; set; }
        public string mhGroupMain { get; set; }
        public string mhGroupSub { get; set; }
        public string mhProcessName { get; set; }

    }

    [Table("ceMastType")]
    public class ViewceMastType
    {
        [Key]
        public int mtId { get; set; }
        public string mtName { get; set; }
        public string mtType { get; set; }
        public string mtProgram { get; set; }
        public string mtIssueBy { get; set; }

    }
    public class ViewSearchHisSum
    {
        public string s_LotNoMoldName { get; set; }
    }

    public class GroupedListceDetailSubMakerRequest
    {
        public string glDocNo { get; set; }
        public List<ViewceDetailSubMakerRequest> gllistDetail { get; set; }
    }

    public class GroupedListceDetailSub
    {
        public string glDocNo { get; set; }
        public List<GroupedListceDetailSubMakerRequest> listGroupViewceDetailSubMakerRequest { get; set; }
    }

    public class VisaulViewceDetailSubMakerRequest
    {
        public string dsDocumentNo { get; set; }
        public int dsRunNo { get; set; }
        public string dsLotNo { get; set; }
        public string dsOrderNo { get; set; }
        public string dsGroupName { get; set; }
        public string dsProcessName { get; set; }
        public double dsKIJUNWT_Man { get; set; }
        public double dsKIJUNWT_Auto { get; set; }
        public double dsWT_Man { get; set; }
        public double dsWT_Auto { get; set; }
    }
    public class GroupViewceDetailSubMakerRequestHissum
    {
        public string GroupName { get; set; }
        public List<VisaulViewceDetailSubMakerRequest> DetailSubMakerRequest { get; set; }
    }

    [Table("ceMastSubHistorySum")]
    public class ViewceMastSubHistorySum
    {
        [Key]
        public string shDocNo { get; set; }
        public string shLotNo { get; set; }
        public double shCKjMat { get; set; } 
        public double shCKjCofficient { get; set; }
        public double shCKjWorkingTime { get; set; } 
        public double shCKjTotal { get; set; } 
        public double shCSmMat { get; set; } 
        public double shCSmCofficient { get; set; } 
        public double shCSmWorkingTime { get; set; } 
        public double shCSmTotal { get; set; }
        public double shCMcMat { get; set; }
        public double shCMcCofficient { get; set; } 
        public double shCMcWorkingTime { get; set; }
        public double shCMcTotal { get; set; }
        public bool shStatus { get; set; } = false;
        public string shIssueBy { get; set; }
    }

    [Table("ceMastSubDetailHistorySum")]
    public class ViewceMastSubDetailHistorySum
    {
        [Key]

        public string sdDocNo { get; set; }
        public int sdRunNo { get; set; }
        public string sdLotNo { get; set; }
        public string sdGroupName { get; set; }
        public string sdProcessName { get; set; }
        public double sdWK_Man { get; set; }
        public double sdWK_Auto { get; set; }
        public bool sdActive_WKMan { get; set; }
        public bool sdActive_WKAuto { get; set; }
        public double sdKIJWT_Man { get; set; }
        public double sdKJWT_Auto { get; set; }
        public double sdWT_Man { get; set; }
        public double sdWT_Auto { get; set; }


    }
    public class GroupDetailSubMakerRequestHissum
    {
        public string GroupName { get; set; }
        public List<ViewceMastSubDetailHistorySum> DetailSubMakerRequest { get; set; }
    }

}
