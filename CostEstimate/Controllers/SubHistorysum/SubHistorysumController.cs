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

namespace CostEstimate.Controllers.SubHistorysum
{
    public class SubHistorysumController : Controller
    {
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
                @class._ListceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == vlotNo && x.smReqStatus == true).OrderByDescending(x => x.smDocumentNo).ToList();
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

                    @class._ListViewceMastSubDetailHistorySum.Add(new ViewceMastSubDetailHistorySum
                    {

                        sdDocNo = @class._ViewceMastSubHistorySum is null ? "" : @class._ViewceMastSubHistorySum.shDocNo,
                        sdRunNo = i + 1,
                        sdLotNo = @class._ViewceMastSubMakerRequest.smLotNo,
                        sdGroupName = v_ListceCostPlanning[i].cpGroupName.Trim(),
                        sdProcessName = v_ListceCostPlanning[i].cpProcessName,
                        sdWK_Man = _listViewceMastSubDetailHistorySum != null ?
                                   _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Man).FirstOrDefault()
                                   : 0,
                        sdWK_Auto = _listViewceMastSubDetailHistorySum != null ?
                                   _listViewceMastSubDetailHistorySum.Where(x => x.sdGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.sdProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.sdWK_Auto).FirstOrDefault()
                                   : 0,
                        sdActive_WKMan = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First(),  //true,
                        sdActive_WKAuto = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First(),
                        sdKIJWT_Man = vsdWK_Man - vsdWT_Man,
                        sdKJWT_Auto = vsdWK_Auto - vsdWT_Auto,
                        sdWT_Man = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTMan).FirstOrDefault(),
                        sdWT_Auto = result.Where(x => x.GroupName == v_ListceCostPlanning[i].cpGroupName.Trim() && x.ProcessName == v_ListceCostPlanning[i].cpProcessName.Trim()).Select(x => x.TotalWTAuto).FirstOrDefault(),

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


    }
}