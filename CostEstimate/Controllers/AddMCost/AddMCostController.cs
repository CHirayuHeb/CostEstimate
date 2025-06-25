using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostEstimate.Models.DBConnect;
using Microsoft.AspNetCore.Mvc;

using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MOLD;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Authorization;
using CostEstimate.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;

namespace CostEstimate.Controllers.AddMCost
{
    public class AddMCostController : Controller
    {  //username emppic ftpdb
        private LAMP _LAMP;
        private HRMS _HRMS;
        private IT _IT;
        private MK _MK;
        private MOLD _MOLD;
        private CacheSettingController _Cache;
        private FunctionsController _callFunc;
        public string path = @"\\thsweb\\CostEstimate\\";
        public string PgName = "CostEstimate";

        public AddMCostController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        public IActionResult Index(Class @class)
        {
            @class._ListYeartHourChange = new List<YeartHourChange>();
            var _ListYeartHourChange1 = _MK._ViewceHourChangeCategory.Select(x => x.hcYear).Distinct().ToList();
            // @class._ListYeartHourChange = _MK._ViewceHourChangeCategory.GroupBy(p => new { p.hcYear, p.hcRev }).Select(x => x.hcYear, x.hcRev).Distinct().Select(y => new YeartHourChange { year = y }).ToList();

            @class._ListYeartHourChange = _MK._ViewceHourChangeCategory.GroupBy(p => new { p.hcYear, p.hcRev, p.hcIssue }).Select(g => new YeartHourChange
            {
                year = g.Key.hcYear,
                Rev = g.Key.hcRev,
                issueDate = g.Key.hcIssue,
            })
                                            .Distinct() // อันนี้อาจไม่จำเป็นหลัง GroupBy แล้ว
                                            .ToList();


            List<ViewceHourChangeCategory> _ListViewceHourChangeCategory = new List<ViewceHourChangeCategory>();



            // _ListViewceHourChangeCategory = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == "2025").ToList();
            @class._ListGroupViewceHourChangeCategory = new List<GroupViewceHourChangeCategory>();
            //@class._ListGroupViewceHourChangeCategory = _ListViewceHourChangeCategory.GroupBy(p => p.hcGroupSub).Select(g => new GroupViewceHourChangeCategory
            //{
            //    GroupName = g.Key.Trim(),
            //    ceHourChangeCategory = g.ToList()
            //}
            // ).ToList();
            return View(@class);
        }

        [HttpPost]
        public PartialViewResult SearchMastCostHourChange(string DocYear, string DocRev, string DocType)
        {

            Class @class = new Class();
            List<ViewceMastHourChage> _ListViewceMastHourChage = new List<ViewceMastHourChage>();
            List<ViewceHourChangeCategory> _ListViewceHourChangeCategory = new List<ViewceHourChangeCategory>();
            List<ViewceHourChangeEntry> _ListViewceHourChangeEntry = new List<ViewceHourChangeEntry>();




            try
            {
                DocYear = DocYear is null ? DateTime.Now.ToString("yyyy") : DocYear;
                string DocYear1 = DocYear.Substring(2, 2);
                string DocYear2 = (int.Parse(DocYear1) + 1).ToString();
                @class._ListGroupedData = new List<GroupedData>();
                @class._ListMlistMonth = FunListMonth(DocYear1, DocYear2).Select(m => new MlistMonth { Month = m }).ToList();
                @class._ListtypeDetail = FunListtype().Select(m => new typeDetail { type = m }).ToList(); ;

                @class.paramCostNo = DocYear;


                int vRev = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear).Select(x => x.hcRev).ToList().Count() == 0
                            ? 0 : _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear).Select(x => x.hcRev + 1).FirstOrDefault();


