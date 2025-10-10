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
using System.Globalization;

namespace CostEstimate.Controllers.NewMoldOther
{
    public class NewMoldOtherController : Controller
    {
        //username emppic ftpdb
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public string path = @"\\thsweb\\CostEstimate\\";
        public string PgName = "CostEstimate";
        public NewMoldOtherController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        public IActionResult Index(Class @class, string id, string Rev)
        {

            @class._ViewOperaterCP = new ViewOperaterCP();

            @class._listAttachment = new List<ViewAttachment>();
            List<string> _listRequestBy = _MK._ViewceMastType.Where(x => x.mtType.Contains("RequestBy") && x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList _TypeofRequestBy = new SelectList(_listRequestBy);
            ViewBag.TypeofRequestBy = _TypeofRequestBy;

            @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();

            @class._ViewceMastModifyRequest = new ViewceMastModifyRequest();
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3").ToList();

            List<string> _listTypeofCavity = _MK._ViewceMastType.Where(x => x.mtType.Contains("Cavity") && x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
            ViewBag.TypeofCavity = _TypeofCavity;

            @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
            @class._ViewceItemPartName = new ViewceItemPartName();
            @class._ListViewceItemPartName = new List<ViewceItemPartName>();

            //table sub
            @class._ViewceMastWorkingTimeRequest = new ViewceMastWorkingTimeRequest();
            @class._ViewceMastMaterialRequest = new ViewceMastMaterialRequest();
            @class._ViewceMastToolGRRequest = new ViewceMastToolGRRequest();
            @class._ViewceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
            @class._ListViewMoldOtherDatailQuotation = new List<ViewMoldOtherDatailQuotation>();
            if (id != null)
            {
                //check status 
                string chk = "";
                int vstepmain = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == id).Select(x => x.mrStep).FirstOrDefault();
                if (vstepmain == 2) //Waiting Checked By WORKING TIME (OPG) , MATERIAL(DG MOLD), TOOL(CAM), INFORMATION(DRG)
                {
                    chk = UpdateStatusDoc(id);
                }


                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == id).FirstOrDefault();
                @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == id).OrderBy(x => x.ipRunNo).ToList();

                //table sub
                @class._ViewceMastWorkingTimeRequest = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == id).FirstOrDefault();
                @class._ViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == id).FirstOrDefault();
                @class._ViewceMastToolGRRequest = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == id).FirstOrDefault();
                @class._ViewceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == id).FirstOrDefault();
                @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == id).ToList();



                //for report Quataion 22/09/2025 
                //@class._ListViewMoldOtherDatailQuotation = getDatailQuotation(id, @class);

                //@class._ListViewMoldOtherDatailQuotation = new List<ViewMoldOtherDatailQuotation>();
                //for (int i = 0; i < @class._ListViewceItemPartName.Count(); i++)
                //{
                //    @class._ListViewMoldOtherDatailQuotation.Add(new ViewMoldOtherDatailQuotation
                //    {
                //        dMoldNoName = @class._ListViewceItemPartName[i].ipPartName,
                //        dCavityNo = @class._ListViewceItemPartName[i].ipCavityNo,
                //        dTypeCavity = @class._ListViewceItemPartName[i].ipTypeCavity,
                //        dDetail = "2 PLATE TYPE",

                //    });
                //}



            }


            return View(@class);
        }

        public string UpdateStatusDoc(string id)
        {
            string vstatus = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3" && x.mfStep == 3).Select(x => x.mfSubject).FirstOrDefault();

            int vstepWK = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == id).Select(x => x.wrStep).FirstOrDefault();
            int vstepMT = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == id).Select(x => x.mrStep).FirstOrDefault();
            int vstepTGR = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == id).Select(x => x.trStep).FirstOrDefault();
            int vstepSP = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == id).Select(x => x.irStep).FirstOrDefault();
            if (vstepWK == 4 && vstepMT == 4 && vstepTGR == 4 && vstepSP == 4)
            {
                var _ceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == id).FirstOrDefault();
                _ceMastMoldOtherRequest.mrStep = 3;
                _ceMastMoldOtherRequest.mrStatus = vstatus;
                _MK._ViewceMastMoldOtherRequest.Update(_ceMastMoldOtherRequest);
                _MK.SaveChanges();
            }


            return "sucess";
        }


        public List<ViewMoldOtherDatailQuotation> getDatailQuotation(string id, Class @class)
        {
            List<ViewMoldOtherDatailQuotation> _ListViewMoldOtherDatailQuotation = new List<ViewMoldOtherDatailQuotation>();
            //for report Quataion 22 / 09 / 2025

            for (int i = 0; i < @class._ListViewceItemPartName.Count(); i++)
            {

                //ceMastInforSpacMoldRequest
                //ceItemInforRequestPartName
                //ceItemInforSlideSystem
                //ceItemInforTypeOfCut
                //ceItemInforShibo
                var DocNoSub = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == id).Select(x => x.irDocumentNoSub).FirstOrDefault();


                string _ItemInforRequestPart = "";
                string ipGate1 = "";
                string ipGate2 = "";
                string ipGate3 = "";
                string ipsprueSys = "";
                string ipMakerRunHot = "";

                string vInforSlideSystem = "";

                string vGMaterial = "";
                string ipBaseCavity = "";
                string ipInsertCavity = "";
                string ipBaseCode = "";
                string ipInsertCode = "";


                string vitemTypeofcout = "";


                string vitemSHIBO = "";

                var _listViewceItemInforRequestPartName = _MK._ViewceItemInforRequestPartName.Where(x => x.ipDocumentNoSub == DocNoSub && x.ipNoProcess == @class._ListViewceItemPartName[i].ipRunNo).ToList();
                for (int j = 0; j < _listViewceItemInforRequestPartName.Count(); j++)
                {
                    ipGate1 += _listViewceItemInforRequestPartName[j].ipGateType1 != null && _listViewceItemInforRequestPartName[j].ipGateType1 != "" ? _listViewceItemInforRequestPartName[j].ipGateType1 + "(" + _listViewceItemInforRequestPartName[j].ipNumberPoint1.ToString() + "DROP), " : "";
                    ipGate2 += _listViewceItemInforRequestPartName[j].ipGateType2 != null && _listViewceItemInforRequestPartName[j].ipGateType2 != "" ? _listViewceItemInforRequestPartName[j].ipGateType2 + "(" + _listViewceItemInforRequestPartName[j].ipNumberPoint2.ToString() + "DROP), " : "";
                    ipGate3 += _listViewceItemInforRequestPartName[j].ipGateType3 != null && _listViewceItemInforRequestPartName[j].ipGateType3 != "" ? _listViewceItemInforRequestPartName[j].ipGateType3 + "(" + _listViewceItemInforRequestPartName[j].ipNumberPoint3.ToString() + "DROP), " : "";


                    //ipsprueSys = _listViewceItemInforRequestPartName[0].ipSprueSystem != null && _listViewceItemInforRequestPartName[0].ipSprueSystem != "" ?
                    //                                        _listViewceItemInforRequestPartName[0].ipSprueSystem : "";
                    //ipMakerRunHot = _listViewceItemInforRequestPartName[0].ipMakerHotRunner != null && _listViewceItemInforRequestPartName[0].ipMakerHotRunner != "" ? "(" + _listViewceItemInforRequestPartName[0].ipMakerHotRunner + "), " : ", ";


                    ipsprueSys = _listViewceItemInforRequestPartName[0].ipSprueSystem ?? "";
                    ipMakerRunHot = string.IsNullOrEmpty(_listViewceItemInforRequestPartName[0].ipMakerHotRunner)
                    ? ", "
                        : $"({_listViewceItemInforRequestPartName[0].ipMakerHotRunner}), ";

                }

                var _listViewceItemInforSlideSystem = _MK._ViewceItemInforSlideSystem.Where(x => x.isDocumentNoSub == DocNoSub && x.isNoProcess == @class._ListViewceItemPartName[i].ipRunNo).ToList();
                if (_listViewceItemInforSlideSystem.Count > 0)
                {
                    for (int j = 0; j < _listViewceItemInforSlideSystem.Count(); j++)
                    {
                        vInforSlideSystem += _listViewceItemInforSlideSystem[j].isSlideSystemType + "(" + _listViewceItemInforSlideSystem[j].isSlideSystemCount.ToString() + " PCS),";
                    }
                }
                else
                {
                    vInforSlideSystem = "NO SLIDE, ";
                }


                //GROUP MATERIAL
                for (int j = 0; j < _listViewceItemInforRequestPartName.Count(); j++)
                {
                    ipBaseCavity = _listViewceItemInforRequestPartName[j].ipBaseCavity != null ? _listViewceItemInforRequestPartName[j].ipBaseCavity + "," : "";
                    ipInsertCavity = _listViewceItemInforRequestPartName[j].ipInsertCavity != null ? _listViewceItemInforRequestPartName[j].ipInsertCavity + "," : "";
                    ipBaseCode = _listViewceItemInforRequestPartName[j].ipBaseCode != null ? _listViewceItemInforRequestPartName[j].ipBaseCode + "," : "";
                    ipInsertCode = _listViewceItemInforRequestPartName[j].ipInsertCode != null ? _listViewceItemInforRequestPartName[j].ipInsertCode + "," : "";
                }
                vGMaterial = ipBaseCavity + ipInsertCavity + ipBaseCode + ipInsertCode;


                //TYPE OF CUT
                var _listItemInforTypeOfCut = _MK._ViewceItemInforTypeOfCut.Where(x => x.icDocumentNoSub == DocNoSub && x.icNoProcess == @class._ListViewceItemPartName[i].ipRunNo).ToList();
                if (_listItemInforTypeOfCut.Count > 0)
                {
                    vitemTypeofcout = "*HAVE ";
                    for (int j = 0; j < _listItemInforTypeOfCut.Count(); j++)
                    {

                        vitemTypeofcout += _listItemInforTypeOfCut[j].icTypeofcut;
                        if (j < _listItemInforTypeOfCut.Count() - 1)
                        {
                            vitemTypeofcout += " && ";
                        }
                    }
                    vitemTypeofcout += " = 0.33 mm";
                }
                else
                {
                    vitemTypeofcout = "*DONT HAVE,";
                }


                //SHIBO
                var _listItemShibo = _MK._ViewceItemInforShibo.Where(x => x.ibDocumentNoSub == DocNoSub && x.ibNoProcess == @class._ListViewceItemPartName[i].ipRunNo).ToList();
                if (_listItemShibo.Count > 0)
                {
                    vitemSHIBO = "*HAVE SHIBO ";
                    for (int j = 0; j < _listItemShibo.Count(); j++)
                    {

                        vitemSHIBO += _listItemShibo[j].ibSHiboPCS;
                        if (j < _listItemInforTypeOfCut.Count() - 1)
                        {
                            vitemSHIBO += ", ";
                        }
                    }

                }
                else
                {
                    vitemSHIBO = "*DONT HAVE SHIBO, ";
                }

                _ItemInforRequestPart = ipGate1 + ipGate2 + ipGate3 + ipsprueSys + ipMakerRunHot + vInforSlideSystem + vGMaterial + vitemTypeofcout + vitemSHIBO;



                //cal dEstimateCost TOTAL MT.

                @class._ListViewDetailceMastChartRateOtherReport = getListViewDetailceMastChartRateOtherReport(id, @class._ListViewceItemPartName[i].ipRunNo);


                @class._ViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == id).FirstOrDefault();
                @class._ListViewceItemMaterialRequestPartName = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub && x.mpNoProcess == @class._ListViewceItemPartName[i].ipRunNo).ToList();


                double vRate = @class._ListViewceItemPartName[i].ipRateReport;
                double vTotalCost = Math.Round(@class._ListViewDetailceMastChartRateOtherReport.Sum(x => x.crTotal_cost), 2);
                double vCalTotal = Math.Round(vTotalCost * vRate, 2);
                double vTOTALMT = @class._ListViewceItemMaterialRequestPartName[0].mpTotal;
                double resultsum = vCalTotal + vTOTALMT;

                double result = Math.Ceiling(resultsum / 10.0) * 10 * 1000;


                _ListViewMoldOtherDatailQuotation.Add(new ViewMoldOtherDatailQuotation
                {
                    dMoldNoName = @class._ListViewceItemPartName[i].ipPartName,
                    dCavityNo = @class._ListViewceItemPartName[i].ipCavityNo,
                    dTypeCavity = @class._ListViewceItemPartName[i].ipTypeCavity,
                    dEstimateCost = result.ToString("N0"),
                    dDetail = _ItemInforRequestPart,

                });
            }

            return _ListViewMoldOtherDatailQuotation;
        }




        //public List<GroupViewceMastChartRateOtherReport> getListGroupViewceMastChartRateOtherReport(string Docno, int vProcess)
        public List<ViewDetailceMastChartRateOtherReport> getListViewDetailceMastChartRateOtherReport(string Docno, int vProcess)
        {
            Class @class = new Class();

            var ceCostPlan = _MK._ViewceMastChartRateOtherReport.Where(x => x.crDocumentNo == Docno).Select(x => x.crCostPlanningNo).FirstOrDefault();
            var listCeCostPlan = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == ceCostPlan).ToList();



            //ceWorkingTimePartName
            var docWokingTime = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == Docno).Select(x => x.wrDocumentNoSub).FirstOrDefault();
            var ceWorkingTimePartName = _MK._ViewceItemWorkingTimePartName.Where(x => x.wpDocumentNoSub == docWokingTime && x.wpNoProcess == vProcess).ToList();



            List<ViewceMastMappingRuleChartRate> mappingList = new List<ViewceMastMappingRuleChartRate>();
            mappingList = _MK._ViewceMastMappingRuleChartRate.ToList();

            //var mappingList = new List<MappingRuleChartRate>
            //{
            //    new MappingRuleChartRate { Code = "DT&QC.", ManFormula = "OT CAD#wpWT_MAN,OT CAM#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "3-D.", ManFormula = "3D(QC)#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "CAD-D.", ManFormula = "CAD-D#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "CAD-M.", ManFormula = "CAD-M#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "BM.", ManFormula = "BM#wpWT_MAN",AutoFormula ="BM#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "NC(CO).", ManFormula = "NC#wpWT_MAN",AutoFormula ="NC#wpWT_MAN,NC#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "NCG(CO).", ManFormula = "NCG#wpWT_MAN",AutoFormula ="NCG#wpWT_MAN,NCG#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "NCL.", ManFormula = "MNC#wpWT_MAN,LNC#wpWT_MAN",AutoFormula ="MNC#wpWT_MAN,MNC#wpWTAuto,LNC#wpWT_MAN,LNC#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "NC(GR).", ManFormula = "NCGR#wpWT_MAN",AutoFormula ="NCGR#wpWT_MAN,NCGR#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "EDM(CO).", ManFormula = "ED#wpWT_MAN",AutoFormula ="ED#wpWT_MAN,ED#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "W-E.", ManFormula = "WE#wpWT_MAN",AutoFormula ="WE#wpWT_MAN,WE#wpWTAuto" },
            //    new MappingRuleChartRate { Code = "M.", ManFormula = "M.#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "F.", ManFormula = "FG#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "P(W).", ManFormula = "PG#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "TRIAL.", ManFormula = "OT TM#wpWT_MAN",AutoFormula ="" },
            //    new MappingRuleChartRate { Code = "OT.FG", ManFormula = "OT 3D#wpWT_MAN,OT NC#wpWT_MAN,OT PG#wpWT_MAN,OT FG#wpWT_MAN",AutoFormula ="" },
            //};


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

                var RuleChartRate = mappingList.FirstOrDefault(m => m.mrCode == listCeCostPlan[i].cpProcessName);
                if (RuleChartRate != null)
                {
                    //get Man
                    //var manParts = RuleChartRate.mrManFormula.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var manParts = string.IsNullOrEmpty(RuleChartRate.mrManFormula) ? Array.Empty<string>() : RuleChartRate.mrManFormula.Split(',', StringSplitOptions.RemoveEmptyEntries);
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
                    //var autoParts =  RuleChartRate.mrAutoFormula.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var autoParts = !string.IsNullOrEmpty(RuleChartRate.mrAutoFormula) ? RuleChartRate.mrAutoFormula.Split(',', StringSplitOptions.RemoveEmptyEntries) : Array.Empty<string>();


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

            return @class._ListViewDetailceMastChartRateOtherReport;// @class._ListGroupViewceMastChartRateOtherReport;

        }
        [Authorize("Checked")]
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastMoldOtherRequest != null ? @classs._ViewceMastMoldOtherRequest.mrStep : 0;
            //int v_step = 2;
            string v_issue = @classs._ViewceMastMoldOtherRequest != null ? @classs._ViewceMastMoldOtherRequest.mrEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastMoldOtherRequest != null ? @classs._ViewceMastMoldOtherRequest.mrDocmentNo : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            partialUrl = Url.Action("SendMail", "NewMoldOther", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });
            try
            {
                if (@classs._ViewceMastMoldOtherRequest != null)
                {
                    if (@classs._ViewceMastMoldOtherRequest.mrDocmentNo != "" && @classs._ViewceMastMoldOtherRequest.mrDocmentNo != null)
                    {
                        // htCostPlanningNo
                        String htDocNo = @classs._ViewceMastMoldOtherRequest.mrDocmentNo.ToString(); //htCostPlanningNo
                                                                                                     //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htStep).ThenBy(x=>x.htDate).ThenBy(x=>x.htTime).ToList();
                        _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
                        if (_listHistory.Count() > 0)
                        {
                            for (int j = 0; j < _listHistory.Count(); j++)
                            {
                                var v_htcc = _listHistory[j].htCC;
                                string v_CCemail = "";
                                if (v_htcc != null)
                                {
                                    ViewrpEmail fromEmailCC = new ViewrpEmail();
                                    string[] splitCC = v_htcc.Split(',');
                                    foreach (var i in splitCC)
                                    {
                                        if (i != " " & i != "")
                                        {
                                            var v_cc = "";
                                            try
                                            {
                                                fromEmailCC = _IT.rpEmails.Where(w => w.emEmpcode == i.Trim()).FirstOrDefault();
                                                v_CCemail += fromEmailCC.emName_M365.ToString() + ",";
                                            }
                                            catch (Exception e)
                                            {
                                                v_cc = e.Message;
                                            }
                                        }
                                    }
                                }

                                _listHistory[j].htCC = v_CCemail;


                            }

                        }


                        return Json(new { status = _listHistory.Count() > 0 ? "hasHistory" : "empty", listHistory = _listHistory, partial = partialUrl });
                    }
                }

            }
            catch (Exception e)
            {

            }

            //return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
            return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
        }

        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;

            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365

            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            //ViewBag.step = s_step;
            string v_empCodeTo, v_emailTo;


            if (s_step == 1)
            {
                //flow 4 working time
                //flow 5 
                for (int i = 4; i < 8; i++)
                {
                    v_empCodeTo = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == 0 && x.mfFlowNo == i.ToString()) != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == 0 && x.mfFlowNo == i.ToString()).Select(x => x.mfTo).FirstOrDefault() : "";
                    string[] s_empCodeTo = v_empCodeTo.Split(",");
                    List<string> _listNameTo = new List<string>();
                    for (int l = 0; l < s_empCodeTo.Count(); l++)
                    {
                        v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == s_empCodeTo[l].ToString()).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                        _listNameTo.Add(v_emailTo);
                    }

                    SelectList _Selectlist = new SelectList(_listNameTo);
                    //if (map.ContainsKey(i))
                    //{
                    //    ViewBag[map[i]] = _Selectlist;
                    //}
                    if (i == 4) ViewBag._listName0 = _Selectlist;
                    if (i == 5) ViewBag._listName1 = _Selectlist;
                    if (i == 6) ViewBag._listName2 = _Selectlist;
                    if (i == 7) ViewBag._listName3 = _Selectlist;

                    v_empCodeTo = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == 0 && x.mfFlowNo == i.ToString()) != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == 0 && x.mfFlowNo == i.ToString()).Select(x => x.mfTo).FirstOrDefault() : "";
                    v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empCodeTo).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                    @class._ListViewceHistoryApproved.Add(new ViewceHistoryApproved
                    {
                        htNo = 0,
                        htDocNo = "",
                        htStep = 1,
                        htStatus = "Approve",
                        htFrom = v_emailFrom,
                        htTo = "",
                        htCC = "",
                        htDate = DateTime.Now.ToString("yyyy/MM/dd"),
                        htTime = "",
                        htRemark = "",
                    });
                }
                return PartialView("SendMail_step2", @class);
            }

            else if (s_step == 7)
            {


                v_empCodeTo = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == mpNo).Select(x => x.mrEmpCodeRequest).FirstOrDefault();
                v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empCodeTo).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;
                @class._ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                @class._ViewceHistoryApproved.htStep = s_step;

                return PartialView("SendMail", @class);
            }
            else
            {
                v_empCodeTo = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "3") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "3").Select(x => x.mfTo).FirstOrDefault() : "";
                v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empCodeTo).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;
                @class._ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                @class._ViewceHistoryApproved.htStep = s_step;

                return PartialView("SendMail", @class);
            }

        }

        public ActionResult SearchMold_Ledger_Number(string term)
        {

            return Json(
                        _MOLD._ViewLLLedger
                            .Where(p => p.LGLegNo.Contains(term))
                            .Select(p =>
                                p.LGLegNo + "" +
                                p.LGTypeCode + "|" +
                                p.LGCustomer + "|" +
                                p.LGMoldNo + "|" + p.LGMoldName + "|" +
                                "0"
                            )
                            .ToList()
                        );

        }

        public ActionResult SearchCustomer(string term)
        {

            return Json(
                        _MK._ViewceMastType
                            .Where(p => p.mtName.Contains(term) && p.mtProgram == "MoldOther" && p.mtType == "Customer")
                            .Select(p => p.mtName
                            )
                            .ToList()
                        );

        }
        public ActionResult SearchFuntion(string term)
        {

            return Json(
                        _MK._ViewceMastType
                            .Where(p => p.mtName.Contains(term) && p.mtProgram == "MoldOther" && p.mtType == "Function")
                            .Select(p => p.mtName
                            )
                            .ToList()
                        );

        }
        public ActionResult SearchModel(string term)
        {

            //remark use type subMaker becuase  use same model
            return Json(
                        _MK._ViewceMastModel
                            .Where(p => p.mmModelName.Contains(term) && p.mmType == "subMaker")
                            .Select(p => p.mmModelName
                            )
                            .ToList()
                        );

        }
        public ActionResult SearchEvent(string term)
        {

            var listEvent = Enumerable.Range(1, 99)
                             .Select(i => $"Q{i}")
                             .ToList();


            return Json(listEvent
                            .Where(q => q.Contains(term, StringComparison.OrdinalIgnoreCase))
                            .ToList()
                            );

        }

        public ActionResult SearchType(string term)
        {

            return Json(
                        _MK._ViewceMastType
                            .Where(p => p.mtName.Contains(term) && p.mtProgram == "MoldOther" && p.mtType == "Type")
                            .Select(p => p.mtName
                            )
                            .ToList()
                        );

        }

        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ceItemPartName)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";
            string vStatus = "";
            string[] chkPermis;
            string[] chkStatus;
            string[] chkSave;
            string[] chkSaveHistory;
            string[] chkSaveSendMail;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;
            try
            {
                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                i_Step = @class._ViewceMastMoldOtherRequest != null ? @class._ViewceMastMoldOtherRequest.mrStep : 0;

                //check step 3 when 4 doc done
                if (i_Step == 2) //Waiting Checked By WORKING TIME (OPG) , MATERIAL(DG MOLD), TOOL(CAM), INFORMATION(DRG)
                {
                    chkStatus = chkStatusDoc(@class);
                    if (chkStatus[0] == "W")
                    {
                        config = chkStatus[0];
                        msg = chkStatus[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                }



                //if(@class._ViewceHistoryApproved == null) { @class._ViewceHistoryApproved = new ViewceHistoryApproved(); } //step 2 case disapprove
                if (i_Step == 1)
                {
                    i_Step = i_Step + 1;
                    for (int i = 0; i < @class._ListViewceHistoryApproved.Count(); i++)
                    {
                        if (@class._ListViewceHistoryApproved[i].htTo != null || (@class._ListViewceHistoryApproved[i].htTo == null && @class._ListViewceHistoryApproved[0].htStatus == "Disapprove"))
                        {
                            if (@class._ListViewceHistoryApproved[0].htStatus == "Approve") //0 เป็นตัวหลักที่เก็บ ของ step 1
                            {
                                // i_Step = i_Step + 1;
                                config = "S";

                                ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).FirstOrDefault();
                                if (fromEmailTO == null)
                                {
                                    config = "E";
                                    msg = "Please Check your Email to , Email incorrect !!!";
                                }

                            }
                            else if (@class._ListViewceHistoryApproved[0].htStatus == "Disapprove")
                            {
                                i_Step = 9;
                                config = "S";
                                //string v_empissue = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == @class._ViewceMastMoldOtherRequest.mrDocmentNo).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                                //@class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).FirstOrDefault();

                            }
                            else
                            {
                                config = "E";
                                msg = "Please input Status";
                            }
                        }
                        else
                        {
                            config = "E";
                            msg = "Please input Status";

                        }
                    }

                }

                else
                {
                    if (@class._ViewceHistoryApproved.htTo != null || (@class._ViewceHistoryApproved.htTo == null && @class._ViewceHistoryApproved.htStatus == "Disapprove"))
                    {
                        if (@class._ViewceHistoryApproved.htStatus == "Approve")
                        {
                            i_Step = i_Step + 1;
                            config = "S";

                            ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();
                            if (fromEmailTO == null)
                            {
                                config = "E";
                                msg = "Please Check your Email to , Email incorrect !!!";
                            }

                        }
                        else if (@class._ViewceHistoryApproved.htStatus == "Disapprove")
                        {
                            i_Step = 9;
                            config = "S";
                            string v_empissue = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == @class._ViewceMastMoldOtherRequest.mrDocmentNo).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                            @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                        }
                        else
                        {
                            config = "E";
                            msg = "Please input Status";
                        }
                    }
                    else
                    {
                        config = "E";
                        msg = "Please input e-mail.";

                    }
                }
                if (config == "S")
                {


                    vRunDoc = RunDocNo(@class);
                    if (vRunDoc[0] == "Fail")
                    {
                        config = "E";
                        msg = "Error Run Doc No : " + vRunDoc[1];
                        return Json(new { c1 = config, c2 = msg });
                    }

                    //check step 1 
                    if (_ceItemPartName != null)
                    {
                        @class._ListViewceItemPartName = JsonConvert.DeserializeObject<List<ViewceItemPartName>>(_ceItemPartName);
                    }


                    chkSave = Save(@class, i_Step, vRunDoc[0], vRunDoc[1], files, "S");
                    if (chkSave[0] == "E")
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                    }


                    //save history
                    chkSaveHistory = SaveHistory(@class, i_Step, vRunDoc[1]);
                    if (chkSave[0] == "E")
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                    }

                    //send mail
                    chkSaveSendMail = SendMailHistory(@class, i_Step, vRunDoc[0], vRunDoc[1]);
                    if (chkSaveSendMail[0] == "E")
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                    }







                }
                else
                {
                    config = "E";
                    //msg = msg;
                    return Json(new { c1 = config, c2 = msg });
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new
                {
                    c1 = config,
                    c2 = msg
                });
            }


            return Json(new { c1 = config, c2 = msg });
        }

        public string[] chkStatusDoc(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            try
            {
                string vDoc = @class._ViewceMastMoldOtherRequest.mrDocmentNo;
                int vstepWK = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == vDoc).Select(x => x.wrStep).FirstOrDefault();
                int vstepMT = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == vDoc).Select(x => x.mrStep).FirstOrDefault();
                int vstepTGR = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == vDoc).Select(x => x.trStep).FirstOrDefault();
                int vstepSP = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == vDoc).Select(x => x.irStep).FirstOrDefault();


                if (vstepWK == 4 && vstepMT == 4 && vstepTGR == 4 && vstepSP == 4)
                {
                    status_per = "S";
                    message_per = "You have permission ";
                }
                else
                {
                    //Waiting Checked By WORKING TIME (OPG) , MATERIAL(DG MOLD), TOOL(CAM), INFORMATION(DRG)
                    status_per = "W";
                    message_per = "Please complete all WORKING TIME(OPG), MATERIAL(DG MOLD), TOOL(CAM), INFORMATION(DRG) before the next step.";
                }






                string[] returnvar = { status_per, message_per };
                return returnvar;
            }
            catch (Exception ex)
            {
                string[] returnvar = { status_per, message_per };
                return returnvar;

            }
        }



        public string[] chkPermission(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            var chkData = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == @class._ViewceMastMoldOtherRequest.mrDocmentNo).FirstOrDefault();
            try
            {

                if (chkData != null)
                {
                    //check operator //check create user
                    if (chkData.mrStep == 0 && _UserId == chkData.mrEmpCodeRequest)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else if (_UserId == chkData.mrEmpCodeApprove)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else if (chkData.mrStep == 8 && _Permiss.ToUpper() == "ADMIN")
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else
                    {
                        status_per = "P";
                        message_per = "You don't have permission to access";
                    }
                }
                else
                {
                    status_per = "S";
                    message_per = "You have permission ";
                }

                string[] returnvar = { status_per, message_per };
                return returnvar;
            }
            catch (Exception ex)
            {
                string[] returnvar = { status_per, message_per };
                return returnvar;

            }
        }
        public string[] RunDocNo(Class @class)
        {

            string v_msg = "";
            string v_rundoc = "";
            int i_rundoc = 0;

            string vIssue = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string vDocCode = "CE";
            string vDocSub = "O";
            string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
            string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);

            try
            {
                //check update revision or new
                if (@class._ViewceMastMoldOtherRequest.mrDocmentNo != null && @class._ViewceMastMoldOtherRequest.mrDocmentNo != "")
                {

                    v_msg = "Update";
                    v_rundoc = @class._ViewceMastMoldOtherRequest.mrDocmentNo;
                    // v_rundoc = "CE" + "S" + "-" + vYY + "-" + vMM + String.Format("{0:D3}", i_rundoc);
                }
                else
                {
                    //CE-S-25-03-001 10,3
                    i_rundoc = _MK._ViewceRunDocument.Where(x => x.rmDocCode == vDocCode && x.rmDocSub == vDocSub && x.rmYear == vYY && x.rmMonth == vMM).OrderByDescending(x => x.rmRunNo).Select(x => x.rmRunNo).FirstOrDefault();
                    v_msg = "New";
                    // i_rundoc = i_rundoc > 0 ? i_rundoc + 1 : 0;
                    if (i_rundoc > 0)
                    {
                        v_rundoc = "CE" + "-" + "O" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", i_rundoc + 1);
                    }
                    else
                    {
                        v_rundoc = "CE" + "-" + "O" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", 1);
                    }

                    //v_rundoc = "CE" + "S" + "-" + vYY + "-" + vMM + String.Format("{0:D3}", i_rundoc);

                }

            }
            catch (Exception ex)
            {
                v_msg = "Fail";
                v_rundoc = ex.Message;
            }


            string[] vRevision = { v_msg, v_rundoc };
            return vRevision;
        }
        public string[] Save(Class @class, int vstep, string status, string RunDoc, List<IFormFile> files, string savetype)
        {
            string empissue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string UpdateBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            //User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
            string v_msg = "";
            string v_status = "";



            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string vDocCode = "CE";
                    string vDocSub = "O";
                    string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
                    string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);
                    int vRevision = @class._ViewceMastMoldOtherRequest.mrRevision; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

                    string empIssue = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).Select(x => x.emEmpcode).First() :
                                        @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest == null ? empissue : @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest;
                    string NickNameIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.NICKNAME).First();
                    string DeptIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.DEPT_CODE).First();


                    string empApprove = vstep == 9 ? empIssue :
                                        @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() :
                                        @class._ViewceMastMoldOtherRequest.mrEmpCodeApprove != null ? @class._ViewceMastMoldOtherRequest.mrEmpCodeApprove : empIssue;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastMoldOtherRequest.mrNameApprove;


                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "3").Select(x => x.mfSubject).First();

                    //case dis approve
                    vstep = vstep == 9 ? vstep = 0 : vstep;

                    // status New  // status Update
                    if (status == "New")
                    {
                        //save run doc //CE-S-25-03-001 10,3
                        ViewceRunDocument _ViewceRunDocument = new ViewceRunDocument();
                        _ViewceRunDocument.rmRunNo = int.Parse(RunDoc.Substring(11, 3));
                        _ViewceRunDocument.rmDocCode = vDocCode;
                        _ViewceRunDocument.rmDocSub = vDocSub;
                        _ViewceRunDocument.rmYear = vYY;
                        _ViewceRunDocument.rmMonth = vMM;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _ViewceRunDocument.rmIssueBy = UpdateBy;
                        _MK._ViewceRunDocument.AddAsync(_ViewceRunDocument);


                        //check new mrRevision by  Customer Name Function Model Name
                        //int n_Rev = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrCustomerName == @class._ViewceMastMoldOtherRequest.mrCustomerName &&
                        //                                                  x.mrFunction == @class._ViewceMastMoldOtherRequest.mrFunction &&
                        //                                                  x.mrModelName == @class._ViewceMastMoldOtherRequest.mrModelName).Select(x => (x.mrRevision)).FirstOrDefault();



                        var vRev = _MK._ViewceMastMoldOtherRequest
                                            .Where(x => x.mrCustomerName == @class._ViewceMastMoldOtherRequest.mrCustomerName
                                                     && x.mrFunction == @class._ViewceMastMoldOtherRequest.mrFunction
                                                     && x.mrModelName == @class._ViewceMastMoldOtherRequest.mrModelName)
                                            .Select(x => (int?)x.mrRevision) // ใช้ nullable int ป้องกัน null
                                            .Max();

                        int vrevision = vRev == null ? 0 : vRev.Value + 1;



                        ViewceMastMoldOtherRequest _ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
                        _ViewceMastMoldOtherRequest.mrDocmentNo = RunDoc;
                        _ViewceMastMoldOtherRequest.mrRevision = vrevision;//  @class._ViewceMastMoldOtherRequest.mrRevision;
                        _ViewceMastMoldOtherRequest.mrCustomerName = @class._ViewceMastMoldOtherRequest.mrCustomerName;
                        _ViewceMastMoldOtherRequest.mrFunction = @class._ViewceMastMoldOtherRequest.mrFunction;
                        _ViewceMastMoldOtherRequest.mrModelName = @class._ViewceMastMoldOtherRequest.mrModelName;
                        _ViewceMastMoldOtherRequest.mrEvent = @class._ViewceMastMoldOtherRequest.mrEvent;
                        _ViewceMastMoldOtherRequest.mrMoldGo = @class._ViewceMastMoldOtherRequest.mrMoldGo;
                        _ViewceMastMoldOtherRequest.mrTry1 = @class._ViewceMastMoldOtherRequest.mrTry1;
                        _ViewceMastMoldOtherRequest.mrMoldMass = @class._ViewceMastMoldOtherRequest.mrMoldMass;
                        _ViewceMastMoldOtherRequest.mrType = @class._ViewceMastMoldOtherRequest.mrType;
                        _ViewceMastMoldOtherRequest.mrIssueDate = @class._ViewceMastMoldOtherRequest.mrIssueDate;
                        _ViewceMastMoldOtherRequest.mrStep = vstep;
                        _ViewceMastMoldOtherRequest.mrStatus = _smStatus;
                        _ViewceMastMoldOtherRequest.mrEmpCodeRequest = empIssue;
                        _ViewceMastMoldOtherRequest.mrNameRequest = NickNameIssue;
                        _ViewceMastMoldOtherRequest.mrEmpCodeApprove = savetype == "D" ? "" : empApprove;
                        _ViewceMastMoldOtherRequest.mrNameApprove = savetype == "D" ? "" : NickNameApprove;
                        _ViewceMastMoldOtherRequest.mrFlowNo = 3;
                        _MK._ViewceMastMoldOtherRequest.AddAsync(_ViewceMastMoldOtherRequest);



                        _MK.SaveChanges();
                    }

                    // status Old
                    else if (status == "Update")
                    {

                        if (@class._ViewceMastMoldOtherRequest.mrStep == 1)
                        //step แจกจ่าย
                        //master
                        //working time
                        //Material
                        //tool & gr
                        //infor spac
                        {

                            //var itemItemPartName = _MK._ViewceItemPartName.Where(p => p.ipDocumentNo == RunDoc).ToList();
                            //if (itemItemPartName.Count > 0)
                            //{
                            //    _MK._ViewceItemPartName.RemoveRange(itemItemPartName);
                            //    _MK.SaveChanges();

                            //    if (@class._ListViewceItemPartName.Count > 0)
                            //    {

                            //        for (int i = 0; i < @class._ListViewceItemPartName.Count(); i++)
                            //        {
                            //            ViewceItemPartName _ViewceItemPartName = new ViewceItemPartName();
                            //            _ViewceItemPartName.ipDocumentNo = RunDoc;
                            //            _ViewceItemPartName.ipRunNo = @class._ListViewceItemPartName[i].ipRunNo;
                            //            _ViewceItemPartName.ipPartName = @class._ListViewceItemPartName[i].ipPartName;
                            //            _ViewceItemPartName.ipCavityNo = @class._ListViewceItemPartName[i].ipCavityNo;
                            //            _ViewceItemPartName.ipTypeCavity = @class._ListViewceItemPartName[i].ipTypeCavity;
                            //            _MK._ViewceItemPartName.AddAsync(_ViewceItemPartName);
                            //            _MK.SaveChanges();
                            //        }
                            //    }

                            //}
                            //else
                            //{
                            //    if (@class._ListViewceItemPartName.Count > 0)
                            //    {

                            //        for (int i = 0; i < @class._ListViewceItemPartName.Count(); i++)
                            //        {
                            //            ViewceItemPartName _ViewceItemPartName = new ViewceItemPartName();
                            //            _ViewceItemPartName.ipDocumentNo = RunDoc;
                            //            _ViewceItemPartName.ipRunNo = i + 1;// @class._ListViewceItemPartName[i].ipRunNo;
                            //            _ViewceItemPartName.ipPartName = @class._ListViewceItemPartName[i].ipPartName;
                            //            _ViewceItemPartName.ipCavityNo = @class._ListViewceItemPartName[i].ipCavityNo;
                            //            _ViewceItemPartName.ipTypeCavity = @class._ListViewceItemPartName[i].ipTypeCavity;
                            //            _MK._ViewceItemPartName.AddAsync(_ViewceItemPartName);
                            //            _MK.SaveChanges();
                            //        }
                            //    }
                            //}

                            var itemItemPartName = _MK._ViewceItemPartName.Where(p => p.ipDocumentNo == RunDoc).ToList();

                            if (itemItemPartName.Any())
                            {
                                _MK._ViewceItemPartName.RemoveRange(itemItemPartName);
                                _MK.SaveChanges();
                            }

                            if (@class._ListViewceItemPartName.Any())
                            {
                                for (int i = 0; i < @class._ListViewceItemPartName.Count; i++)
                                {
                                    var part = @class._ListViewceItemPartName[i];
                                    var _ViewceItemPartName = new ViewceItemPartName
                                    {
                                        ipDocumentNo = RunDoc,
                                        ipRunNo = itemItemPartName.Any() ? part.ipRunNo : i + 1,
                                        ipPartName = part.ipPartName,
                                        ipCavityNo = part.ipCavityNo,
                                        ipRateReport = part.ipRateReport,
                                        ipTypeCavity = part.ipTypeCavity
                                    };
                                    _MK._ViewceItemPartName.AddAsync(_ViewceItemPartName);
                                    _MK.SaveChanges();


                                    //_MK._ViewceItemPartName.Add(_ViewceItemPartName);
                                }
                                //  _MK.SaveChanges();
                            }



                            //working time 0 flow 4
                            //Material 1 flow 5
                            //tool & gr 2 flow 6
                            //infor spac 3 flow 7
                            //savetype == "D" savedraft
                            if (savetype == "S" && vstep != 0)
                            {
                                for (int i = 0; i < @class._ListViewceHistoryApproved.Count(); i++)
                                {


                                    string empSubIssue = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htFrom).Select(x => x.emEmpcode).First() :
                                          @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest == null ? empissue : @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest;
                                    // string empSubIssue = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htFrom).Select(x => x.emEmpcode).First() : "";
                                    string NickNameSubIssue = empSubIssue != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubIssue).Select(x => x.NICKNAME).First() : "";

                                    string _smsubStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == 1 && x.mfFlowNo == (i + 4).ToString()).Select(x => x.mfSubject).First();

                                    string empSubApprove = "";// @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastMoldOtherRequest.mrEmpCodeApprove;
                                    string NickNameSubApprove = "";// empSubApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubApprove).Select(x => x.NICKNAME).First() : "";


                                    //save 4 table Working time,Material,Tool& GR,Information Spec
                                    if (i == 0)
                                    {
                                        empSubApprove = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).Select(x => x.emEmpcode).First() :
                                                       _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == RunDoc).FirstOrDefault() == null ?
                                                       empSubIssue :
                                                       _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == RunDoc).Select(x => x.wrEmpCodeApprove).FirstOrDefault();
                                        ;
                                        NickNameSubApprove = empSubApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubApprove).Select(x => x.NICKNAME).First() : "";

                                        var _ceMastWorkingTimeRequest = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == RunDoc).FirstOrDefault();
                                        if (_ceMastWorkingTimeRequest != null)
                                        {
                                            _MK._ViewceMastWorkingTimeRequest.Remove(_ceMastWorkingTimeRequest);
                                            _MK.SaveChanges();
                                        }

                                        ViewceMastWorkingTimeRequest _ViewceMastWorkingTimeRequest = new ViewceMastWorkingTimeRequest();
                                        _ViewceMastWorkingTimeRequest.wrDocumentNo = RunDoc;
                                        _ViewceMastWorkingTimeRequest.wrDocumentNoSub = RunDoc + "-W";
                                        _ViewceMastWorkingTimeRequest.wrIssueDate = IssueBy;
                                        _ViewceMastWorkingTimeRequest.wrStep = 1;//vstep;// 1;
                                        _ViewceMastWorkingTimeRequest.wrStatus = _smsubStatus;
                                        _ViewceMastWorkingTimeRequest.wrEmpCodeRequest = empSubIssue;
                                        _ViewceMastWorkingTimeRequest.wrNameRequest = NickNameSubIssue;
                                        _ViewceMastWorkingTimeRequest.wrEmpCodeApprove = empSubApprove;
                                        _ViewceMastWorkingTimeRequest.wrNameApprove = NickNameSubApprove;
                                        _ViewceMastWorkingTimeRequest.wrFlowNo = i + 4;
                                        _MK._ViewceMastWorkingTimeRequest.AddAsync(_ViewceMastWorkingTimeRequest);
                                        _MK.SaveChanges();


                                    }
                                    else if (i == 1)
                                    {
                                        empSubApprove = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).Select(x => x.emEmpcode).First() :
                                                        _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == RunDoc).FirstOrDefault() == null ?
                                                        empSubIssue :
                                                        _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == RunDoc).Select(x => x.mrEmpCodeApprove).FirstOrDefault();
                                        ;
                                        NickNameSubApprove = empSubApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubApprove).Select(x => x.NICKNAME).First() : "";

                                        var _ceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == RunDoc).FirstOrDefault();
                                        if (_ceMastMaterialRequest != null)
                                        {
                                            _MK._ViewceMastMaterialRequest.Remove(_ceMastMaterialRequest);
                                            _MK.SaveChanges();
                                        }

                                        ViewceMastMaterialRequest _ViewceMastMaterialRequest = new ViewceMastMaterialRequest();
                                        _ViewceMastMaterialRequest.mrDocumentNo = RunDoc;
                                        _ViewceMastMaterialRequest.mrDocumentNoSub = RunDoc + "-M";
                                        _ViewceMastMaterialRequest.mrIssueDate = IssueBy;
                                        _ViewceMastMaterialRequest.mrStep = 1;//vstep;// 1;
                                        _ViewceMastMaterialRequest.mrStatus = _smsubStatus;
                                        _ViewceMastMaterialRequest.mrEmpCodeRequest = empSubIssue;
                                        _ViewceMastMaterialRequest.mrNameRequest = NickNameSubIssue;
                                        _ViewceMastMaterialRequest.mrEmpCodeApprove = empSubApprove;
                                        _ViewceMastMaterialRequest.mrNameApprove = NickNameSubApprove;
                                        _ViewceMastMaterialRequest.mrFlowNo = i + 4;
                                        _MK._ViewceMastMaterialRequest.AddAsync(_ViewceMastMaterialRequest);
                                        _MK.SaveChanges();



                                    }
                                    else if (i == 2)
                                    {
                                        empSubApprove = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).Select(x => x.emEmpcode).First() :
                                                      _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == RunDoc).FirstOrDefault() == null ?
                                                      empSubIssue :
                                                      _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == RunDoc).Select(x => x.trEmpCodeApprove).FirstOrDefault();
                                        ;
                                        NickNameSubApprove = empSubApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubApprove).Select(x => x.NICKNAME).First() : "";

                                        var _ceMastToolGRRequest = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == RunDoc).FirstOrDefault();
                                        if (_ceMastToolGRRequest != null)
                                        {
                                            _MK._ViewceMastToolGRRequest.Remove(_ceMastToolGRRequest);
                                            _MK.SaveChanges();
                                        }

                                        //if (_ceMastToolGRRequest == null)
                                        //{ }
                                        ViewceMastToolGRRequest _ViewceMastToolGRRequest = new ViewceMastToolGRRequest();
                                        _ViewceMastToolGRRequest.trDocumentNo = RunDoc;
                                        _ViewceMastToolGRRequest.trDocumentNoSub = RunDoc + "-T";
                                        _ViewceMastToolGRRequest.trIssueDate = IssueBy;
                                        _ViewceMastToolGRRequest.trStep = 1;//vstep;//1;
                                        _ViewceMastToolGRRequest.trStatus = _smsubStatus;
                                        _ViewceMastToolGRRequest.trEmpCodeRequest = empSubIssue;
                                        _ViewceMastToolGRRequest.trNameRequest = NickNameSubIssue;
                                        _ViewceMastToolGRRequest.trEmpCodeApprove = empSubApprove;
                                        _ViewceMastToolGRRequest.trNameApprove = NickNameSubApprove;
                                        _ViewceMastToolGRRequest.trFlowNo = i + 4;
                                        _MK._ViewceMastToolGRRequest.AddAsync(_ViewceMastToolGRRequest);
                                        _MK.SaveChanges();

                                    }
                                    else if (i == 3)
                                    {
                                        empSubApprove = @class._ListViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo).Select(x => x.emEmpcode).First() :
                                                     _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == RunDoc).FirstOrDefault() == null ?
                                                     empSubIssue :
                                                     _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == RunDoc).Select(x => x.irEmpCodeApprove).FirstOrDefault();
                                        ;
                                        NickNameSubApprove = empSubApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empSubApprove).Select(x => x.NICKNAME).First() : "";

                                        var _ceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == RunDoc).FirstOrDefault();
                                        if (_ceMastInforSpacMoldRequest != null)
                                        {
                                            _MK._ViewceMastInforSpacMoldRequest.Remove(_ceMastInforSpacMoldRequest);
                                            _MK.SaveChanges();
                                        }
                                        //if (_ceMastInforSpacMoldRequest == null)
                                        //{ }
                                        ViewceMastInforSpacMoldRequest _ViewceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
                                        _ViewceMastInforSpacMoldRequest.irDocumentNo = RunDoc;
                                        _ViewceMastInforSpacMoldRequest.irDocumentNoSub = RunDoc + "-I";
                                        _ViewceMastInforSpacMoldRequest.irIssueDate = IssueBy;
                                        _ViewceMastInforSpacMoldRequest.irStep = 1;//vstep;// 1;
                                        _ViewceMastInforSpacMoldRequest.irStatus = _smsubStatus;
                                        _ViewceMastInforSpacMoldRequest.irEmpCodeRequest = empSubIssue;
                                        _ViewceMastInforSpacMoldRequest.irNameRequest = NickNameSubIssue;
                                        _ViewceMastInforSpacMoldRequest.irEmpCodeApprove = empSubApprove;
                                        _ViewceMastInforSpacMoldRequest.irNameApprove = NickNameSubApprove;
                                        _ViewceMastInforSpacMoldRequest.irFlowNo = i + 4;
                                        _MK._ViewceMastInforSpacMoldRequest.AddAsync(_ViewceMastInforSpacMoldRequest);
                                        _MK.SaveChanges();


                                    }


                                }
                            }






                        }

                        //string empMainApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() :
                        //                      _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == RunDoc).Select(x => x.mrEmpCodeApprove).FirstOrDefault() == null ?
                        //                       _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == RunDoc).Select(x => x.mrEmpCodeRequest).FirstOrDefault()
                        //                      : _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == RunDoc).Select(x => x.mrEmpCodeApprove).FirstOrDefault();
                        //string NickNameMainApprove = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empMainApprove).Select(x => x.NICKNAME).First();



                        var vOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == RunDoc).FirstOrDefault();
                        //vOtherRequest.mrDocmentNo = RunDoc;
                        vOtherRequest.mrRevision = @class._ViewceMastMoldOtherRequest.mrRevision;
                        vOtherRequest.mrCustomerName = @class._ViewceMastMoldOtherRequest.mrCustomerName;
                        vOtherRequest.mrFunction = @class._ViewceMastMoldOtherRequest.mrFunction;
                        vOtherRequest.mrModelName = @class._ViewceMastMoldOtherRequest.mrModelName;
                        vOtherRequest.mrEvent = @class._ViewceMastMoldOtherRequest.mrEvent;
                        vOtherRequest.mrMoldGo = @class._ViewceMastMoldOtherRequest.mrMoldGo;
                        vOtherRequest.mrTry1 = @class._ViewceMastMoldOtherRequest.mrTry1;
                        vOtherRequest.mrMoldMass = @class._ViewceMastMoldOtherRequest.mrMoldMass;
                        vOtherRequest.mrType = @class._ViewceMastMoldOtherRequest.mrType;
                        vOtherRequest.mrIssueDate = vstep == 0 ? "" : @class._ViewceMastMoldOtherRequest.mrIssueDate;
                        vOtherRequest.mrStep = vstep;
                        vOtherRequest.mrStatus = _smStatus;
                        //vOtherRequest.mrEmpCodeRequest = empIssue;
                        //vOtherRequest.mrNameRequest = NickNameIssue;
                        vOtherRequest.mrEmpCodeApprove = empApprove;// empMainApprove;
                        vOtherRequest.mrNameApprove = NickNameApprove;//NickNameMainApprove;
                        _MK._ViewceMastMoldOtherRequest.Update(vOtherRequest);
                        _MK.SaveChanges();


                    }


                    //new revision
                    if (vstep == 8) //chk step == finish insert to table ceMastChartRateOtherReport
                    {
                        var ceMastChartRateOtherReport = _MK._ViewceMastChartRateOtherReport.Where(x => x.crDocumentNo == RunDoc).ToList();

                        if (ceMastChartRateOtherReport.Any())
                        {
                            _MK._ViewceMastChartRateOtherReport.RemoveRange(ceMastChartRateOtherReport);
                            _MK.SaveChanges();
                        }
                        //insert to 
                        var ceMastCostModel = _MK._ViewceMastCostModel.Where(x => x.mcModelName == @class._ViewceMastMoldOtherRequest.mrModelName).OrderByDescending(x => x.mcCostPlanningNo).Select(x => x.mcCostPlanningNo).FirstOrDefault();
                        ViewceMastChartRateOtherReport _ViewceMastChartRateOtherReport = new ViewceMastChartRateOtherReport();
                        //_ViewceMastChartRateOtherReport.crRunno = 1; run auto
                        _ViewceMastChartRateOtherReport.crDocumentNo = RunDoc;
                        _ViewceMastChartRateOtherReport.crCostPlanningNo = ceMastCostModel;
                        _MK._ViewceMastChartRateOtherReport.AddAsync(_ViewceMastChartRateOtherReport);
                        _MK.SaveChanges();

                    }


                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    string[] v_statusFile = savefile(@class, files, RunDoc);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {

                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;
                }
            }

            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }
        public string[] savefile(Class @class, List<IFormFile> file, string RunDoc)
        {
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string fileName = "";
            string v_error = "";
            string v_fnType = "";// v_type != "" ? v_type : @class._ViewsvsServiceRequest.srType;

            string v_status = "";
            string v_msg = "";
            int v_count = 0;
            try
            {
                if (file is null)
                {
                    v_status = "S";
                    v_msg = "Save & Send Mail Already";
                }
                else
                {
                    List<ViewAttachment> listInsert = new List<ViewAttachment>();
                    using (var dbContextTransaction = _IT.Database.BeginTransaction())
                    {
                        try
                        {
                            if (file is null)
                            {
                                //vStatus = "File not found";
                                v_status = "S";
                                v_msg = "Save & Send Mail Already";
                            }
                            else
                            {
                                for (int i = 0; i < file.Count; i++)
                                {
                                    string IssueDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    fileName = IssueDate + "-" + file[i].FileName;//System.IO.Path.GetExtension(file.FileName).ToLower();
                                    string filePath = path + fileName;
                                    var fileLocation = new FileInfo(filePath);
                                    //filePaths.Add(filePath);
                                    if (!Directory.Exists(filePath))
                                    {
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file[i].CopyTo(stream);
                                        }
                                    }
                                    ViewAttachment _viewAttachment = new ViewAttachment();
                                    _viewAttachment.fnNo = RunDoc; // @class._ViewceMastSubMakerRequest.smRevision;//vSno.ToString();
                                    _viewAttachment.fnPath = filePath;
                                    _viewAttachment.fnFilename = fileName;
                                    _viewAttachment.fnIssueBy = IssueBy;
                                    _viewAttachment.fnUpdateBy = IssueBy;
                                    _viewAttachment.fnType = v_fnType;
                                    _viewAttachment.fnProgram = PgName; //Program  name CostEstimate
                                                                        //_IT.Attachment.AddAsync(_viewAttachment);
                                                                        //_IT.SaveChanges();
                                                                        //vStatus = fileName;
                                    listInsert.Add(_viewAttachment);
                                }

                            }
                            _IT.Attachment.AddRange(listInsert); // Add หลายรายการ
                            _IT.SaveChanges(); // Save ทีเดียว
                            dbContextTransaction.Commit();
                            v_status = "S";
                            v_msg = "Save & Send Mail Already";
                        }
                        catch (Exception e)
                        {
                            v_status = "E";
                            v_error = e.Message;
                            v_msg = "Error Save file :" + e.Message;
                            //dbContextTransaction.Rollback();
                            try
                            {
                                dbContextTransaction.Rollback();
                            }
                            catch
                            {
                                // ignore ถ้า transaction ปิดไปแล้ว
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                v_status = "E";
                v_error = e.Message;
                v_msg = "Error Save file :" + e.Message;

            }
            v_count = v_count;
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public string[] SendMailHistory(Class @class, int vstep, string status, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            string vEmpCodeCCemail = "";
            string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            int vRevision = @class._ViewceMastMoldOtherRequest.mrRevision; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

            //string Empcode_IssueBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
            string v_EmpCodeRequest = @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest == null || @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest == ""
                                                                                ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                : @class._ViewceMastMoldOtherRequest.mrEmpCodeRequest + " : " + @class._ViewceMastMoldOtherRequest.mrNameRequest;


            try
            {
                string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "3").Select(x => x.mfSubject).First();
                vstep = vstep == 9 ? vstep = 0 : vstep;

                List<string> _listStep2Type = new List<string> { "Working Time", "MATERIAL", "Tool &GR", "INFORMATION  SPEC MOLD" };
                List<string> _listStep2Type1 = new List<string> { "W", "M", "T", "I" };

                if (vstep == 0)
                {
                    if (@class._ViewceHistoryApproved == null)
                    {
                        @class._ViewceHistoryApproved = new ViewceHistoryApproved();
                        @class._ViewceHistoryApproved.htFrom = @class._ListViewceHistoryApproved[0].htFrom;
                        string v_empissue = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == @class._ViewceMastMoldOtherRequest.mrDocmentNo).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                        @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                        @class._ViewceHistoryApproved.htCC = @class._ListViewceHistoryApproved[0].htCC;

                    }

                }

                if (@class._ViewceMastMoldOtherRequest.mrStep == 1 && vstep != 0)
                {

                    for (int i = 0; i < @class._ListViewceHistoryApproved.Count(); i++)
                    {
                        MimeMessage email = new MimeMessage();

                        // from / to
                        ViewrpEmail fromEmailFrom = _IT.rpEmails
                            .Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htFrom)
                            .FirstOrDefault();

                        ViewrpEmail fromEmailTO = _IT.rpEmails
                            .Where(w => w.emName_M365 == @class._ListViewceHistoryApproved[i].htTo)
                            .FirstOrDefault();

                        MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                        MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);

                        email.Subject = "CostEstimate Mold Other Request ==> " + _smStatus;
                        email.From.Add(FromMailFrom);
                        email.To.Add(FromMailTO);

                        // CC
                        if (@class._ListViewceHistoryApproved[i].htCC != null)
                        {
                            string[] splitCC = @class._ListViewceHistoryApproved[i].htCC.Split(',');
                            foreach (var cc in splitCC)
                            {
                                if (!string.IsNullOrWhiteSpace(cc))
                                {
                                    var fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == cc).FirstOrDefault();
                                    if (fromEmailCC != null)
                                    {
                                        MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                        email.Cc.Add(FromMailcc);
                                    }
                                }
                            }
                        }

                        // body
                        var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther&subType=" + _listStep2Type1[i].ToString();
                        var bodyBuilder = new BodyBuilder();
                        string EmailBody = $"<div>" +
                      $"<B>Cost Estimate : Mold Other : Type => " + _listStep2Type[i].ToString() + " </B> <br>" +
                      $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                      $"<B>Customer Name : </B> " + @class._ViewceMastMoldOtherRequest.mrCustomerName + "<br>" +
                      $"<B>Function : </B> " + @class._ViewceMastMoldOtherRequest.mrFunction + "<br>" +
                      $"<B>Model Name : </B> " + @class._ViewceMastMoldOtherRequest.mrModelName + "<br>" +
                      $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                      $"<B>Status : </B> " + _smStatus + "<br> " +
                      $"<B> หมายเหตุ : </B> " + @class._ListViewceHistoryApproved[i].htRemark + "<br> " +
                      $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                      $"</a>" +
                      $"</div>";

                        bodyBuilder.HtmlBody = EmailBody;
                        email.Body = bodyBuilder.ToMessageBody();

                        // send
                        using (var smtp1 = new SmtpClient())
                        {
                            smtp1.Connect("203.146.237.138");
                            smtp1.Send(email);
                            smtp1.Disconnect(true);
                        }
                    }
                }
                else
                {
                    var email = new MimeMessage();
                    ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                    ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                    MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                    MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                    email.Subject = "CostEstimate Mold Other Request==> " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
                    //email.From.Add(MailboxAddress.Parse(_ViewlrHistoryApprove.htFrom));
                    email.From.Add(FromMailFrom);
                    email.To.Add(FromMailTO);

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                    email.Cc.Add(FromMailcc);
                                    vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }
                    var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther";// + getSrNo[0].ToString();
                    var bodyBuilder = new BodyBuilder();
                    //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                    string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                    string EmailBody = $"<div>" +
                        $"<B>Cost Estimate : Mold Other </B> <br>" +
                        $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                        $"<B>Customer Name : </B> " + @class._ViewceMastMoldOtherRequest.mrCustomerName + "<br>" +
                        $"<B>Function : </B> " + @class._ViewceMastMoldOtherRequest.mrFunction + "<br>" +
                        $"<B>Revision No : </B> " + vRevision.ToString("D2") + " <br>" +
                        $"<B>Model Name : </B> " + @class._ViewceMastMoldOtherRequest.mrModelName + "<br>" +
                        $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                        $"<B>Status : </B> " + _smStatus + "<br> " +
                        $"<B> หมายเหตุ : </B> " + @class._ViewceHistoryApproved.htRemark + "<br> " +
                        $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                        $"</a>" +
                        $"</div>";

                    // bodyBuilder.Attachments.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\dev_rfc.log");

                    bodyBuilder.HtmlBody = string.Format(EmailBody);
                    email.Body = bodyBuilder.ToMessageBody();

                    // send email
                    var smtp1 = new SmtpClient();
                    //smtp.Connect("mail.csloxinfo.com");
                    smtp1.Connect("203.146.237.138");
                    //smtp.Connect("10.200.128.12");
                    smtp1.Send(email);
                    smtp1.Disconnect(true);
                }




                v_status = "S";
                v_msg = "File saved and email sent.!!!";
            }
            catch (Exception ex)
            {
                // dbContextTransaction.Rollback();

                v_status = "E";
                v_msg = "Error Save History: " + ex.Message;
            }



            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public string[] SaveHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";

            //test send mail
            string vEmpCodeCCemail = "";


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "3").Select(x => x.mfSubject).First();
                    vstep = vstep == 9 ? vstep = 0 : vstep;
                    List<string> _listType = new List<String> { "W", "M", "T", "I" };
                    if (vstep == 0)
                    {
                        if (@class._ViewceHistoryApproved == null)
                        {
                            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
                            @class._ViewceHistoryApproved.htFrom = @class._ListViewceHistoryApproved[0].htFrom;
                            string v_empissue = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == @class._ViewceMastMoldOtherRequest.mrDocmentNo).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                            @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                            @class._ViewceHistoryApproved.htCC = @class._ListViewceHistoryApproved[0].htCC;

                        }

                    }


                    if (@class._ViewceMastMoldOtherRequest.mrStep == 1 && vstep != 0) // step send mail  working MT GR,SP
                    {

                        for (int j = 0; j < @class._ListViewceHistoryApproved.Count(); j++)
                        {
                            vEmpCodeCCemail = "";
                            if (@class._ListViewceHistoryApproved[j].htCC != null)
                            {
                                ViewrpEmail fromEmailCC = new ViewrpEmail();
                                string[] splitCC = @class._ListViewceHistoryApproved[j].htCC.Split(',');
                                foreach (var i in splitCC)
                                {
                                    if (i != " " & i != "")
                                    {
                                        var v_cc = "";
                                        try
                                        {
                                            fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                            vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                        }
                                        catch (Exception e)
                                        {
                                            v_cc = e.Message;
                                        }
                                    }
                                }
                            }

                            ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                            _ViewceHistoryApproved.htDocNo = RunDoc + "-" + _listType[j];// getSrNo[0].ToString();
                            _ViewceHistoryApproved.htStep = 1;//0;// vstep;
                            _ViewceHistoryApproved.htStatus = @class._ListViewceHistoryApproved[0].htStatus;
                            _ViewceHistoryApproved.htFrom = @class._ListViewceHistoryApproved[j].htFrom;
                            _ViewceHistoryApproved.htTo = @class._ListViewceHistoryApproved[j].htTo;
                            _ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                            _ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                            _ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                            _ViewceHistoryApproved.htRemark = @class._ListViewceHistoryApproved[j].htRemark;
                            _MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);



                        }
                        _MK.SaveChanges();

                    }
                    else //case normal send
                    {
                        if (@class._ViewceHistoryApproved.htCC != null)
                        {
                            ViewrpEmail fromEmailCC = new ViewrpEmail();
                            string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                            foreach (var i in splitCC)
                            {
                                if (i != " " & i != "")
                                {
                                    var v_cc = "";
                                    try
                                    {
                                        fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                        vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                    }
                                    catch (Exception e)
                                    {
                                        v_cc = e.Message;
                                    }
                                }
                            }
                        }
                        ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                        _ViewceHistoryApproved.htDocNo = RunDoc;// getSrNo[0].ToString();
                        _ViewceHistoryApproved.htStep = vstep;
                        _ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                        _ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                        _ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                        _ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                        _ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                        _ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                        _ViewceHistoryApproved.htRemark = @class._ViewceHistoryApproved.htRemark;
                        _MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);
                        _MK.SaveChanges();
                    }



                    _MK.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    // dbContextTransaction.Rollback();
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;


                }
            }
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }


        public ActionResult DeteleDataFile(string id, string vname)
        {
            try
            {
                //var find = _IT.Attachment(X => X.)

                ViewAttachment find = _IT.Attachment.Where(x => x.fnNo == id && x.fnFilename == vname && x.fnProgram == "CostEstimate").FirstOrDefault();
                var delete = _IT.Attachment.Remove(find);


                _IT.SaveChanges();
            }
            catch
            {
                return Json(new { res = "error" });

            }
            return Json(new { res = "success" });


            //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());

        }

        [HttpPost]
        public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ceItemPartName)
        {
            string config = "S";
            string msg = "";

            string[] chkPermis;
            string[] chkStatus;
            string[] chkSave;

            int i_Step = 0;
            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;

            try
            {
                i_Step = @class._ViewceMastMoldOtherRequest.mrStep;

                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                if (i_Step == 2) //Waiting Checked By WORKING TIME (OPG) , MATERIAL(DG MOLD), TOOL(CAM), INFORMATION(DRG)
                {
                    chkStatus = chkStatusDoc(@class);
                    if (chkStatus[0] == "W")
                    {
                        config = chkStatus[0];
                        msg = chkStatus[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                }


                //check step 1 
                if (_ceItemPartName != null)
                {
                    @class._ListViewceItemPartName = JsonConvert.DeserializeObject<List<ViewceItemPartName>>(_ceItemPartName);
                }

                vRunDoc = RunDocNo(@class);
                if (vRunDoc[0] == "Fail")
                {
                    config = "E";
                    msg = "Error Run Doc No : " + vRunDoc[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                chkSave = Save(@class, i_Step, vRunDoc[0], vRunDoc[1], files, "D");
                if (chkSave[0] == "E")
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                else
                {
                    config = chkSave[0];
                    //msg = chkSave[1];
                    msg = "Save Data success ";
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;

            }
            return Json(new { c1 = config, c2 = msg });

        }



        [HttpPost]
        public PartialViewResult PrintMoldQUOTATION(string mpNo, Class @class)
        {
            try
            {


                if (mpNo != null)
                {
                    @class._ViewOperaterCP = new ViewOperaterCP();

                    string tbHistoryIssueName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryIssueEMPCODE = tbHistoryIssueName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryIssueName)).Select(x => x.emEmpcode).FirstOrDefault();

                    string tbHistoryCheckedGLName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedGLEMPCODE = tbHistoryCheckedGLName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedGLName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryCheckedName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedEMPCODE = tbHistoryCheckedName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryApproveByName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 8).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 8).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryApproveByEMPCODE = tbHistoryApproveByName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryApproveByName)).Select(x => x.emEmpcode).FirstOrDefault();



                    //    //string tbHistoryIssue3 = _IT.rpEmails.Where(u => u.emName_M365.Contains(_MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault())).Select(x => x.emEmpcode).FirstOrDefault();
                    //    //string tbHistoryIssue = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //    //    _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //    //string tbHistoryChecked = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //    //  _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //    //string tbHistoryApproveBy = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //    // _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();


                    //    //ViewAccEMPLOYEE _ViewAccEMPLOYEEIssue = new ViewAccEMPLOYEE();
                    //    //ViewAccEMPLOYEE _ViewAccEMPLOYEEChecked = new ViewAccEMPLOYEE();
                    //    //ViewAccEMPLOYEE _ViewAccEMPLOYEEApproveBy = new ViewAccEMPLOYEE();

                    //    //_ViewAccEMPLOYEEIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssue).FirstOrDefault();
                    //    //_ViewAccEMPLOYEEChecked = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryChecked).FirstOrDefault();
                    //    //_ViewAccEMPLOYEEApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveBy).FirstOrDefault();


                    //    //string tbFlowissue = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault();
                    //    //string tbFlowCheck1 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault();
                    //    //string tbFlowCheck2 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault();
                    //    //string tbFlowApprove = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault();


                    //    _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    string vIssueBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault();
                    string vCheckByTL = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vCheckByTM = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == mpNo).FirstOrDefault();
                    @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == mpNo).ToList();
                    @class._ViewOperaterCP.IssueBy = vIssueBy;
                    @class._ViewOperaterCP.CheckedByTL = vCheckByTL;
                    @class._ViewOperaterCP.CheckedByTM = vCheckByTM;
                    @class._ViewOperaterCP.ApproveBy = vApproveBy;

                    @class._ViewOperaterCP.empIssueBy = tbHistoryIssueEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTL = tbHistoryCheckedGLEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTM = tbHistoryCheckedEMPCODE;
                    @class._ViewOperaterCP.empApproveBy = tbHistoryApproveByEMPCODE;



                    @class._ListViewMoldOtherDatailQuotation = getDatailQuotation(mpNo, @class);


                    @class.rMoldGO = chgDateFormat(@class._ViewceMastMoldOtherRequest.mrMoldGo, "MM/yy",0);
                    @class.rMoldTry1 = chgDateFormat(@class._ViewceMastMoldOtherRequest.mrTry1 ,"MM/yy",0);
                    @class.rMoldMass1 = chgDateFormat(@class._ViewceMastMoldOtherRequest.mrMoldMass, "MM/yy",1);
                    @class.rMoldMass = chgDateFormat(@class._ViewceMastMoldOtherRequest.mrMoldMass, "MM/yy",0);

                    //MoldTry1
                    //MoldMass1
                    //MoldMass
                }


            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }

            return PartialView("_PartialDisplayMoldOtherQuotation", @class);


        }
        public string chgDateFormat(string vdate, string vformat,int vM)
        {
            DateTime dateValue = DateTime.ParseExact(vdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            dateValue = dateValue.AddMonths(-vM);
            string formatted = dateValue.ToString(vformat);
            return formatted;
        }


    }
}