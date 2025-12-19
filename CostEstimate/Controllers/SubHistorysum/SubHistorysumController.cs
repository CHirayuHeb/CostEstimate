using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CostEstimate.Models.Approval;
using CostEstimate.Models.Canvas;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.Table.LAMP;
//export excel
using ClosedXML.Excel;
using System.Data;
using System.IO;
using CostEstimate.Models.Table.MK;

using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CostEstimate.Controllers.SubHistorysum
{
    public class SubHistorysumController : Controller
    {
        public string[] Header_FileExport = { "No.", "Document No.", "Lot No.", "Customer Name", "Rev.No​ ", "Model Name", "Function", "Mold No. ", "Mold Name", "Cavity No.", "Development Stage", "Material Out/Date​", "Delivery/Date​", "Waiting Approve", "Status​ " };
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public SubHistorysumController(LAMP lamp, HRMS hrms, IT it, MK mk, CacheSettingController cacheController, FunctionsController callfunction)
        {
            _LAMP = lamp;
            _HRMS = hrms;
            _IT = it;
            _MK = mk;
            _Cache = cacheController;
            _callFunc = callfunction;
        }

        // เมธอดช่วยลดซ้ำ


        [Authorize("Checked")]
        public IActionResult Index(Class @class, string vLotNoMoldName)
        {
            try
            {

                string vlotNo = "";
                string vMoldName = "";

                vLotNoMoldName = vLotNoMoldName.Split('|')[0];

                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();

                @class._ViewceMastSubHistorySum = new ViewceMastSubHistorySum(); //for report main
                @class._ListViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>(); //for report main
                List<ViewceMastSubDetailHistorySum> _listViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>();
                List<ViewceMastSubDetailHistorySum> _ViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>(); //for report main

                @class._ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shLotNo == vLotNoMoldName).FirstOrDefault();
                _listViewceMastSubDetailHistorySum = _MK._ViewceMastSubDetailHistorySum.Where(x => x.sdLotNo == vLotNoMoldName).ToList();

                //@class._ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shDocNo.Contains(vLotNoMoldName)).FirstOrDefault();
                //@class._ListViewceMastSubDetailHistorySum = _MK._ViewceMastSubDetailHistorySum.Where(x => x.sdLotNo.Contains(vLotNoMoldName)).OrderBy(x=>x.sdRunNo).ToList();


                if (vLotNoMoldName != null || vLotNoMoldName != "")
                {
                    vlotNo = vLotNoMoldName.Split("|")[0];
                    //vMoldName = vLotNoMoldName.Split("|")[1];
                    @class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == vlotNo).OrderByDescending(x => x.smDocumentNo).FirstOrDefault();


                }

                List<ViewceMastProcess> _ListceMastProcess = new List<ViewceMastProcess>();
                List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                List<ViewceCostPlanning> _ViewceCostPlanning = new List<ViewceCostPlanning>();
                List<VisaulViewceDetailSubMakerRequest> _ListVisaulViewceDetailSubMakerRequest = new List<VisaulViewceDetailSubMakerRequest>(); //sum all
                @class._ListGroupViewceDetailSubMakerRequest = new List<GroupViewceDetailSubMakerRequest>();

                string v_CostPlanningNo = _MK._ViewceMastCostModel.OrderByDescending(x => x.mcCostPlanningNo).Select(x => x.mcCostPlanningNo).First();
                var v_ListceCostPlanning = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == v_CostPlanningNo).ToList();
                _ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();
                @class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();


                //start detail all
                @class._ListceMastSubMakerRequest = new List<ViewceMastSubMakerRequest>();
                @class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == vlotNo && x.smReqStatus == true && x.smStep == 7).OrderByDescending(x => x.smDocumentNo).ToList();
                @class._ListceDetailSubMakerRequest = _MK._ViewceDetailSubMakerRequest.Where(d => @class._ListceMastSubMakerRequest.Select(m => m.smDocumentNo).Contains(d.dsDocumentNo)).ToList();


                var viewList = @class._ListceDetailSubMakerRequest; // สมมุติว่าเป็น List<ViewceDetailSubMakerRequest>

                @class._ListGroupedListceDetailSub = viewList
                    .GroupBy(x => x.dsDocumentNo) // Group ชั้นที่ 1
                    .Select(group1 => new GroupedListceDetailSub
                    {
                        glDocNo = group1.Key,
                        listGroupViewceDetailSubMakerRequest = group1
                            .GroupBy(x => x.dsGroupName) // Group ชั้นที่ 2
                            .Select(group2 => new GroupedListceDetailSubMakerRequest
                            {
                                glDocNo = group2.Key,
                                gllistDetail = group2.ToList() // ชั้นสุดท้าย
                            }).ToList()
                    }).ToList();

                //end detail all


                //start sum all
                var result = @class._ListceDetailSubMakerRequest.GroupBy(x => new { x.dsGroupName, x.dsProcessName })
                            .Select(g => new
                            {
                                GroupName = g.Key.dsGroupName,
                                ProcessName = g.Key.dsProcessName,
                                TotalWTMan = g.Sum(x => x.dsWT_Man),
                                TotalWTAuto = g.Sum(x => x.dsWT_Auto)
                            })
                            .ToList();

                //check 02/08/2025
                //group  NC(CA). NC(CO).
                double sumNC_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NC(CA)") || x.sdProcessName.Contains("NC(CO)"))).Sum(x => x.sdWK_Man)
                                  : 0;
                double sumNC_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NC(CA)") || x.sdProcessName.Contains("NC(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumNC_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("NC(CA)") || x.ProcessName.Trim().Contains("NC(CO)"))).Sum(x => x.TotalWTMan);
                double sumNC_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("NC(CA)") || x.ProcessName.Trim().Contains("NC(CO)"))).Sum(x => x.TotalWTAuto);
                double sumNC_KIJWT_Man = sumNC_WK_Man - sumNC_WT_Man;
                double sumNC_KIJWT_Auto = sumNC_WK_Auto - sumNC_WT_Auto;

                //group  NCG(CA). NCG(CO).
                double sumNCG_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                                 _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NCG(CA)") || x.sdProcessName.Contains("NCG(CO)"))).Sum(x => x.sdWK_Man)
                                 : 0;
                double sumNCG_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NCG(CA)") || x.sdProcessName.Contains("NCG(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumNCG_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("NCG(CA)") || x.ProcessName.Trim().Contains("NCG(CO)"))).Sum(x => x.TotalWTMan);
                double sumNCG_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("NCG(CA)") || x.ProcessName.Trim().Contains("NCG(CO)"))).Sum(x => x.TotalWTAuto);
                double sumNCG_KIJWT_Man = sumNCG_WK_Man - sumNCG_WT_Man;
                double sumNCG_KIJWT_Auto = sumNCG_WK_Auto - sumNCG_WT_Auto;


                //group  EDM(CA). EDM(CO).
                double sumEDM_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                              _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("EDM(CA)") || x.sdProcessName.Contains("EDM(CO)"))).Sum(x => x.sdWK_Man)
                              : 0;
                double sumEDM_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("EDM(CA)") || x.sdProcessName.Contains("EDM(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumEDM_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("EDM(CA)") || x.ProcessName.Trim().Contains("EDM(CO)"))).Sum(x => x.TotalWTMan);
                double sumEDM_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("EDM(CA)") || x.ProcessName.Trim().Contains("EDM(CO)"))).Sum(x => x.TotalWTAuto);
                double sumEDM_KIJWT_Man = sumEDM_WK_Man - sumEDM_WT_Man;
                double sumEDM_KIJWT_Auto = sumEDM_WK_Auto - sumEDM_WT_Auto;




                for (int i = 0; i < v_ListceCostPlanning.Count(); i++)
                {
                    double vsdWK_Man = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Man).FirstOrDefault()
                                  : 0;
                    double vsdWK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                 _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Auto).FirstOrDefault()
                                 : 0;
                    double vsdWT_Man = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault();
                    double vsdWT_Auto = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault();

                    double sdWT_Man = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault();
                    double sdWT_Auto = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault();


                    bool sdActive_WKMan = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)) is null ? false : _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).FirstOrDefault();  //true,
                    bool sdActive_WKAuto = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)) is null ? false : _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).FirstOrDefault();



                    if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NC(CA)") || v_ListceCostPlanning[i].cpProcessName.Contains("NCG(CA)") || v_ListceCostPlanning[i].cpProcessName.Contains("EDM(CA)"))
                    {
                        vsdWK_Man = 0;
                        vsdWK_Auto = 0;
                        vsdWT_Man = 0;
                        vsdWT_Auto = 0;
                        sdWT_Man = 0;
                        sdWT_Auto = 0;
                        sdActive_WKMan = false;
                        sdActive_WKAuto = false;



                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NC(CO)"))
                    {
                        vsdWK_Man = sumNC_WK_Man;
                        vsdWK_Auto = sumNC_WK_Auto;
                        vsdWT_Man = sumNC_WT_Man;
                        vsdWT_Auto = sumNC_WT_Auto;
                        sdWT_Man = sumNC_WT_Man;
                        sdWT_Auto = sumNC_WT_Auto;
                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NCG(CO)"))
                    {
                        vsdWK_Man = sumNCG_WK_Man;
                        vsdWK_Auto = sumNCG_WK_Auto;
                        vsdWT_Man = sumNCG_WT_Man;
                        vsdWT_Auto = sumNCG_WT_Auto;
                        sdWT_Man = sumNCG_WT_Man;
                        sdWT_Auto = sumNCG_WT_Auto;
                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("EDM(CO)"))
                    {
                        vsdWK_Man = sumEDM_WK_Man;
                        vsdWK_Auto = sumEDM_WK_Auto;
                        vsdWT_Man = sumEDM_WT_Man;
                        vsdWT_Auto = sumEDM_WT_Auto;
                        sdWT_Man = sumEDM_WT_Man;
                        sdWT_Auto = sumEDM_WT_Auto;
                    }






                    @class._ListViewceMastSubDetailHistorySum.Add(new ViewceMastSubDetailHistorySum
                    {

                        sdDocNo = @class._ViewceMastSubHistorySum is null ? "" : @class._ViewceMastSubHistorySum.shDocNo,
                        sdRunNo = i + 1,
                        sdLotNo = @class._ViewceMastSubMakerRequest.smLotNo,
                        sdGroupName = v_ListceCostPlanning[i].cpGroupName.Trim(),
                        sdProcessName = v_ListceCostPlanning[i].cpProcessName,



                        sdWK_Man = vsdWK_Man,
                        sdWK_Auto = vsdWK_Auto,
                        //sdWK_Man = _listViewceMastSubDetailHistorySum != null ?
                        //   _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Man).FirstOrDefault()
                        //   : 0,
                        //sdWK_Auto = _listViewceMastSubDetailHistorySum != null ?
                        //           _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Auto).FirstOrDefault()
                        //           : 0,
                        sdActive_WKMan = sdActive_WKMan, // _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First(),  //true,
                        sdActive_WKAuto = sdActive_WKAuto,//_ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First(),

                        sdKIJWT_Man = vsdWK_Man - vsdWT_Man,
                        sdKJWT_Auto = vsdWK_Auto - vsdWT_Auto,
                        sdWT_Man = sdWT_Man,//result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault(),
                        sdWT_Auto = sdWT_Auto,//result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault(),




                    });

                }

                //_ListGroupDetailSubMakerRequestHissum
                @class._ListGroupDetailSubMakerRequestHissum = @class._ListViewceMastSubDetailHistorySum.GroupBy(p => p.sdGroupName).Select(g => new GroupDetailSubMakerRequestHissum
                {
                    GroupName = g.Key.Trim(),
                    DetailSubMakerRequest = g.ToList()
                }
                 ).ToList();
                // end sum all








            }
            catch (Exception ex)
            {
                string msg;
                msg = ex.InnerException.Message;

            }
            return View(@class);
        }

        public ActionResult SearchMastSubMarker(string term)
        {
            {

                return Json(_MK._ViewceMastSubMakerRequest.Where(p => p.smLotNo.Contains(term))
                                    .Distinct()
                                    .Select(p => p.smLotNo + "|" + p.smMoldName)
                                    .ToList());
                // return Json(_MOLD._ViewmtMaster_Mold_Control.Where(p => p.mcLedger_Number.Contains(term)).Select(p => p.mcLedger_Number + "|" + p.mcCUS + "|" + p.mcMoldname + "|" + (p.mcCavity != "" ? p.mcCavity : "0")).ToList());
            }
        }

        [Authorize("Checked")]
        [HttpPost]
        public JsonResult SaveHisSumSub(Class @class, string _ListViewceMastSubDetailHistorySum)
        {
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            string[] vRunDoc;
            string vDocCode = "CE";
            string vDocSub = "H";
            string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
            string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);

            string config = "S";
            string msg = "Data has been saved successfully";
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {

                try
                {

                    if (_ListViewceMastSubDetailHistorySum != null)
                    {
                        @class._ListViewceMastSubDetailHistorySum = JsonConvert.DeserializeObject<List<ViewceMastSubDetailHistorySum>>(_ListViewceMastSubDetailHistorySum);
                    }


                    //save ceMastSubHistorySum
                    if (@class._ViewceMastSubHistorySum.shDocNo is null) //new
                    {
                        vRunDoc = RunDocNo(@class);
                        //vRunDoc = RunDoc(@class);
                        //vRunDocNo = RunDocNo(@class);

                        if (vRunDoc[0] == "Fail")
                        {
                            config = "E";
                            msg = "Error Run Doc No : " + vRunDoc[1];
                            return Json(new { c1 = config, c2 = msg });
                        }

                        ViewceRunDocument _ViewceRunDocument = new ViewceRunDocument();
                        _ViewceRunDocument.rmRunNo = int.Parse(vRunDoc[1].Substring(11, 3));
                        _ViewceRunDocument.rmDocCode = vDocCode;
                        _ViewceRunDocument.rmDocSub = vDocSub;
                        _ViewceRunDocument.rmYear = vYY;
                        _ViewceRunDocument.rmMonth = vMM;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _MK._ViewceRunDocument.AddAsync(_ViewceRunDocument);


                        @class._ViewceMastSubHistorySum.shDocNo = vRunDoc[1];
                        @class._ViewceMastSubHistorySum.shLotNo = @class._ViewceMastSubMakerRequest.smLotNo;

                        ViewceMastSubHistorySum _ViewceMastSubHistorySum = new ViewceMastSubHistorySum();
                        _ViewceMastSubHistorySum.shDocNo = @class._ViewceMastSubHistorySum.shDocNo;
                        _ViewceMastSubHistorySum.shLotNo = @class._ViewceMastSubHistorySum.shLotNo;
                        _ViewceMastSubHistorySum.shCKjMat = @class._ViewceMastSubHistorySum.shCKjMat;
                        _ViewceMastSubHistorySum.shCKjCofficient = @class._ViewceMastSubHistorySum.shCKjCofficient;
                        _ViewceMastSubHistorySum.shCKjWorkingTime = @class._ViewceMastSubHistorySum.shCKjWorkingTime;
                        _ViewceMastSubHistorySum.shCKjTotal = @class._ViewceMastSubHistorySum.shCKjTotal;
                        _ViewceMastSubHistorySum.shCSmMat = @class._ViewceMastSubHistorySum.shCSmMat;
                        _ViewceMastSubHistorySum.shCSmCofficient = @class._ViewceMastSubHistorySum.shCSmCofficient;
                        _ViewceMastSubHistorySum.shCSmWorkingTime = @class._ViewceMastSubHistorySum.shCSmWorkingTime;
                        _ViewceMastSubHistorySum.shCSmTotal = @class._ViewceMastSubHistorySum.shCSmTotal;
                        _ViewceMastSubHistorySum.shCMcMat = @class._ViewceMastSubHistorySum.shCMcMat;
                        _ViewceMastSubHistorySum.shCMcCofficient = @class._ViewceMastSubHistorySum.shCMcCofficient;
                        _ViewceMastSubHistorySum.shCMcWorkingTime = @class._ViewceMastSubHistorySum.shCMcWorkingTime;
                        _ViewceMastSubHistorySum.shCMcTotal = @class._ViewceMastSubHistorySum.shCMcTotal;
                        _ViewceMastSubHistorySum.shStatus = @class._ViewceMastSubHistorySum.shStatus;
                        _ViewceMastSubHistorySum.shIssueBy = IssueBy;
                        _MK._ViewceMastSubHistorySum.Add(_ViewceMastSubHistorySum);
                        _MK.SaveChanges();

                    }
                    else //update
                    {
                        ViewceMastSubHistorySum _ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shDocNo == @class._ViewceMastSubHistorySum.shDocNo && x.shLotNo == @class._ViewceMastSubHistorySum.shLotNo).FirstOrDefault();
                        if (_ViewceMastSubHistorySum != null)
                        {
                            //shDocNo
                            //shLotNo
                            _ViewceMastSubHistorySum.shCKjMat = @class._ViewceMastSubHistorySum.shCKjMat;
                            _ViewceMastSubHistorySum.shCKjCofficient = @class._ViewceMastSubHistorySum.shCKjCofficient;
                            _ViewceMastSubHistorySum.shCKjWorkingTime = @class._ViewceMastSubHistorySum.shCKjWorkingTime;
                            _ViewceMastSubHistorySum.shCKjTotal = @class._ViewceMastSubHistorySum.shCKjTotal;
                            _ViewceMastSubHistorySum.shCSmMat = @class._ViewceMastSubHistorySum.shCSmMat;
                            _ViewceMastSubHistorySum.shCSmCofficient = @class._ViewceMastSubHistorySum.shCSmCofficient;
                            _ViewceMastSubHistorySum.shCSmWorkingTime = @class._ViewceMastSubHistorySum.shCSmWorkingTime;
                            _ViewceMastSubHistorySum.shCSmTotal = @class._ViewceMastSubHistorySum.shCSmTotal;
                            _ViewceMastSubHistorySum.shCMcMat = @class._ViewceMastSubHistorySum.shCMcMat;
                            _ViewceMastSubHistorySum.shCMcCofficient = @class._ViewceMastSubHistorySum.shCMcCofficient;
                            _ViewceMastSubHistorySum.shCMcWorkingTime = @class._ViewceMastSubHistorySum.shCMcWorkingTime;
                            _ViewceMastSubHistorySum.shCMcTotal = @class._ViewceMastSubHistorySum.shCMcTotal;
                            _ViewceMastSubHistorySum.shStatus = @class._ViewceMastSubHistorySum.shStatus;
                            _MK._ViewceMastSubHistorySum.Update(_ViewceMastSubHistorySum);

                            //shIssueBy
                        }
                    }
                    _MK.SaveChanges();
                    //save ceMastSubDetailHistorySum
                    var itemSubDetail = _MK._ViewceMastSubDetailHistorySum.Where(p => p.sdDocNo == @class._ViewceMastSubHistorySum.shDocNo).ToList();
                    _MK._ViewceMastSubDetailHistorySum.RemoveRange(itemSubDetail);
                    _MK.SaveChanges();
                    for (int i = 0; i < @class._ListViewceMastSubDetailHistorySum.Count(); i++)
                    {
                        var _ViewceMastSubDetailHistorySum = new ViewceMastSubDetailHistorySum()
                        {
                            sdDocNo = @class._ViewceMastSubHistorySum.shDocNo,
                            sdRunNo = i + 1,
                            sdLotNo = @class._ViewceMastSubHistorySum.shLotNo,
                            sdGroupName = @class._ListViewceMastSubDetailHistorySum[i].sdGroupName,
                            sdProcessName = @class._ListViewceMastSubDetailHistorySum[i].sdProcessName,
                            sdWK_Man = @class._ListViewceMastSubDetailHistorySum[i].sdWK_Man,
                            sdWK_Auto = @class._ListViewceMastSubDetailHistorySum[i].sdWK_Auto,
                            sdActive_WKMan = @class._ListViewceMastSubDetailHistorySum[i].sdActive_WKMan,
                            sdActive_WKAuto = @class._ListViewceMastSubDetailHistorySum[i].sdActive_WKAuto,
                            sdKIJWT_Man = @class._ListViewceMastSubDetailHistorySum[i].sdKIJWT_Man,
                            sdKJWT_Auto = @class._ListViewceMastSubDetailHistorySum[i].sdKJWT_Auto,
                            sdWT_Man = @class._ListViewceMastSubDetailHistorySum[i].sdWT_Man,
                            sdWT_Auto = @class._ListViewceMastSubDetailHistorySum[i].sdWT_Auto,
                        };
                        _MK._ViewceMastSubDetailHistorySum.AddAsync(_ViewceMastSubDetailHistorySum);
                    }
                    _MK.SaveChanges();

                    dbContextTransaction.Commit();

                    return Json(new { c1 = config, c2 = msg });
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    config = "E";
                    msg = "Something is wrong !!!!! : " + ex.Message;
                    return Json(new { c1 = config, c2 = msg });
                }
            }



        }

        public string[] RunDocNo(Class @class)
        {

            string v_msg = "";
            string v_rundoc = "";
            int i_rundoc = 0;

            string vIssue = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string vDocCode = "CE";
            string vDocSub = "H";
            string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
            string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);

            try
            {
                //check update revision or new
                if (@class._ViewceMastSubHistorySum.shDocNo != null && @class._ViewceMastSubHistorySum.shDocNo != "")
                {

                    v_msg = "Update";
                    v_rundoc = @class._ViewceMastSubHistorySum.shDocNo;
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
                        v_rundoc = "CE" + "-" + "H" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", i_rundoc + 1);
                    }
                    else
                    {
                        v_rundoc = "CE" + "-" + "H" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", 1);
                    }

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

        public IActionResult btnExportExcel(Class @class, string lotno)
        {
            string slipMat = lotno + "|" + DateTime.Now.ToString("yyyyMMdd:HHmmss");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export(" + slipMat + ").xlsx";
            try
            {

                ViewceMastSubMakerRequest _ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == lotno).FirstOrDefault();
                ViewceMastSubHistorySum _ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shLotNo == lotno).FirstOrDefault();



                string vlotNo = "";
                string vMoldName = "";

                string vLotNoMoldName = lotno;

                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();

                @class._ViewceMastSubHistorySum = new ViewceMastSubHistorySum(); //for report main
                @class._ListViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>(); //for report main
                List<ViewceMastSubDetailHistorySum> _listViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>();
                List<ViewceMastSubDetailHistorySum> _ViewceMastSubDetailHistorySum = new List<ViewceMastSubDetailHistorySum>(); //for report main

                @class._ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shLotNo == vLotNoMoldName).FirstOrDefault();
                _listViewceMastSubDetailHistorySum = _MK._ViewceMastSubDetailHistorySum.Where(x => x.sdLotNo == vLotNoMoldName).ToList();

                //@class._ViewceMastSubHistorySum = _MK._ViewceMastSubHistorySum.Where(x => x.shDocNo.Contains(vLotNoMoldName)).FirstOrDefault();
                //@class._ListViewceMastSubDetailHistorySum = _MK._ViewceMastSubDetailHistorySum.Where(x => x.sdLotNo.Contains(vLotNoMoldName)).OrderBy(x=>x.sdRunNo).ToList();


                if (vLotNoMoldName != null || vLotNoMoldName != "")
                {
                    vlotNo = vLotNoMoldName.Split("|")[0];
                    //vMoldName = vLotNoMoldName.Split("|")[1];
                    @class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == vlotNo).OrderByDescending(x => x.smDocumentNo).FirstOrDefault();


                }

                List<ViewceMastProcess> _ListceMastProcess = new List<ViewceMastProcess>();
                List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                List<ViewceCostPlanning> _ViewceCostPlanning = new List<ViewceCostPlanning>();
                List<VisaulViewceDetailSubMakerRequest> _ListVisaulViewceDetailSubMakerRequest = new List<VisaulViewceDetailSubMakerRequest>(); //sum all
                @class._ListGroupViewceDetailSubMakerRequest = new List<GroupViewceDetailSubMakerRequest>();

                string v_CostPlanningNo = _MK._ViewceMastCostModel.OrderByDescending(x => x.mcCostPlanningNo).Select(x => x.mcCostPlanningNo).First();
                var v_ListceCostPlanning = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == v_CostPlanningNo).ToList();
                _ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();
                @class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();


                //start detail all
                @class._ListceMastSubMakerRequest = new List<ViewceMastSubMakerRequest>();
                @class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == vlotNo && x.smReqStatus == true && x.smStep == 7).OrderByDescending(x => x.smDocumentNo).ToList();
                @class._ListceDetailSubMakerRequest = _MK._ViewceDetailSubMakerRequest.Where(d => @class._ListceMastSubMakerRequest.Select(m => m.smDocumentNo).Contains(d.dsDocumentNo)).ToList();


                var viewList = @class._ListceDetailSubMakerRequest; // สมมุติว่าเป็น List<ViewceDetailSubMakerRequest>

                @class._ListGroupedListceDetailSub = viewList
                    .GroupBy(x => x.dsDocumentNo) // Group ชั้นที่ 1
                    .Select(group1 => new GroupedListceDetailSub
                    {
                        glDocNo = group1.Key,
                        listGroupViewceDetailSubMakerRequest = group1
                            .GroupBy(x => x.dsGroupName) // Group ชั้นที่ 2
                            .Select(group2 => new GroupedListceDetailSubMakerRequest
                            {
                                glDocNo = group2.Key,
                                gllistDetail = group2.ToList() // ชั้นสุดท้าย
                            }).ToList()
                    }).ToList();

                //end detail all


                //start sum all
                var result = @class._ListceDetailSubMakerRequest.GroupBy(x => new { x.dsGroupName, x.dsProcessName })
                            .Select(g => new
                            {
                                GroupName = g.Key.dsGroupName,
                                ProcessName = g.Key.dsProcessName,
                                TotalWTMan = g.Sum(x => x.dsWT_Man),
                                TotalWTAuto = g.Sum(x => x.dsWT_Auto)
                            })
                            .ToList();
                //check 02/08/2025
                //group  NC(CA). NC(CO).
                double sumNC_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NC(CA)") || x.sdProcessName.Contains("NC(CO)"))).Sum(x => x.sdWK_Man)
                                  : 0;
                double sumNC_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NC(CA)") || x.sdProcessName.Contains("NC(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumNC_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("NC(CA)") || x.ProcessName.Trim().Contains("NC(CO)"))).Sum(x => x.TotalWTMan);
                double sumNC_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("NC(CA)") || x.ProcessName.Trim().Contains("NC(CO)"))).Sum(x => x.TotalWTAuto);
                double sumNC_KIJWT_Man = sumNC_WK_Man - sumNC_WT_Man;
                double sumNC_KIJWT_Auto = sumNC_WK_Auto - sumNC_WT_Auto;

                //group  NCG(CA). NCG(CO).
                double sumNCG_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                                 _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NCG(CA)") || x.sdProcessName.Contains("NCG(CO)"))).Sum(x => x.sdWK_Man)
                                 : 0;
                double sumNCG_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("NCG(CA)") || x.sdProcessName.Contains("NCG(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumNCG_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("NCG(CA)") || x.ProcessName.Trim().Contains("NCG(CO)"))).Sum(x => x.TotalWTMan);
                double sumNCG_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("NCG(CA)") || x.ProcessName.Trim().Contains("NCG(CO)"))).Sum(x => x.TotalWTAuto);
                double sumNCG_KIJWT_Man = sumNCG_WK_Man - sumNCG_WT_Man;
                double sumNCG_KIJWT_Auto = sumNCG_WK_Auto - sumNCG_WT_Auto;


                //group  EDM(CA). EDM(CO).
                double sumEDM_WK_Man = _listViewceMastSubDetailHistorySum != null ?
                              _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("EDM(CA)") || x.sdProcessName.Contains("EDM(CO)"))).Sum(x => x.sdWK_Man)
                              : 0;
                double sumEDM_WK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                  _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains("NC.") && (x.sdProcessName.Contains("EDM(CA)") || x.sdProcessName.Contains("EDM(CO)"))).Sum(x => x.sdWK_Auto)
                                  : 0;
                double sumEDM_WT_Man = result.Where(x => x.GroupName.Contains("NC.") && (
                                      x.ProcessName.Trim().Contains("EDM(CA)") || x.ProcessName.Trim().Contains("EDM(CO)"))).Sum(x => x.TotalWTMan);
                double sumEDM_WT_Auto = result.Where(x => x.GroupName.Contains("NC.") && (
                                     x.ProcessName.Trim().Contains("EDM(CA)") || x.ProcessName.Trim().Contains("EDM(CO)"))).Sum(x => x.TotalWTAuto);
                double sumEDM_KIJWT_Man = sumEDM_WK_Man - sumEDM_WT_Man;
                double sumEDM_KIJWT_Auto = sumEDM_WK_Auto - sumEDM_WT_Auto;

                for (int i = 0; i < v_ListceCostPlanning.Count(); i++)
                {
                    double vsdWK_Man = _listViewceMastSubDetailHistorySum != null ?
                                _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Man).FirstOrDefault()
                                : 0;
                    double vsdWK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                 _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Auto).FirstOrDefault()
                                 : 0;
                    double vsdWT_Man = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault();
                    double vsdWT_Auto = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault();

                    double sdWT_Man = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault();
                    double sdWT_Auto = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault();

                    bool sdActive_WKMan = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First();  //true,
                    bool sdActive_WKAuto = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First();



                    if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NC(CA)") || v_ListceCostPlanning[i].cpProcessName.Contains("NCG(CA)") || v_ListceCostPlanning[i].cpProcessName.Contains("EDM(CA)"))
                    {
                        vsdWK_Man = 0;
                        vsdWK_Auto = 0;
                        vsdWT_Man = 0;
                        vsdWT_Auto = 0;
                        sdWT_Man = 0;
                        sdWT_Auto = 0;
                        sdActive_WKMan = false;
                        sdActive_WKAuto = false;



                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NC(CO)"))
                    {
                        vsdWK_Man = sumNC_WK_Man;
                        vsdWK_Auto = sumNC_WK_Auto;
                        vsdWT_Man = sumNC_WT_Man;
                        vsdWT_Auto = sumNC_WT_Auto;
                        sdWT_Man = sumNC_WT_Man;
                        sdWT_Auto = sumNC_WT_Auto;
                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("NCG(CO)"))
                    {
                        vsdWK_Man = sumNCG_WK_Man;
                        vsdWK_Auto = sumNCG_WK_Auto;
                        vsdWT_Man = sumNCG_WT_Man;
                        vsdWT_Auto = sumNCG_WT_Auto;
                        sdWT_Man = sumNCG_WT_Man;
                        sdWT_Auto = sumNCG_WT_Auto;
                    }
                    else if (v_ListceCostPlanning[i].cpProcessName.Trim().Contains("EDM(CO)"))
                    {
                        vsdWK_Man = sumEDM_WK_Man;
                        vsdWK_Auto = sumEDM_WK_Auto;
                        vsdWT_Man = sumEDM_WT_Man;
                        vsdWT_Auto = sumEDM_WT_Auto;
                        sdWT_Man = sumEDM_WT_Man;
                        sdWT_Auto = sumEDM_WT_Auto;
                    }






                    @class._ListViewceMastSubDetailHistorySum.Add(new ViewceMastSubDetailHistorySum
                    {

                        sdDocNo = @class._ViewceMastSubHistorySum is null ? "" : @class._ViewceMastSubHistorySum.shDocNo,
                        sdRunNo = i + 1,
                        sdLotNo = @class._ViewceMastSubMakerRequest.smLotNo,
                        sdGroupName = v_ListceCostPlanning[i].cpGroupName.Trim(),
                        sdProcessName = v_ListceCostPlanning[i].cpProcessName,



                        sdWK_Man = vsdWK_Man,
                        sdWK_Auto = vsdWK_Auto,
                        //sdWK_Man = _listViewceMastSubDetailHistorySum != null ?
                        //   _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Man).FirstOrDefault()
                        //   : 0,
                        //sdWK_Auto = _listViewceMastSubDetailHistorySum != null ?
                        //           _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Auto).FirstOrDefault()
                        //           : 0,
                        sdActive_WKMan = sdActive_WKMan, // _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First(),  //true,
                        sdActive_WKAuto = sdActive_WKAuto,//_ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First(),

                        sdKIJWT_Man = vsdWK_Man - vsdWT_Man,
                        sdKJWT_Auto = vsdWK_Auto - vsdWT_Auto,
                        sdWT_Man = sdWT_Man,//result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault(),
                        sdWT_Auto = sdWT_Auto,//result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault(),




                    });

                }

                //_ListGroupDetailSubMakerRequestHissum
                @class._ListGroupDetailSubMakerRequestHissum = @class._ListViewceMastSubDetailHistorySum.GroupBy(p => p.sdGroupName).Select(g => new GroupDetailSubMakerRequestHissum
                {
                    GroupName = g.Key.Trim(),
                    DetailSubMakerRequest = g.ToList()
                }
             ).ToList();
                // end sum all

                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells.Style.Font.Size = 30;
                    worksheet.Cells["C2"].Value = "SUB MAKER REQUEST SHEET DETAIL.";
                    worksheet.Cells["C2"].Style.Font.Size = 50;
                    worksheet.Cells["C2"].Style.Font.Bold = true;
                    worksheet.Cells["C3"].Value = "COST PLANNING DIVISION.";
                    worksheet.Cells["C3"].Style.Font.Size = 42;
                    worksheet.Cells["C3"].Style.Font.Bold = true;

                    //detail  header
                    //row 1
                    worksheet.Cells["C5"].Value = "Customer Name :";
                    worksheet.Cells["C5"].Style.Font.Size = 30;
                    worksheet.Cells["C5"].Style.Font.Bold = true;
                    worksheet.Cells["D5"].Value = _ViewceMastSubMakerRequest.smCustomerName;
                    worksheet.Cells["D5"].Style.Font.Size = 30;

                    worksheet.Cells["E5"].Value = "Mold No. & Name :";
                    worksheet.Cells["E5"].Style.Font.Size = 30;
                    worksheet.Cells["E5"].Style.Font.Bold = true;
                    worksheet.Cells["F5"].Value = _ViewceMastSubMakerRequest.smMoldName;
                    worksheet.Cells["F5"].Style.Font.Size = 30;

                    worksheet.Cells["G5"].Value = "Model Name :";
                    worksheet.Cells["G5"].Style.Font.Size = 30;
                    worksheet.Cells["G5"].Style.Font.Bold = true;
                    worksheet.Cells["H5"].Value = _ViewceMastSubMakerRequest.smMoldName;
                    worksheet.Cells["H5"].Style.Font.Size = 30;


                    //row 2
                    worksheet.Cells["C6"].Value = "Function :";
                    worksheet.Cells["C6"].Style.Font.Size = 30;
                    worksheet.Cells["C6"].Style.Font.Bold = true;
                    worksheet.Cells["D6"].Value = _ViewceMastSubMakerRequest.smFunction;
                    worksheet.Cells["D6"].Style.Font.Size = 30;

                    worksheet.Cells["E6"].Value = "Lot No :";
                    worksheet.Cells["E6"].Style.Font.Size = 30;
                    worksheet.Cells["E6"].Style.Font.Bold = true;
                    worksheet.Cells["F6"].Value = _ViewceMastSubMakerRequest.smLotNo;
                    worksheet.Cells["F6"].Style.Font.Size = 30;

                    worksheet.Cells["G6"].Value = "Development Stage :";
                    worksheet.Cells["G6"].Style.Font.Size = 30;
                    worksheet.Cells["G6"].Style.Font.Bold = true;
                    worksheet.Cells["H6"].Value = _ViewceMastSubMakerRequest.smDevelopmentStage;
                    worksheet.Cells["H6"].Style.Font.Size = 30;

                    //row 3
                    worksheet.Cells["C7"].Value = "Revision :";
                    worksheet.Cells["C7"].Style.Font.Size = 30;
                    worksheet.Cells["C7"].Style.Font.Bold = true;
                    worksheet.Cells["D7"].Value = _ViewceMastSubMakerRequest.smRevision;
                    worksheet.Cells["D7"].Style.Font.Size = 30;

                    worksheet.Cells["E7"].Value = "Cavity No. :";
                    worksheet.Cells["E7"].Style.Font.Size = 30;
                    worksheet.Cells["E7"].Style.Font.Bold = true;
                    worksheet.Cells["F7"].Value = _ViewceMastSubMakerRequest.smCavityNo;
                    worksheet.Cells["F7"].Style.Font.Size = 30;

                    worksheet.Cells["G7"].Value = "Date Issue :";
                    worksheet.Cells["G7"].Style.Font.Size = 30;
                    worksheet.Cells["G7"].Style.Font.Bold = true;
                    worksheet.Cells["H7"].Value = _ViewceMastSubHistorySum.shIssueBy;
                    worksheet.Cells["H7"].Style.Font.Size = 30;





                    //detail
                    worksheet.Cells["C10:G10"].Merge = true;
                    worksheet.Cells["C10"].Value = _ViewceMastSubHistorySum.shLotNo;
                    worksheet.Cells["C10"].Style.Font.Size = 36;
                    worksheet.Cells["C10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells["H10:I10"].Merge = true;
                    worksheet.Cells["H10"].Value = "SUB ALL";
                    worksheet.Cells["H10"].Style.Font.Size = 36;
                    worksheet.Cells["H10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    /////
                    worksheet.Cells["C11:C12"].Merge = true;
                    worksheet.Cells["C11"].Value = "DETAIL.";
                    worksheet.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["D11:E11"].Merge = true;
                    worksheet.Cells["D11"].Value = "WORKING TIME";
                    worksheet.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells["F11:G11"].Merge = true;
                    worksheet.Cells["F11"].Value = "KIJUN - SUB";
                    worksheet.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells["H11:I11"].Merge = true;
                    worksheet.Cells["H11"].Value = "WT (Hr).";
                    worksheet.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Cells["D12"].Value = "MAN";
                    worksheet.Cells["E12"].Value = "(TOTAL)";
                    worksheet.Cells["F12"].Value = "MAN";
                    worksheet.Cells["G12"].Value = "(TOTAL)";
                    worksheet.Cells["H12"].Value = "MAN";
                    worksheet.Cells["I12"].Value = "(TOTAL)";
                    worksheet.Cells["D12:I12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //ตีเส้น
                    worksheet.Cells["C10:G10"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells["H10:I10"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells["C11:C12"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells["D11:E11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells["F11:G11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells["H11:I11"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    var unmergedRange = worksheet.Cells["D12:I12"];
                    foreach (var cell in unmergedRange)
                    {
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    worksheet.Cells["C10:I12"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells["C10:I12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["C10:I12"].Style.Font.Bold = true;

                    int D_row = 13;
                    int D_col = 3;
                    double sumPProcess = 0;
                    double sumPWTProcess = 0;
                    double sumPKJProcess = 0;

                    double sumPKJMan = 0;
                    double sumPKJAuto = 0;

                    foreach (var row in @class._ListGroupDetailSubMakerRequestHissum)
                    {
                        double VsumsdWK_Man = 0;
                        double VsdWK_Auto = 0;
                        double VsdKIJWT_Man = 0;
                        double VsdKJWT_Auto = 0;
                        double VsdWT_Man = 0;
                        double VsdWT_Auto = 0;

                        for (int i = 0; i < row.DetailSubMakerRequest.Count(); i++)
                        {
                            worksheet.Cells[D_row + i, D_col].Value = row.DetailSubMakerRequest[i].sdProcessName.ToString();

                            worksheet.Cells[D_row + i, D_col + 1].Value = row.DetailSubMakerRequest[i].sdWK_Man.ToString();
                            worksheet.Cells[D_row + i, D_col + 2].Value = row.DetailSubMakerRequest[i].sdWK_Auto.ToString();

                            worksheet.Cells[D_row + i, D_col + 3].Value = row.DetailSubMakerRequest[i].sdKIJWT_Man.ToString();
                            worksheet.Cells[D_row + i, D_col + 4].Value = row.DetailSubMakerRequest[i].sdKJWT_Auto.ToString();

                            if (row.DetailSubMakerRequest[i].sdKIJWT_Man < 0) //color red
                            {
                                SetNegativeCellStyle(worksheet.Cells[D_row + i, D_col + 3]);
                            }
                            if (row.DetailSubMakerRequest[i].sdKJWT_Auto < 0)
                            {
                                SetNegativeCellStyle(worksheet.Cells[D_row + i, D_col + 4]);
                            }



                            worksheet.Cells[D_row + i, D_col + 5].Value = row.DetailSubMakerRequest[i].sdWT_Man.ToString();
                            worksheet.Cells[D_row + i, D_col + 6].Value = row.DetailSubMakerRequest[i].sdWT_Auto.ToString();


                            VsumsdWK_Man += row.DetailSubMakerRequest[i].sdWK_Man;
                            VsdWK_Auto += row.DetailSubMakerRequest[i].sdWK_Auto;
                            VsdKIJWT_Man += row.DetailSubMakerRequest[i].sdKIJWT_Man;
                            VsdKJWT_Auto += row.DetailSubMakerRequest[i].sdKJWT_Auto;
                            VsdWT_Man += row.DetailSubMakerRequest[i].sdWT_Man;
                            VsdWT_Auto += row.DetailSubMakerRequest[i].sdWT_Auto;






                        }
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col].Value = "TOTAL " + row.GroupName.ToString();
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col].Style.Font.Bold = true;

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 1].Value = VsumsdWK_Man.ToString("N0");
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 2].Value = VsdWK_Auto.ToString("N0");
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 3].Value = VsdKIJWT_Man.ToString("N0");
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 4].Value = VsdKJWT_Auto.ToString("N0");
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 5].Value = VsdWT_Man.ToString("N0");
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 6].Value = VsdWT_Auto.ToString("N0");

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 2].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 3].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 4].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 5].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 6].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                        worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 6].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)


                        //color red 
                        if (VsumsdWK_Man < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 1]); }
                        if (VsdWK_Auto < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 2]); }
                        if (VsdKIJWT_Man < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 3]); }
                        if (VsdKJWT_Auto < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 4]); }
                        if (VsdWT_Man < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 5]); }
                        if (VsdWT_Auto < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + row.DetailSubMakerRequest.Count(), D_col + 6]); }




                        sumPKJMan += VsdKIJWT_Man;
                        sumPKJAuto += VsdKJWT_Auto;

                        //sum process
                        string vGroupName = row.GroupName.Trim();
                        if (vGroupName.ToLower().Contains("nc"))
                        {
                            sumPProcess += VsdWK_Auto;
                            sumPWTProcess += VsdWT_Auto;
                            sumPKJProcess += VsdKJWT_Auto;
                        }
                        else
                        {
                            sumPProcess += VsumsdWK_Man;
                            sumPWTProcess += VsdWT_Man;
                            sumPKJProcess += VsdKIJWT_Man;
                        }




                        // ตีกรอบทั้งหมดตั้งแต่บรรทัดเริ่มต้นของกลุ่มจนถึง TOTAL
                        int startRow1 = D_row;
                        int endRow1 = D_row + row.DetailSubMakerRequest.Count(); // รวมแถว TOTAL ด้วย
                        int startCol1 = D_col;
                        int endCol1 = D_col + 6;

                        for (int r = startRow1; r <= endRow1; r++)
                        {
                            for (int c = startCol1; c <= endCol1; c++)
                            {
                                var cell = worksheet.Cells[r, c];
                                cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }


                        D_row = D_row + row.DetailSubMakerRequest.Count() + 1;



                    }
                    //Totoal Process
                    worksheet.Cells[D_row, D_col].Value = "TOTAL PROCESS ";
                    worksheet.Cells[D_row, D_col + 1].Value = sumPProcess.ToString("N0");  //PROCESS
                    //worksheet.Cells[D_row, D_col + 3].Value = sumPKJMan.ToString(); //KJ Man
                    worksheet.Cells[D_row, D_col + 3].Value = sumPKJProcess.ToString("N0"); //PKJ Process
                    //worksheet.Cells[D_row, D_col + 4].Value = sumPKJAuto.ToString(); //KJ Auto
                    worksheet.Cells[D_row, D_col + 5].Value = sumPWTProcess.ToString("N0"); //WT Process

                    if (sumPProcess < 0) { SetNegativeCellStyle(worksheet.Cells[D_row, D_col + 1]); }
                    if (sumPKJProcess < 0) { SetNegativeCellStyle(worksheet.Cells[D_row, D_col + 3]); }
                    if (sumPWTProcess < 0) { SetNegativeCellStyle(worksheet.Cells[D_row, D_col + 5]); }



                    worksheet.Cells[D_row + 1, D_col + 4].Value = "COST SUB MAKER";
                    worksheet.Cells[D_row + 1, D_col + 5, D_row + 1, D_col + 6].Merge = true;
                    worksheet.Cells[D_row + 1, D_col + 5].Value = _ViewceMastSubHistorySum.shCSmMat.ToString("N0");
                    if (_ViewceMastSubHistorySum.shCSmMat < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 1, D_col + 5]); }
                    worksheet.Cells[D_row + 1, D_col + 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[D_row + 1, D_col + 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // ตีเส้นรอบพื้นที่ทั้งหมด
                    var borderRange = worksheet.Cells[D_row, D_col, D_row + 1, D_col + 6];
                    borderRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    borderRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    borderRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    borderRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;




                    worksheet.Cells[D_row + 3, 3, D_row + 4, 4].Merge = true;
                    worksheet.Cells[D_row + 3, 3].Value = "REMARK";
                    worksheet.Cells[D_row + 3, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[D_row + 3, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Cells[D_row + 3, 5, D_row + 4, 5].Merge = true;
                    worksheet.Cells[D_row + 3, 5].Value = "KIJUN COST";
                    worksheet.Cells[D_row + 3, 5].Style.Font.Size = 28;
                    worksheet.Cells[D_row + 3, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[D_row + 3, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[D_row + 3, 6, D_row + 4, 6].Merge = true;
                    worksheet.Cells[D_row + 3, 6].Value = "SUB MAKER COST";
                    worksheet.Cells[D_row + 3, 6].Style.Font.Size = 28;
                    worksheet.Cells[D_row + 3, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[D_row + 3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[D_row + 3, 7, D_row + 4, 7].Merge = true;
                    worksheet.Cells[D_row + 3, 7].Value = "MOLD CONTROL";
                    worksheet.Cells[D_row + 3, 7].Style.Font.Size = 28;
                    worksheet.Cells[D_row + 3, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[D_row + 3, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[D_row + 5, 3, D_row + 5, 4].Merge = true; // C15 ถึง D15
                    worksheet.Cells[D_row + 5, 3].Value = "COST MATERIAL";
                    worksheet.Cells[D_row + 5, 5].Value = _ViewceMastSubHistorySum.shCKjMat.ToString("N0");
                    worksheet.Cells[D_row + 5, 6].Value = _ViewceMastSubHistorySum.shCSmMat.ToString("N0");
                    worksheet.Cells[D_row + 5, 7].Value = _ViewceMastSubHistorySum.shCMcMat.ToString("N0");

                    if (_ViewceMastSubHistorySum.shCKjMat < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 5, 5]); }
                    if (_ViewceMastSubHistorySum.shCSmMat < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 5, 6]); }
                    if (_ViewceMastSubHistorySum.shCMcMat < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 5, 7]); }



                    worksheet.Cells[D_row + 6, 3, D_row + 6, 4].Merge = true; // C15 ถึง D15
                    worksheet.Cells[D_row + 6, 3].Value = "COST COFFICIENT";
                    worksheet.Cells[D_row + 6, 5].Value = _ViewceMastSubHistorySum.shCKjCofficient.ToString("N0");
                    worksheet.Cells[D_row + 6, 6].Value = "";
                    worksheet.Cells[D_row + 6, 7].Value = _ViewceMastSubHistorySum.shCMcCofficient.ToString("N0");

                    if (_ViewceMastSubHistorySum.shCKjCofficient < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 6, 5]); }
                    if (_ViewceMastSubHistorySum.shCMcCofficient < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 6, 6]); }


                    worksheet.Cells[D_row + 7, 3, D_row + 7, 4].Merge = true; // C15 ถึง D15
                    worksheet.Cells[D_row + 7, 3].Value = "COST WORKING TIME";
                    worksheet.Cells[D_row + 7, 5].Value = _ViewceMastSubHistorySum.shCKjWorkingTime.ToString("N0");
                    worksheet.Cells[D_row + 7, 6].Value = _ViewceMastSubHistorySum.shCSmWorkingTime.ToString("N0");
                    worksheet.Cells[D_row + 7, 7].Value = _ViewceMastSubHistorySum.shCMcWorkingTime.ToString("N0");

                    if (_ViewceMastSubHistorySum.shCKjWorkingTime < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 7, 5]); }
                    if (_ViewceMastSubHistorySum.shCSmWorkingTime < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 7, 6]); }
                    if (_ViewceMastSubHistorySum.shCMcWorkingTime < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 7, 7]); }




                    worksheet.Cells[D_row + 8, 3, D_row + 8, 4].Merge = true; // C15 ถึง D15
                    worksheet.Cells[D_row + 8, 3].Value = "TOTAL COST";
                    worksheet.Cells[D_row + 8, 5].Value = _ViewceMastSubHistorySum.shCKjTotal.ToString("N0");
                    worksheet.Cells[D_row + 8, 6].Value = "";
                    worksheet.Cells[D_row + 8, 7].Value = _ViewceMastSubHistorySum.shCMcTotal.ToString("N0");

                    if (_ViewceMastSubHistorySum.shCKjTotal < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 8, 5]); }
                    if (_ViewceMastSubHistorySum.shCMcTotal < 0) { SetNegativeCellStyle(worksheet.Cells[D_row + 8, 7]); }



                    var tableRange = worksheet.Cells[D_row + 3, 3, D_row + 8, 7];
                    tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;



                    // Auto fit column width
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    for (int col = 3; col <= 9; col++)
                    {
                        //if (col == 3)
                        //{
                        //    worksheet.Column(col).Width = 45;
                        //}
                        //else
                        //{
                        //    worksheet.Column(col).Width = 30;
                        //}

                        worksheet.Column(col).Width = 48;
                    }



                    //sheet 2
                    var worksheet2 = package.Workbook.Worksheets.Add("Sheet2");
                    worksheet2.Cells.Style.Font.Size = 30;
                    worksheet2.Cells["C2"].Value = "SUB MAKER REQUEST SHEET DETAIL.";
                    worksheet2.Cells["C2"].Style.Font.Size = 50;
                    worksheet2.Cells["C2"].Style.Font.Bold = true;
                    worksheet2.Cells["C3"].Value = "COST PLANNING DIVISION.";
                    worksheet2.Cells["C3"].Style.Font.Size = 42;
                    worksheet2.Cells["C3"].Style.Font.Bold = true;

                    //detail  header
                    //row 1
                    worksheet2.Cells["C5"].Value = "Customer Name :";
                    worksheet2.Cells["C5"].Style.Font.Size = 30;
                    worksheet2.Cells["C5"].Style.Font.Bold = true;
                    worksheet2.Cells["E5"].Value = _ViewceMastSubMakerRequest.smCustomerName;
                    worksheet2.Cells["E5"].Style.Font.Size = 30;

                    worksheet2.Cells["G5"].Value = "Mold No. & Name :";
                    worksheet2.Cells["G5"].Style.Font.Size = 30;
                    worksheet2.Cells["G5"].Style.Font.Bold = true;
                    worksheet2.Cells["I5"].Value = _ViewceMastSubMakerRequest.smMoldName;
                    worksheet2.Cells["I5"].Style.Font.Size = 30;

                    worksheet2.Cells["K5"].Value = "Model Name :";
                    worksheet2.Cells["K5"].Style.Font.Size = 30;
                    worksheet2.Cells["K5"].Style.Font.Bold = true;
                    worksheet2.Cells["M5"].Value = _ViewceMastSubMakerRequest.smMoldName;
                    worksheet2.Cells["M5"].Style.Font.Size = 30;


                    //row 2
                    worksheet2.Cells["C6"].Value = "Function :";
                    worksheet2.Cells["C6"].Style.Font.Size = 30;
                    worksheet2.Cells["C6"].Style.Font.Bold = true;
                    worksheet2.Cells["E6"].Value = _ViewceMastSubMakerRequest.smFunction;
                    worksheet2.Cells["E6"].Style.Font.Size = 30;

                    worksheet2.Cells["G6"].Value = "Lot No :";
                    worksheet2.Cells["G6"].Style.Font.Size = 30;
                    worksheet2.Cells["G6"].Style.Font.Bold = true;
                    worksheet2.Cells["I6"].Value = _ViewceMastSubMakerRequest.smLotNo;
                    worksheet2.Cells["I6"].Style.Font.Size = 30;

                    worksheet2.Cells["K6"].Value = "Development Stage :";
                    worksheet2.Cells["K6"].Style.Font.Size = 30;
                    worksheet2.Cells["K6"].Style.Font.Bold = true;
                    worksheet2.Cells["M6"].Value = _ViewceMastSubMakerRequest.smDevelopmentStage;
                    worksheet2.Cells["M6"].Style.Font.Size = 30;

                    //row 3
                    worksheet2.Cells["C7"].Value = "Revision :";
                    worksheet2.Cells["C7"].Style.Font.Size = 30;
                    worksheet2.Cells["C7"].Style.Font.Bold = true;
                    worksheet2.Cells["E7"].Value = _ViewceMastSubMakerRequest.smRevision;
                    worksheet2.Cells["E7"].Style.Font.Size = 30;

                    worksheet2.Cells["G7"].Value = "Cavity No. :";
                    worksheet2.Cells["G7"].Style.Font.Size = 30;
                    worksheet2.Cells["G7"].Style.Font.Bold = true;
                    worksheet2.Cells["I7"].Value = _ViewceMastSubMakerRequest.smCavityNo;
                    worksheet2.Cells["I7"].Style.Font.Size = 30;

                    worksheet2.Cells["K7"].Value = "Date Issue :";
                    worksheet2.Cells["K7"].Style.Font.Size = 30;
                    worksheet2.Cells["K7"].Style.Font.Bold = true;
                    worksheet2.Cells["M7"].Value = _ViewceMastSubHistorySum.shIssueBy;
                    worksheet2.Cells["M7"].Style.Font.Size = 30;





                    int W2_row = 11;
                    int W2_col = 3; // doc no
                    int W2_colD = 3; // doc no

                    int W2_rowW = 13; //row detail
                    int W2_colW = 3; // col  detail

                    int W2_rowWSum = 13; //row detail


                    foreach (var row in @class._ListGroupedListceDetailSub)
                    {
                        // Merge 2 คอลัมน์ในแนวนอน

                        worksheet2.Cells[W2_row, W2_col, W2_row, W2_col + 1].Merge = true;

                        // ใส่ค่า
                        worksheet2.Cells[W2_row, W2_col].Value = row.glDocNo;
                        worksheet2.Cells[W2_row + 1, W2_colD].Value = "(MAN)";
                        worksheet2.Cells[W2_row + 1, W2_colD + 1].Value = "(TOTAL)";

                        worksheet2.Cells[W2_row, W2_col].Style.Font.Size = 28;
                        worksheet2.Cells[W2_row + 1, W2_col].Style.Font.Size = 28;
                        worksheet2.Cells[W2_row + 1, W2_col + 1].Style.Font.Size = 28;


                        SetAlignCenter(worksheet2.Cells[W2_row, W2_col]);
                        SetAlignCenter(worksheet2.Cells[W2_row + 1, W2_col]);
                        SetAlignCenter(worksheet2.Cells[W2_row + 1, W2_col + 1]);


                        SetBorder(worksheet2.Cells[W2_row, W2_col, W2_row, W2_col + 1]);
                        SetBorder(worksheet2.Cells[W2_row + 1, W2_colD]);
                        SetBorder(worksheet2.Cells[W2_row + 1, W2_col + 1]);

                        double sumPWTCostSUB = 0;
                        double sumPWTProcessD = 0;
                        double sumPWTManNc = 0;
                        double sumPWTMan = 0;
                        double sumPWTAuto = 0;


                        foreach (var subGroup in row.listGroupViewceDetailSubMakerRequest)
                        {
                            double sumWTMan = 0;
                            double sumWTAuto = 0;


                            foreach (var item in subGroup.gllistDetail)
                            {
                                double vWtMan = item.dsWT_Man;
                                double vWtAuto = item.dsWT_Auto;

                                worksheet2.Cells[W2_rowW, W2_colD].Value = vWtMan.ToString("N0");
                                worksheet2.Cells[W2_rowW, W2_colD + 1].Value = vWtAuto.ToString("N0");


                                SetAlignCenter(worksheet2.Cells[W2_rowW, W2_colD]);
                                SetAlignCenter(worksheet2.Cells[W2_rowW, W2_colD + 1]);
                                SetBorder(worksheet2.Cells[W2_rowW, W2_colD]);
                                SetBorder(worksheet2.Cells[W2_rowW, W2_colD + 1]);


                                sumWTMan += vWtMan;
                                sumWTAuto += vWtAuto;
                                string vGroupName = @item.dsGroupName;
                                if (vGroupName.ToLower().Contains("nc"))
                                {
                                    sumPWTProcessD += vWtAuto;
                                    sumPWTManNc += vWtMan;
                                }
                                else
                                {
                                    sumPWTProcessD += vWtMan;
                                }

                                W2_rowW += 1;
                                W2_rowWSum += 1;
                            }
                            worksheet2.Cells[W2_rowW, W2_colD].Value = sumWTMan.ToString("N0");
                            worksheet2.Cells[W2_rowW, W2_colD + 1].Value = sumWTAuto.ToString("N0");

                            worksheet2.Cells[W2_rowW, W2_colD].Style.Font.Bold = true;
                            worksheet2.Cells[W2_rowW, W2_colD + 1].Style.Font.Bold = true;

                            //worksheet2.Cells[W2_rowW, W2_colD].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(200, 200, 200)); //LightGray
                            //worksheet2.Cells[W2_rowW, W2_colD + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(200, 200, 200)); //LightGray

                            worksheet2.Cells[W2_rowW, W2_colD].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                            worksheet2.Cells[W2_rowW, W2_colD].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)

                            worksheet2.Cells[W2_rowW, W2_colD + 1].Style.Fill.PatternType = ExcelFillStyle.Solid; // ต้องมี!
                            worksheet2.Cells[W2_rowW, W2_colD + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // หรือ Color.FromArgb(128, 128, 128)



                            SetAlignCenter(worksheet2.Cells[W2_rowW, W2_colD]);
                            SetBorder(worksheet2.Cells[W2_rowW, W2_colD]);
                            SetAlignCenter(worksheet2.Cells[W2_rowW, W2_colD + 1]);
                            SetBorder(worksheet2.Cells[W2_rowW, W2_colD + 1]);


                            W2_rowW += 1;
                            W2_rowWSum += 1;

                            //worksheet2.Cells[W2_rowW, W2_colW].Value = "TOTAL ";
                        }
                        sumPWTCostSUB = (sumPWTProcessD - sumPWTManNc) * 1000;
                        worksheet2.Cells[W2_rowWSum, W2_colD].Value = sumPWTProcessD.ToString("N0");
                        worksheet2.Cells[W2_rowWSum, W2_colD].Style.Font.Bold = true;


                        worksheet2.Cells[W2_rowWSum + 1, W2_colD].Value = "";
                        worksheet2.Cells[W2_rowWSum + 1, W2_colD + 1].Value = sumPWTCostSUB.ToString("N0");

                        SetAlignCenter(worksheet2.Cells[W2_rowWSum, W2_colD]);
                        SetBorder(worksheet2.Cells[W2_rowWSum, W2_colD]);

                        SetAlignCenter(worksheet2.Cells[W2_rowWSum, W2_colD + 1]);
                        SetBorder(worksheet2.Cells[W2_rowWSum, W2_colD + 1]);

                        SetAlignCenter(worksheet2.Cells[W2_rowWSum + 1, W2_colD]);
                        SetBorder(worksheet2.Cells[W2_rowWSum + 1, W2_colD]);

                        SetAlignCenter(worksheet2.Cells[W2_rowWSum + 1, W2_colD + 1]);
                        SetBorder(worksheet2.Cells[W2_rowWSum + 1, W2_colD + 1]);

                        W2_rowW = 13;
                        W2_rowWSum = 13;


                        W2_col += 2;
                        W2_colD += 2;
                    }


                    //sumPWTCostSUB = (sumPWTProcessD - sumPWTManNc) * 1000;
                    //sumPCostsubMaker = sumPCostsubMaker + sumPWTCostSUB;







                    // Auto fit column width
                    worksheet2.Cells[worksheet2.Dimension.Address].AutoFitColumns();
                    for (int col = 3; col <= W2_colD; col++)
                    {
                        worksheet2.Column(col).Width = 25;
                    }



                    // Export เป็น stream
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    // ส่งไฟล์ออก (ใช้ใน ASP.NET Core)
                    // string fileName = $"Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                var stream = new MemoryStream();
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }




        }
        void SetBorder(ExcelRange cell)
        {
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        void SetAlignCenter(ExcelRange cell)
        {
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
        public static void SetNegativeCellStyle(ExcelRange cell)
        {
            cell.Style.Font.Color.SetColor(Color.Red); // ตัวหนังสือสีแดง
            cell.Style.Font.Bold = true; // ตัวหนา

            //cell.Style.Fill.PatternType = ExcelFillStyle.Solid; // ให้มีพื้นหลัง

        }

    }
}