                //chek type
                if (DocType == "Add")
                {
                    //check old version
                    int vARev = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear).Select(x => x.hcRev).ToList().Count() == 0
                        ? 0 : _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear).OrderByDescending(x => x.hcRev).Select(x => x.hcRev).FirstOrDefault();
                    _ListViewceHourChangeCategory = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear && x.hcRev == vARev)
                                                                          .ToList() // แปลงก่อน เพื่อให้ใช้ LINQ to Objects ได้
                                                                          .OrderBy(x => x.hcYear)
                                                                          .ThenBy(x => x.hcRev)
                                                                          .ThenBy(x => x.hcGroupMain)
                                                                          .ThenBy(x => int.TryParse(x.hcGroupSub, out var n) ? n : int.MaxValue)
                                                                          .ThenBy(x => x.hcProcessName)
                                                                          .ToList();
                    //have ดึง version เก่ามา
                    if (_ListViewceHourChangeCategory.Count > 0)
                    {
                        //    _ListViewceHourChangeCategory.ForEach(
                        //        x => x.hcRev = vARev + 1

                        //    );
                        _ListViewceHourChangeCategory.ForEach(x =>
                        {
                            x.hcRev = vARev + 1;
                            x.hcIssue = "";
                        });

                        _ListViewceHourChangeEntry = _MK._ViewceHourChangeEntry.Where(x => x.heYear == DocYear && x.heRev == vARev).ToList();

                        @class._ListGroupViewceHourChangeCategory = new List<GroupViewceHourChangeCategory>();
                        @class._ListGroupViewceHourChangeCategory = _ListViewceHourChangeCategory.GroupBy(p => p.hcGroupMain).Select(g => new GroupViewceHourChangeCategory
                        {
                            GroupName = g.Key.Trim(),
                            ceHourChangeCategory = g.ToList()
                        }
                         ).ToList();
                        // รวม RESULTS + FORECAST เข้าด้วยกัน
                        var subGrouped = _ListViewceHourChangeEntry
                            .GroupBy(s => new
                            {
                                s.heGroupMain,
                                s.heProcessName,
                                s.heMonth,
                                s.heType
                                //HeType =s.heType == "RESULTS" || s.HeType == "FORECAST" ? "RESULTS/FORECAST" : s.HeType
                            })
                            .Select(g => new
                            {
                                g.Key.heGroupMain,
                                g.Key.heProcessName,
                                g.Key.heMonth,
                                HeType = g.Key.heType,
                                HeAmount = g.Sum(x => x.heAmount)
                            }).ToList();

                        @class._ListGroupedData = _ListViewceHourChangeEntry.GroupBy(e => new { e.heGroupMain, e.heProcessName, e.heType })
                                           .Select(g => new GroupedData
                                           {
                                               HcGroupMain = g.Key.heGroupMain,
                                               HeProcessName = g.Key.heProcessName,
                                               HeType = g.Key.heType,
                                               Months = g.Select(e => new MonthData
                                               {
                                                   HeMonth = e.heMonth,
                                                   HeAmount = e.heAmount
                                               }).ToList()
                                           }).ToList();

                        @class._ListGroupedResult = _ListViewceHourChangeCategory
                            .GroupBy(m => m.hcGroupMain)
                            .Select(g => new GroupedResult
                            {
                                HcGroupMain = g.Key,
                                Processes = g.Select(m => new ProcessResult
                                {
                                    HcGroupMain = m.hcGroupMain,
                                    HcProcessName = m.hcProcessName,
                                    //select heProcessName 
                                    SubItems = _ListViewceHourChangeEntry
                                        .Where(s => s.heProcessName == m.hcProcessName)
                                        .ToList()
                                }).ToList()
                            })
                            .ToList();

                    }
                    else//not ทำใหม่
                    {
                        _ListViewceMastHourChage = _MK._ViewceMastHourChage.ToList() // แปลงก่อน เพื่อให้ใช้ LINQ to Objects ได้
                                                                        .OrderBy(x => x.mhGroupMain)
                                                                        .ThenBy(x => int.TryParse(x.mhGroupSub, out var n) ? n : int.MaxValue)
                                                                        .ThenBy(x => x.mhProcessName)
                                                                        .ToList();



                        for (int i = 0; i < _ListViewceMastHourChage.Count(); i++)
                        {
                            _ListViewceHourChangeCategory.Add(new ViewceHourChangeCategory
                            {
                                hcId = i,
                                hcYear = DocYear,
                                hcRev = vRev,
                                hcGroupMain = _ListViewceMastHourChage[i].mhGroupMain,
                                hcGroupSub = _ListViewceMastHourChage[i].mhGroupSub,
                                hcProcessName = _ListViewceMastHourChage[i].mhProcessName,

                            });
                        }

                        @class._ListGroupViewceHourChangeCategory = new List<GroupViewceHourChangeCategory>();
                        @class._ListGroupViewceHourChangeCategory = _ListViewceHourChangeCategory.GroupBy(p => p.hcGroupMain).Select(g => new GroupViewceHourChangeCategory
                        {
                            GroupName = g.Key.Trim(),
                            ceHourChangeCategory = g.ToList()
                        }
                         ).ToList();



                        @class._ListGroupedData = _ListViewceHourChangeEntry.GroupBy(e => new { e.heGroupMain, e.heProcessName, e.heType })
                                                .Select(g => new GroupedData
                                                {
                                                    HcGroupMain = g.Key.heGroupMain,
                                                    HeProcessName = g.Key.heProcessName,
                                                    HeType = g.Key.heType,
                                                    Months = g.Select(e => new MonthData
                                                    {
                                                        HeMonth = e.heMonth,
                                                        HeAmount = e.heAmount
                                                    }).ToList()
                                                }).ToList();

                        @class._ListGroupedResult = _ListViewceHourChangeCategory
                       .GroupBy(m => m.hcGroupMain)
                       .Select(g => new GroupedResult
                       {
                           HcGroupMain = g.Key,
                           Processes = g.Select(m => new ProcessResult
                           {
                               HcGroupMain = m.hcGroupMain,
                               HcProcessName = m.hcProcessName,
                               //select heProcessName 
                               SubItems = _ListViewceHourChangeEntry
                                   .Where(s => s.heProcessName == m.hcProcessName)
                                   .ToList()
                           }).ToList()
                       })
                       .ToList();

                    }





                }
                else //Edit
                {
                    _ListViewceHourChangeCategory = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear && x.hcRev == int.Parse(DocRev))
                                                                             .ToList() // แปลงก่อน เพื่อให้ใช้ LINQ to Objects ได้
                                                                             .OrderBy(x => x.hcYear)
                                                                             .ThenBy(x => x.hcRev)
                                                                             .ThenBy(x => x.hcGroupMain)
                                                                             .ThenBy(x => int.TryParse(x.hcGroupSub, out var n) ? n : int.MaxValue)
                                                                             .ThenBy(x => x.hcProcessName)
                                                                             .ToList();

                    if (_ListViewceHourChangeCategory.Count > 0)
                    {
                        _ListViewceHourChangeCategory = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == DocYear && x.hcRev == int.Parse(DocRev))
                                                                                    .ToList() // แปลงก่อน เพื่อให้ใช้ LINQ to Objects ได้
                                                                                    .OrderBy(x => x.hcYear)
                                                                                    .ThenBy(x => x.hcRev)
                                                                                    .ThenBy(x => x.hcGroupMain)
                                                                                    .ThenBy(x => int.TryParse(x.hcGroupSub, out var n) ? n : int.MaxValue)
                                                                                    .ThenBy(x => x.hcProcessName)
                                                                                    .ToList();



                        _ListViewceHourChangeEntry = _MK._ViewceHourChangeEntry.Where(x => x.heYear == DocYear && x.heRev == int.Parse(DocRev)).ToList();

                        @class._ListGroupViewceHourChangeCategory = new List<GroupViewceHourChangeCategory>();
                        @class._ListGroupViewceHourChangeCategory = _ListViewceHourChangeCategory.GroupBy(p => p.hcGroupMain).Select(g => new GroupViewceHourChangeCategory
                        {
                            GroupName = g.Key.Trim(),
                            ceHourChangeCategory = g.ToList()
                        }
                         ).ToList();


                        // รวม RESULTS + FORECAST เข้าด้วยกัน
                        var subGrouped = _ListViewceHourChangeEntry
                            .GroupBy(s => new
                            {
                                s.heGroupMain,
                                s.heProcessName,
                                s.heMonth,
                                s.heType
                                //HeType =s.heType == "RESULTS" || s.HeType == "FORECAST" ? "RESULTS/FORECAST" : s.HeType
                            })
                            .Select(g => new
                            {
                                g.Key.heGroupMain,
                                g.Key.heProcessName,
                                g.Key.heMonth,
                                HeType = g.Key.heType,
                                HeAmount = g.Sum(x => x.heAmount)
                            }).ToList();



                        @class._ListGroupedData = _ListViewceHourChangeEntry.GroupBy(e => new { e.heGroupMain, e.heProcessName, e.heType })
                                                .Select(g => new GroupedData
                                                {
                                                    HcGroupMain = g.Key.heGroupMain,
                                                    HeProcessName = g.Key.heProcessName,
                                                    HeType = g.Key.heType,
                                                    Months = g.Select(e => new MonthData
                                                    {
                                                        HeMonth = e.heMonth,
                                                        HeAmount = e.heAmount
                                                    }).ToList()
                                                }).ToList();

                        @class._ListGroupedResult = _ListViewceHourChangeCategory
                            .GroupBy(m => m.hcGroupMain)
                            .Select(g => new GroupedResult
                            {
                                HcGroupMain = g.Key,
                                Processes = g.Select(m => new ProcessResult
                                {
                                    HcGroupMain = m.hcGroupMain,
                                    HcProcessName = m.hcProcessName,
                                    //select heProcessName 
                                    SubItems = _ListViewceHourChangeEntry
                                        .Where(s => s.heProcessName == m.hcProcessName)
                                        .ToList()
                                }).ToList()
                            })
                            .ToList();




                    }
                }


            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastChargeRate", @class);

        }

        public List<string> FunListMonth(string docYear1, string docYear2)
        {
            List<string> _listMonth = new List<string>
                                    {
                                        "04/" + docYear1,
                                        "05/" + docYear1,
                                        "06/" + docYear1,
                                        "07/" + docYear1,
                                        "08/" + docYear1,
                                        "09/" + docYear1,
                                        "10/" + docYear1,
                                        "11/" + docYear1,
                                        "12/" + docYear1,
                                        "01/" + docYear2,
                                        "02/" + docYear2,
                                        "03/" + docYear2
                                    };

            // set to ViewBag if needed
            //ViewBag.listofMonth = new SelectList(_listMonth);

            return _listMonth;
        }


        public List<string> FunListtype()
        {
            var customOrder = new List<string> { "PLAN", "RESULTS", "FORECAST" };
            var distinctTypes = _MK._ViewceHourChangeEntry.Select(x => x.heType).Distinct().OrderBy(x => customOrder.IndexOf(x)).ToList();

            // set to ViewBag if needed
            //ViewBag.listofMonth = new SelectList(_listMonth);

            return distinctTypes;
        }


        [HttpPost]
        public JsonResult SaveMasterCostChRate(Class @class, string _ceHourChangeEntry, string _ceHourChangeCategory)
        {
            string config = "S";
            string msg = "Your data has been successfully recorded.";
            string IssueBy = DateTime.Now.ToString("dd-MMM-yy", CultureInfo.InvariantCulture);

            string vDocyear = "";
            int vDocRev = 0;
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    @class._ListViewceHourChangeCategory = JsonConvert.DeserializeObject<List<ViewceHourChangeCategory>>(_ceHourChangeCategory);
                    if (@class._ListViewceHourChangeCategory != null)
                    {
                        vDocyear = @class._ListViewceHourChangeCategory[0].hcYear;
                        vDocRev = @class._ListViewceHourChangeCategory[0].hcRev;
                    }


                    var itemHourChange = _MK._ViewceHourChangeCategory.Where(p => p.hcYear == vDocyear && p.hcRev == vDocRev).ToList();
                    _MK._ViewceHourChangeCategory.RemoveRange(itemHourChange);
                    _MK.SaveChanges();

                    for (int i = 0; i < @class._ListViewceHourChangeCategory.Count(); i++)
                    {
                        var _ViewceHourChangeCategory = new ViewceHourChangeCategory()
                        {
                            //hcId = _runNo,
                            hcYear = @class._ListViewceHourChangeCategory[i].hcYear,
                            hcRev = @class._ListViewceHourChangeCategory[i].hcRev,
                            hcGroupMain = @class._ListViewceHourChangeCategory[i].hcGroupMain,
                            hcGroupSub = @class._ListViewceHourChangeCategory[i].hcGroupSub,
                            hcProcessName = @class._ListViewceHourChangeCategory[i].hcProcessName,
                            hcIssue = @class._ListViewceHourChangeCategory[i].hcIssue is null || @class._ListViewceHourChangeCategory[i].hcIssue == "" ? IssueBy : @class._ListViewceHourChangeCategory[i].hcIssue,
                        };
                        _MK._ViewceHourChangeCategory.AddAsync(_ViewceHourChangeCategory);
                    }

                    @class._ListViewceHourChangeEntry = JsonConvert.DeserializeObject<List<ViewceHourChangeEntry>>(_ceHourChangeEntry);
                    var itemHourChangeEntry = _MK._ViewceHourChangeEntry.Where(p => p.heYear == vDocyear && p.heRev == vDocRev).ToList();
                    _MK._ViewceHourChangeEntry.RemoveRange(itemHourChangeEntry);
                    _MK.SaveChanges();

                    for (int i = 0; i < @class._ListViewceHourChangeEntry.Count(); i++)
                    {
                        var _ViewceHourChangeEntry = new ViewceHourChangeEntry()
                        {
                            //hcId = _runNo,
                            heYear = @class._ListViewceHourChangeEntry[i].heYear,
                            heRev = @class._ListViewceHourChangeEntry[i].heRev,
                            heMonth = @class._ListViewceHourChangeEntry[i].heMonth,
                            heGroupMain = @class._ListViewceHourChangeEntry[i].heGroupMain,
                            heProcessName = @class._ListViewceHourChangeEntry[i].heProcessName,
                            heType = @class._ListViewceHourChangeEntry[i].heType,
                            heAmount = @class._ListViewceHourChangeEntry[i].heAmount,
                        };
                        _MK._ViewceHourChangeEntry.AddAsync(_ViewceHourChangeEntry);


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

        public ActionResult DeleteMasterCostChRate(string vYear, string vRev)
        {
            try
            {
                int iRev = vRev != null ? int.Parse(vRev) : 0;

                var findCategory = _MK._ViewceHourChangeCategory.Where(x => x.hcYear == vYear && x.hcRev == iRev).ToList();
                if (findCategory != null)
                {
                    _MK._ViewceHourChangeCategory.RemoveRange(findCategory);
                }

                var findEntry = _MK._ViewceHourChangeEntry.Where(x => x.heYear == vYear && x.heRev == iRev).ToList();
                if (findEntry != null)
                {
                    _MK._ViewceHourChangeEntry.RemoveRange(findEntry);
                }
                _MK.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { res = "error: " + ex.Message });

            }
            return Json(new { res = "success" });


            //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());

        }


    }
}