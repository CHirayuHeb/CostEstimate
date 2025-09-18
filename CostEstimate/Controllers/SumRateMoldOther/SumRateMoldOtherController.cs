using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.MyRequest;
using CostEstimate.Models.New;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MOLD;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Http;
using System.IO;

using MimeKit;
using MailKit.Net.Smtp;

namespace CostEstimate.Controllers.SumRateMoldOther
{
    public class SumRateMoldOtherController : Controller
    {
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public string path = @"\\thsweb\\CostEstimate\\";
        public string PgName = "CostEstimate";
        public SumRateMoldOtherController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
        {
            _LAMP = lamp;
            _HRMS = hrms;
            _IT = it;
            _MK = mk;
            _MOLD = mold;
            _Cache = cacheController;
            _callFunc = callfunction;
        }


        [Authorize("Checked")]
        public IActionResult Index(Class @class, string Docno, int vProcess)
        {

            try
            {

                @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();

                @class._ViewceMastWorkingTimeRequest = new ViewceMastWorkingTimeRequest();
                @class._ViewceItemWorkingTimePartName = new ViewceItemWorkingTimePartName();
                @class._ViewceItemWorkingTimeSizeProduct = new ViewceItemWorkingTimeSizeProduct();

                @class._ViewceMastMaterialRequest = new ViewceMastMaterialRequest();
                @class._ViewceItemPartName = new ViewceItemPartName();
                @class._ListViewceItemMaterialRequestPartName = new List<ViewceItemMaterialRequestPartName>();
                @class._ListGroupViewceMastChartRateOtherReport = new List<GroupViewceMastChartRateOtherReport>();

                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == Docno).FirstOrDefault();

                @class._ViewceMastWorkingTimeRequest = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == Docno).FirstOrDefault();
                @class._ViewceItemWorkingTimeSizeProduct = _MK._ViewceItemWorkingTimeSizeProduct.Where(x => x.wsDocumentNoSub == @class._ViewceMastWorkingTimeRequest.wrDocumentNoSub && x.wsRunNo == vProcess).FirstOrDefault();
                // @class._ViewceItemWorkingTimePartName = _MK._ViewceItemWorkingTimePartName.Where(x => x.wpDocumentNoSub == @class._ViewceMastWorkingTimeRequest.wrDocumentNoSub).FirstOrDefault();

