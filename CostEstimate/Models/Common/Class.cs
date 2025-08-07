using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MK;
using CostEstimate.Models.Table.MOLD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CostEstimate.Models.Common
{
    public class Class
    {
        public static string perAdmin = "admin";
        public ViewLogin _ViewLogin { get; set; }
        public Error _Error { get; set; }
        public string param { get; set; }
        public string paramtype { get; set; }

        public string paramCostNo { get; set; }
        public string paramCostDes { get; set; }

        public string paramDateIssue { get; set; }

        //MK
        public ViewGroupbyceMastCostModel _ViewGroupbyceMastCostModel { get; set; }
        public List<ViewGroupbyceMastCostModel> _ListViewGroupbyceMastCostModel { get; set; }
        public List<GroupViewceMastCostModel> _ListGroupViewceMastCostModel { get; set; }
        public GroupViewceMastCostModel _GroupViewceMastCostModel { get; set; }


        public ViewLoginPgm _ViewLoginPgm { get; set; }
        public ViewSearchData _ViewSearchData { get; set; }

        public ViewSearchHisSum _ViewSearchHisSum { get; set; } //for search history sum 09/07/2025

        


        public List<ViewceMastSubMakerRequest> _ListceMastSubMakerRequest { get; set; }
        public List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest { get; set; }
        public List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequestHisSum { get; set; }
        public List<ViewceMastProcess> _ListceMastProcess { get; set; }
        public List<ViewceCostPlanning> _ListceCostPlanning { get; set; }
        public List<ViewceMastCostModel> _ListceMastCostModel { get; set; }
        public List<ViewceMastModel> _ListceMastModel { get; set; }
        public List<ViewceMastFlowApprove> _ListceMastFlowApprove { get; set; }
        public List<ViewceHistoryApproved> _ListceHistoryApproved { get; set; }
        public List<ViewceRunDocument> _ListViewceRunDocument { get; set; }
        public List<ViewcceRunCostpalnning> _ListViewcceRunCostpalnning { get; set; }
        public List<ViewceCostPlanning> _ListViewceCostPlanning { get; set; }

        public ViewceMastSubMakerRequest _ViewceMastSubMakerRequest { get; set; }
        public ViewceDetailSubMakerRequest _ViewceDetailSubMakerRequest { get; set; }
        public ViewceMastProcess _ViewceMastProcess { get; set; }
        public ViewceCostPlanning _ViewceCostPlanning { get; set; }
        public ViewceMastCostModel _ViewceMastCostModel { get; set; }
        public ViewceMastModel _ViewceMastModel { get; set; }
        public ViewceMastFlowApprove _ViewceMastFlowApprove { get; set; }
        public ViewceHistoryApproved _ViewceHistoryApproved { get; set; }
        public List<ViewceHistoryApproved> _ListViewceHistoryApproved { get; set; }
        public ViewceRunDocument _ViewceRunDocument { get; set; }

        public ViewcceRunCostpalnning _ViewcceRunCostpalnning { get; set; }
        public ViewOperaterCP _ViewOperaterCP { get; set; }



        //public GroupViewceCostPlanning _roupViewceCostPlanning { get; set; }
        public GroupViewceDetailSubMakerRequest _GroupViewceDetailSubMakerRequest { get; set; }
        public List<GroupViewceDetailSubMakerRequest> _ListGroupViewceDetailSubMakerRequest { get; set; }

        //add cost
        public GroupViewCostPlanning _GroupViewCostPlanning { get; set; }
        public List<GroupViewCostPlanning> _ListGroupViewCostPlanning { get; set; }

        //public List<GroupViewceCostPlanning> _ListGroupViewceCostPlanning { get; set; }


        //public GroupViewceDetailSubMakerRequestHissum _GroupViewceDetailSubMakerRequestHisSum { get; set; }
        //public List<GroupViewceDetailSubMakerRequestHissum> _ListGroupViewceDetailSubMakerRequestHisSum { get; set; }


        ////MOLD
        //public ViewmtMaster_Mold_Control _ViewmtMaster_Mold_Control { get; set; }
        //public ViewceHistoryApproved _ViewsvsHistoryApproved { get; set; }

        //IT
        public ViewAttachment Attachment { get; set; }
        public List<ViewAttachment> _listAttachment { get; set; }

        public List<ViewLLLedger> _listViewLLLedger { get; set; }




        //Mold modify

        public List<ViewceMastModifyRequest> _ListViewceMastModifyRequest { get; set; }
        public ViewceMastModifyRequest _ViewceMastModifyRequest { get; set; }

        public List<ViewceItemModifyRequest> _ListViewceItemModifyRequest { get; set; }
        public ViewceItemModifyRequest _ViewceItemModifyRequest { get; set; }

        //admin mold
        public List<ViewceHourChangeCategory> _ListViewceHourChangeCategory { get; set; }
        public ViewceHourChangeCategory _ViewceHourChangeCategory { get; set; }
        public List<ViewceHourChangeEntry> _ListViewceHourChangeEntry { get; set; }
        public ViewceHourChangeEntry _ViewceHourChangeEntry { get; set; }

        public GroupViewceHourChangeCategory _GroupViewceHourChangeCategory { get; set; }
        public List<GroupViewceHourChangeCategory> _ListGroupViewceHourChangeCategory { get; set; }


        public GroupViewceHourChangeEntry GroupViewceHourChangeEntry { get; set; }
        public List<GroupViewceHourChangeEntry> _ListGroupViewceHourChangeEntry { get; set; }

        public GroupedResult _GroupedResult { get; set; }
        public List<GroupedResult> _ListGroupedResult { get; set; }


        public ProcessResult _ProcessResult { get; set; }
        public List<ProcessResult> _ListProcessResult { get; set; }


        //<summary>
        public GroupedResult1 _GroupedResult1 { get; set; }
        public List<GroupedResult1> _ListGroupedResult1 { get; set; }

        public ProcessResult1 _ProcessResult1 { get; set; }
        public List<ProcessResult1> _ListProcessResult1 { get; set; }


        public ViewceHourChangeEntryMonth _ViewceHourChangeEntryMonth { get; set; }
        public List<ViewceHourChangeEntryMonth> _ListViewceHourChangeEntryMonth { get; set; }



        public GroupMain GroupMain { get; set; }
        public List<GroupMain> _ListGroupMain { get; set; }

        public GroupSub GroupSub { get; set; }
        public List<GroupSub> _ListGroupSub { get; set; }

        public GroupSubMonth GroupSubMonth { get; set; }
        public List<GroupSubMonth> _ListGroupSubMonth { get; set; }

        public ViewceMastHourChage _ViewceMastHourChage { get; set; }
        public List<ViewceMastHourChage> _ListViewceMastHourChage { get; set; }




        //for entry month
        public GroupMainData GroupMainData { get; set; }
        public List<GroupMainData> _ListGroupMainData { get; set; }

        public GroupedData GroupedData { get; set; }
        public List<GroupedData> _ListGroupedData { get; set; }

        public MonthData MonthData { get; set; }
        public List<MonthData> _ListMonthData { get; set; }

        public MonthData MlistMonth { get; set; }
        public List<MlistMonth> _ListMlistMonth { get; set; }

        public typeDetail typeDetail { get; set; }
        public List<typeDetail> _ListtypeDetail { get; set; }

        public YeartHourChange YeartHourChange { get; set; }
        public List<YeartHourChange> _ListYeartHourChange { get; set; }


        public ViewceMastType ViewceMastType { get; set; }
        public List<ViewceMastType> _ListViewceMastType { get; set; }


        public GroupedListceDetailSubMakerRequest _GroupedListceDetailSubMakerRequest { get; set; }
        public List<GroupedListceDetailSubMakerRequest> _ListGroupedListceDetailSubMakerRequest { get; set; }


        public GroupedListceDetailSub _GroupedListceDetailSub { get; set; }
        public List<GroupedListceDetailSub> _ListGroupedListceDetailSub { get; set; }

        public GroupViewceDetailSubMakerRequest _GroupViewceDetailSubMakerRequest1 { get; set; }
        public List<GroupViewceDetailSubMakerRequest> _ListGroupViewceDetailSubMakerRequest1 { get; set; }

        //sum all 
        public VisaulViewceDetailSubMakerRequest _VisaulViewceDetailSubMakerRequest { get; set; }
        public List<VisaulViewceDetailSubMakerRequest> _ListVisaulViewceDetailSubMakerRequest { get; set; }
        public GroupViewceDetailSubMakerRequestHissum groupViewceDetailSubMakerRequestHissum { get; set; }
        public List<GroupViewceDetailSubMakerRequestHissum> _ListGroupViewceDetailSubMakerRequestHissum { get; set; }

        public ViewceMastSubHistorySum _ViewceMastSubHistorySum { get; set; }
        public List<ViewceMastSubDetailHistorySum> _ListViewceMastSubDetailHistorySum  { get; set; }
        public GroupDetailSubMakerRequestHissum GroupDetailSubMakerRequestHissum { get; set; }
        public List<GroupDetailSubMakerRequestHissum> _ListGroupDetailSubMakerRequestHissum { get; set; }


        //mold other
        public ViewceSubWorkingTimeRequestItem _ViewceSubWorkingTimeRequestItem { get; set; }
        public List<ViewceSubWorkingTimeRequestItem> _ListViewceSubWorkingTimeRequestItem { get; set; }
        public GroupViewceSubWorkingTimeRequestItem _GroupViewceSubWorkingTimeRequestItem { get; set; }
        public List<GroupViewceSubWorkingTimeRequestItem> _ListGroupViewceSubWorkingTimeRequestItem { get; set; }



    }




    public class OTTimeStart
    {
        public string Time { get; set; }
    }
    public class OTTimeEnd
    {
        public string Time { get; set; }
    }
    public class OTModel
    {
        public string Name { get; set; }
    }
    public class OTProdLine
    {
        public string Name { get; set; }
    }
    public class OTReason
    {
        public string Code { get; set; }
        public string Caption { get; set; }
    }
    public class CCMail
    {
        public string email { get; set; }
    }

    public class req
    {
        public string no { get; set; }
    }
    public class searchbydate
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class CategoryWorkerList
    {
        public Guid Guid { get; set; }
        public byte EmpPic { get; set; }
        public string PriName { get; set; }
        public string EmpCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
        public string GRP_Code { get; set; }
    }

    public class workerImages
    {
        public string empcode { get; set; }
        public string image { get; set; }
    }
}
