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

        //MK
        public ViewGroupbyceMastCostModel _ViewGroupbyceMastCostModel { get; set; }
        public List<ViewGroupbyceMastCostModel> _ListViewGroupbyceMastCostModel { get; set; }
        public List<GroupViewceMastCostModel> _ListGroupViewceMastCostModel { get; set; }
        public GroupViewceMastCostModel _GroupViewceMastCostModel { get; set; }


        public ViewLoginPgm _ViewLoginPgm { get; set; }
        public ViewSearchData _ViewSearchData { get; set; }

        public List<ViewceMastSubMakerRequest> _ListceMastSubMakerRequest { get; set; }
        public List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest { get; set; }
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