                @class._ViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == Docno).FirstOrDefault();
                @class._ViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == Docno && x.ipRunNo == vProcess).FirstOrDefault();
                @class._ListViewceItemMaterialRequestPartName = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub && x.mpNoProcess == vProcess).ToList();



                //select CE mast cost

                @class._ListGroupViewceMastChartRateOtherReport = getListGroupViewceMastChartRateOtherReport(Docno, vProcess);

            }
            catch (Exception ex)
            {

            }

            return View(@class);
        }

        public List<GroupViewceMastChartRateOtherReport> getListGroupViewceMastChartRateOtherReport(string Docno, int vProcess)
        {
            Class @class = new Class();

            var ceCostPlan = _MK._ViewceMastChartRateOtherReport.Where(x => x.crDocumentNo == Docno).Select(x => x.crCostPlanningNo).FirstOrDefault();
            var listCeCostPlan = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == ceCostPlan).ToList();



            //ceWorkingTimePartName
            var docWokingTime = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == Docno).Select(x => x.wrDocumentNoSub).FirstOrDefault();
            var ceWorkingTimePartName = _MK._ViewceItemWorkingTimePartName.Where(x => x.wpDocumentNoSub == docWokingTime && x.wpNoProcess == vProcess).ToList();


            var mappingList = new List<MappingRuleChartRate>
            {
                new MappingRuleChartRate { Code = "DT&QC.", ManFormula = "OT CAD#wpWT_MAN,OT CAM#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "3-D.", ManFormula = "3D(QC)#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "CAD-D.", ManFormula = "CAD-D#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "CAD-M.", ManFormula = "CAD-M#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "BM.", ManFormula = "BM#wpWT_MAN",AutoFormula ="BM#wpWTAuto" },
                new MappingRuleChartRate { Code = "NC(CO).", ManFormula = "NC#wpWT_MAN",AutoFormula ="NC#wpWT_MAN,NC#wpWTAuto" },
                new MappingRuleChartRate { Code = "NCG(CO).", ManFormula = "NCG#wpWT_MAN",AutoFormula ="NCG#wpWT_MAN,NCG#wpWTAuto" },
                new MappingRuleChartRate { Code = "NCL.", ManFormula = "MNC#wpWT_MAN,LNC#wpWT_MAN",AutoFormula ="MNC#wpWT_MAN,MNC#wpWTAuto,LNC#wpWT_MAN,LNC#wpWTAuto" },
                new MappingRuleChartRate { Code = "NC(GR).", ManFormula = "NCGR#wpWT_MAN",AutoFormula ="NCGR#wpWT_MAN,NCGR#wpWTAuto" },
                new MappingRuleChartRate { Code = "EDM(CO).", ManFormula = "ED#wpWT_MAN",AutoFormula ="ED#wpWT_MAN,ED#wpWTAuto" },
                new MappingRuleChartRate { Code = "W-E.", ManFormula = "WE#wpWT_MAN",AutoFormula ="WE#wpWT_MAN,WE#wpWTAuto" },
                new MappingRuleChartRate { Code = "M.", ManFormula = "M.#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "F.", ManFormula = "FG#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "P(W).", ManFormula = "PG#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "TRIAL.", ManFormula = "OT TM#wpWT_MAN",AutoFormula ="" },
                new MappingRuleChartRate { Code = "OT.FG", ManFormula = "OT 3D#wpWT_MAN,OT NC#wpWT_MAN,OT PG#wpWT_MAN,OT FG#wpWT_MAN",AutoFormula ="" },
            };


            /*   DT&QC. = OT CAD(MAN) + OT CAM(MAN)
                 3-D. = 3D(QC)(MAN)
                 CAD-D. = CAD-D(MAN)
                 CAD-M. =CAD-M(MAN)
                 BM. = BM(MAN), BM(AUTO)	
                 NC(CA). = ?
                 NC(CO).= NC(MAN),  NC(MAN)+NC(AUTO)
                 NCG(CA). = ?
                 NCG(CO). =NCG(MAN) ,NCG(MAN) +NCG(AUTO)
                 NCL. =MNC(MAN) + LNC(MAN)   , MNC(MAN) + MNC(AUTO)+LNC(MAN)+LNC(AUTO)	
                 NC(GR).= NCGR(MAN) ,NCGR(MAN)+ NCGR(AUTO)
                 EDM(CA). =?
                 EDM(CO). =ED(MAN) , ED(MAN) + ED(AUTO)
                 W-E. =WE(MAN) ,	WE(MAN)+WE(AUTO)
                 M. = GM(MAN)
                 SG,FG,CG. =?
                 D. =?
                 D(E). =?
                 RD. =?
                 L.=?
                 W. =?
                 W(L). =?
                 P(A).=?
                 C,U.=?
                 DS.=?
                 MK.=?
                 MF. =?
                 F. = FG(MAN)
                 F(GR).=?
                 M-A.=?
                 P(M).=?
                 P(W). = PG(MAN)
                 TRIAL. = OT TM(MAN)
                 OT.FG = =OT 3D(MAN) + OT NC(MAN) + OT PG(MAN)+  OT FG(MAN)   
                 MEETING.
            */



            @class._ListViewDetailceMastChartRateOtherReport = new List<ViewDetailceMastChartRateOtherReport>();
            for (int i = 0; i < listCeCostPlan.Count(); i++)
            {
                //select list
                double crWTMan = 0;
                double crWTTotal = 0;

                var RuleChartRate = mappingList.FirstOrDefault(m => m.Code == listCeCostPlan[i].cpProcessName);
                if (RuleChartRate != null)
                {
                    //get Man
                    var manParts = RuleChartRate.ManFormula.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in manParts)
                    {
                        var lmanParts = part.Split('#');
                        if (lmanParts.Count() > 1)
                        {


                            var vceItemWorking = _MK._ViewceItemWorkingTimePartName.Where(x => x.wpDocumentNoSub == docWokingTime && x.wpNoProcess == vProcess && x.wpProcessName == lmanParts[0].ToString()).ToList();
                            double vWTMan = vceItemWorking != null && vceItemWorking.Count() > 0 ?
                                            lmanParts[1].ToString() == "wpWT_MAN" ? vceItemWorking.Select(x => x.wpWT_Man).FirstOrDefault() : vceItemWorking.Select(x => x.wpWT_Auto).FirstOrDefault() : 0;

                            crWTMan += vWTMan;

                        }



                    }
                    //get auto
                    var autoParts = RuleChartRate.AutoFormula.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in autoParts)
                    {
                        var lmanParts = part.Split('#');
                        if (lmanParts.Count() > 1)
                        {

                            var vceItemWorking = _MK._ViewceItemWorkingTimePartName.Where(x => x.wpDocumentNoSub == docWokingTime && x.wpNoProcess == vProcess && x.wpProcessName == lmanParts[0].ToString()).ToList();
                            double vWTAuto = vceItemWorking != null && vceItemWorking.Count() > 0 ?
                                         lmanParts[1].ToString() == "wpWT_MAN" ? vceItemWorking.Select(x => x.wpWT_Man).FirstOrDefault() : vceItemWorking.Select(x => x.wpWT_Auto).FirstOrDefault() : 0;
                            crWTTotal += vWTAuto;

                        }




                    }

                }

                //find list 

                //for loop sum

                //DESIGN
                //NC.
                double sSumLabour_Cost = listCeCostPlan[i].cpProcessName == "NC." ? crWTTotal * listCeCostPlan[i].cpDP_Rate / 1000 : crWTMan * listCeCostPlan[i].cpDP_Rate / 1000;
                double sSumDPrCost = listCeCostPlan[i].cpProcessName == "NC." ? crWTTotal * listCeCostPlan[i].cpDP_Rate / 1000 : crWTMan * listCeCostPlan[i].cpDP_Rate / 1000;
                double sSumME_Cost = listCeCostPlan[i].cpProcessName == "NC." ? crWTTotal * listCeCostPlan[i].cpME_Rate / 1000 : crWTMan * listCeCostPlan[i].cpME_Rate / 1000;


                @class._ListViewDetailceMastChartRateOtherReport.Add(new ViewDetailceMastChartRateOtherReport
                {
                    crGroupName = listCeCostPlan[i].cpGroupName,
                    cpProcessName = listCeCostPlan[i].cpProcessName,
                    crWTMan = crWTMan,
                    crWTTotal = crWTTotal,
                    crLabour_Rate = listCeCostPlan[i].cpLabour_Rate,
                    crLabour_Cost = Math.Round(crWTMan * listCeCostPlan[i].cpLabour_Rate / 1000, 2),
                    crDP_Rate = listCeCostPlan[i].cpDP_Rate,
                    crpDP_Cost = listCeCostPlan[i].cpProcessName == "NC." ? Math.Round(crWTTotal * listCeCostPlan[i].cpDP_Rate / 1000, 2) : Math.Round(crWTMan * listCeCostPlan[i].cpDP_Rate / 1000, 2),
                    crME_Rate = listCeCostPlan[i].cpME_Rate,
                    crME_Cost = listCeCostPlan[i].cpProcessName == "NC." ? Math.Round(crWTTotal * listCeCostPlan[i].cpME_Rate / 1000, 2) : Math.Round(crWTMan * listCeCostPlan[i].cpME_Rate / 1000, 2),
                    crTotal_cost = Math.Round(sSumLabour_Cost + sSumDPrCost + sSumME_Cost, 2),
                    crChartRateSub_Local_Rate = listCeCostPlan[i].cpCR_Local_Rate,
                    crChartRateSub_Local_Cost = listCeCostPlan[i].cpProcessName == "NC." ? Math.Round(crWTTotal * listCeCostPlan[i].cpCR_Local_Rate / 1000, 2) : Math.Round(crWTMan * listCeCostPlan[i].cpCR_Local_Rate / 1000, 2),
                    crChartRateSub_Oversea_Rate = listCeCostPlan[i].cpCR_Oversea_Rate,
                    crChartRateSub_Oversea_Cost = listCeCostPlan[i].cpProcessName == "NC." ? Math.Round(crWTTotal * listCeCostPlan[i].cpCR_Oversea_Rate / 1000, 2) : Math.Round(crWTMan * listCeCostPlan[i].cpCR_Oversea_Rate / 1000, 2),
                });
            }




            @class._ListGroupViewceMastChartRateOtherReport = @class._ListViewDetailceMastChartRateOtherReport.GroupBy(p => p.crGroupName).Select(g => new GroupViewceMastChartRateOtherReport
            {
                GroupName = g.Key.Trim(),
                DetailceMastChartRateOtherReport = g.ToList()
            }).ToList();

            return @class._ListGroupViewceMastChartRateOtherReport;

        }

    }
}