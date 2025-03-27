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

namespace CostEstimate.Controllers.AddCost
{
    public class AddCostController : Controller
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

        public AddCostController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListViewcceRunCostpalnning = new List<ViewcceRunCostpalnning>();
            List<ViewcceRunCostpalnning> _ViewcceRunCostpalnning = _MK._ViewcceRunCostpalnning.OrderBy(x => x.rcYear).ThenBy(x => x.rcRunNo).Distinct().ToList();
            @class._ListGroupViewceMastCostModel = new List<GroupViewceMastCostModel>();
            if (_ViewcceRunCostpalnning.Count() > 0)
            {
                for (int i = 0; i < _ViewcceRunCostpalnning.Count(); i++)
                {
                    string v_des = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.cpDescription).FirstOrDefault() is null ?
                                    "" :
                                   _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.cpDescription).FirstOrDefault();
                    var _GroupViewceMastCostModel = new GroupViewceMastCostModel()
                    {
                        //exp: CP-2025-01
                        CostPlanningNo = _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo),
                        //Description = _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.mcDescription).FirstOrDefault() is null ? "" : _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.mcDescription).FirstOrDefault(),
                        Description = v_des,
                        // Description =  _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)) is null ? "" : _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.mcDescription).First() ,
                    };
                    //@class._ListGroupViewceMastCostModel.Add(new GroupViewceMastCostModel { CostPlanningNo = _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo),
                    //                                                                        Description = _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == _ViewcceRunCostpalnning[i].rcDocCode + "-" + _ViewcceRunCostpalnning[i].rcYear + "-" + String.Format("{0:D2}", _ViewcceRunCostpalnning[i].rcRunNo)).Select(x => x.mcDescription).First()
                    //                      });
                    @class._ListGroupViewceMastCostModel.Add(_GroupViewceMastCostModel);
                }
            }



            @class._ViewceMastModel = new ViewceMastModel();
            List<ViewceMastModel> _ViewceMastModel = _MK._ViewceMastModel.OrderBy(x => x.mmModelName).Distinct().ToList();


            SelectList formMastModel = new SelectList(_ViewceMastModel.Select(s => s.mmModelName).Distinct());
            ViewBag.formMastModel = formMastModel;

            //@class._ListGroupViewceMastCostModel = new List<GroupViewceMastCostModel>();
            //@class._ListGroupViewceMastCostModel = _MK._ViewceMastCostModel.GroupBy(c => new
            //{
            //    c.mcCostPlanningNo,
            //    c.mcDescription
            //}).Select(gcs => new GroupViewceMastCostModel()
            //{
            //    CostPlanningNo = gcs.Key.mcCostPlanningNo,
            //    Description = gcs.Key.mcDescription,

            //}).ToList();
            return View(@class);
        }
        [HttpPost]
        public PartialViewResult SearchMastCostModel(string DocNo, string Desc, Class @class)
        {
            try
            {
                @class.paramCostNo = DocNo;
                @class.paramCostDes = Desc;
                @class._ViewceMastModel = new ViewceMastModel();
                List<ViewceMastModel> _ViewceMastModel = _MK._ViewceMastModel.OrderBy(x => x.mmModelName).Distinct().ToList();
                SelectList formMastModel = new SelectList(_ViewceMastModel.Select(s => s.mmModelName).Distinct());
                ViewBag.formMastModel = formMastModel;

                @class._ListceMastCostModel = new List<ViewceMastCostModel>();
                @class._ListceMastCostModel = _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == DocNo).ToList();
            }
            catch (Exception ex)
            {

            }

            return PartialView("_PartialMastCostModel", @class);

        }


        public ActionResult DeleteMasterPlanning(string DocNo)
        {
            try
            {
                //var find = _IT.Attachment(X => X.)
                string[] vDocNo = DocNo.Split('-');
                string vDoc = vDocNo[0].ToString();
                string vYear = vDocNo[1].ToString();
                int vRunNo = int.Parse(vDocNo[2].ToString());

                //cerunCostPalnning
                ViewcceRunCostpalnning vRun = _MK._ViewcceRunCostpalnning.Where(x => x.rcRunNo == vRunNo && x.rcDocCode == vDoc && x.rcYear == vYear).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewcceRunCostpalnning.Remove(vRun);
                }


                //MastCostModel
                ViewceMastCostModel find = _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == DocNo).FirstOrDefault();
                // ViewAttachment find = _IT.Attachment.Where(x => x.fnNo == id && x.fnFilename == vname && x.fnProgram == "CostEstimate").FirstOrDefault();
                if (find != null)
                {
                    _MK._ViewceMastCostModel.Remove(find);
                }



                //ceCostPlanning
                var products = _MK._ViewceCostPlanning.Where(p => p.cpCostPlanningNo == DocNo).ToList();
                if (products != null)
                {
                    _MK._ViewceCostPlanning.RemoveRange(products);
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


        public ActionResult DeleteMasterModel(string DocNo, string ModelName)
        {
            try
            {


                //cerunCostPalnning
                ViewceMastCostModel vRun = _MK._ViewceMastCostModel.Where(x => x.mcCostPlanningNo == DocNo && x.mcModelName == ModelName).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMastCostModel.Remove(vRun);
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


        [HttpPost]
        public ActionResult AddMaster(Class @class)
        {
            string config = "S";
            string msg = "Save Data success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {

                    if (@class._ListceMastCostModel != null)
                    {
                        var existingProduct = _MK._ViewceMastCostModel
                                  .Where(p => p.mcCostPlanningNo == @class._ListceMastCostModel[0].mcCostPlanningNo).ToList(); // ใช้เงื่อนไขตามที่คุณต้องการ (เช่น ใช้ `Id` หรือ `Name` เป็นต้น)

                        _MK._ViewceMastCostModel.RemoveRange(existingProduct);
                        _MK.SaveChanges();
                        for (int i = 0; i < @class._ListceMastCostModel.Count(); i++)
                        {


                            var _ViewceMastCostModel = new ViewceMastCostModel()
                            {
                                mcCostPlanningNo = @class._ListceMastCostModel[i].mcCostPlanningNo,
                                mcModelName = @class._ListceMastCostModel[i].mcModelName,
                                mcDescription = @class._ListceMastCostModel[i].mcDescription,
                                mcIssueBy = IssueBy,
                                mcUpdateBy = IssueBy,
                            };
                            _MK._ViewceMastCostModel.AddAsync(_ViewceMastCostModel);
                            //}
                        }
                        _MK.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        config = "N";
                        msg = "ไม่มีข้อมูล";
                    }


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    config = "E";
                    msg = "Error Save: " + ex.InnerException.Message;
                }
            }

            return Json(new { c1 = config, c2 = msg });
            // return Json(new { res = "success" });

        }


        [HttpPost]
        public PartialViewResult SearchMastCostPlanning(string DocNo, string Desc, Class @class)
        {
            try
            {
                @class.paramCostNo = DocNo;
                @class.paramCostDes = Desc;
                List<ViewceMastProcess> _ListceMastProcess = new List<ViewceMastProcess>();
                _ListceMastProcess = _MK._ViewceMastProcess.ToList();
                @class._ListceCostPlanning = new List<ViewceCostPlanning>();

                if (DocNo != null)
                {
                    @class._ListceCostPlanning = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == DocNo).ToList();

                }
                else
                {
                    for (int i = 0; i < _ListceMastProcess.Count(); i++)
                    {
                        @class._ListceCostPlanning.Add(new ViewceCostPlanning
                        {
                            cpCostPlanningNo = "",
                            cpNo = i + 1,
                            cpDescription = "",
                            cpGroupName = _ListceMastProcess[i].mpGroupName,
                            cpProcessName = _ListceMastProcess[i].mpProcessName,
                            cpLabour_Rate = 0,
                            cpDP_Rate = 0,
                            cpME_Rate = 0,
                            cpIssueBy = "",
                            cpUpdateBy = "",
                        });
                    }
                }

                if (@class._ListceCostPlanning.Count() > 0)
                {

                    @class._ListGroupViewCostPlanning = @class._ListceCostPlanning.GroupBy(p => p.cpGroupName).Select(g => new GroupViewCostPlanning
                    {
                        GroupName = g.Key,
                        ceCostPlanning = g.ToList()
                    }).ToList();

                }



            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastCostPlanning", @class);

        }

        [HttpPost]
        public ActionResult AddMasterCostPlanning(Class @class)
        {
            string config = "S";
            string[] vRunCostNo;
            string[] vSaveCost;
            string msg = "Save Data success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            //using (var dbContextTransaction = _MK.Database.BeginTransaction())
            //{
            try
            {
                //run doc 
                vRunCostNo = RunCostNo(@class);
                if (vRunCostNo[0] == "E")
                {
                    config = "E";
                    msg = "Error Save: " + vRunCostNo[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                //save cecostplanning
                vSaveCost = SaveCostPlanning(@class, vRunCostNo[0], vRunCostNo[1], vRunCostNo[2]);
                if (vSaveCost[0] == "E")
                {
                    config = "E";
                    msg = "Error Save: " + vRunCostNo[1];
                    return Json(new { c1 = config, c2 = msg });
                }

            }
            catch (Exception ex)
            {
                //dbContextTransaction.Rollback();
                config = "E";
                msg = "Error Save: " + ex.InnerException.Message;
            }
            //}

            return Json(new { c1 = config, c2 = msg });
            // return Json(new { res = "success" });

        }
        public string[] RunCostNo(Class @class)
        {
            string v_status = "S";
            string v_rundoc = "";
            string vyyyy = DateTime.Now.ToString("yyyy");
            int i_runCostNo = 0;
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    //run doc 
                    if (@class.paramCostNo == null)
                    {
                        v_status = "New";
                        i_runCostNo = _MK._ViewcceRunCostpalnning.Where(x => x.rcYear == vyyyy).FirstOrDefault() == null
                                                                            ? 1 : _MK._ViewcceRunCostpalnning.Where(x => x.rcYear == vyyyy).OrderByDescending(x => x.rcRunNo).Select(x => x.rcRunNo).First() + 1;
                        v_rundoc = "CP-" + vyyyy + "-" + String.Format("{0:D2}", i_runCostNo);
                    }
                    else
                    {
                        v_status = "Update";
                        v_rundoc = @class.paramCostNo;
                    }

                    //save cecostplanning

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    v_status = "E";
                    v_rundoc = "Error Save: " + ex.InnerException.Message;
                }
            }


            string[] vRunCostNo = { v_status, v_rundoc, i_runCostNo.ToString() };
            return vRunCostNo;
        }

        public string[] SaveCostPlanning(Class @class, string vStatus, string RunCostNo, string i_runCostNo)
        {
            string v_status = "S";
            string v_rundoc = "";
            string vyyyy = DateTime.Now.ToString("yyyy");

            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    //run doc
                    if (vStatus == "New")
                    {
                        //insert
                        ViewcceRunCostpalnning _ViewcceRunCostpalnning = new ViewcceRunCostpalnning();
                        _ViewcceRunCostpalnning.rcRunNo = int.Parse(i_runCostNo);
                        _ViewcceRunCostpalnning.rcDocCode = "CP";
                        _ViewcceRunCostpalnning.rcYear = vyyyy;
                        _ViewcceRunCostpalnning.rcIssueBy = IssueBy;
                        _ViewcceRunCostpalnning.rcUpdateBy = IssueBy;
                        _MK._ViewcceRunCostpalnning.AddAsync(_ViewcceRunCostpalnning);

                        


                        //_ListViewceCostPlanning = Count = 34
                        if (@class._ListViewceCostPlanning.Count() > 0)
                        {
                            for (int i = 0; i < @class._ListViewceCostPlanning.Count(); i++)
                            {
                                var _ViewceCostPlanning = new ViewceCostPlanning()
                                {
                                    cpCostPlanningNo = RunCostNo,
                                    cpNo = i + 1,
                                    cpDescription = @class.paramCostDes,
                                    cpGroupName = @class._ListViewceCostPlanning[i].cpGroupName,
                                    cpProcessName = @class._ListViewceCostPlanning[i].cpProcessName,
                                    cpLabour_Rate = @class._ListViewceCostPlanning[i].cpLabour_Rate,
                                    cpDP_Rate = @class._ListViewceCostPlanning[i].cpDP_Rate,
                                    cpME_Rate = @class._ListViewceCostPlanning[i].cpME_Rate,
                                    cpIssueBy = @class._ListViewceCostPlanning[i].cpIssueBy == null ? IssueBy : @class._ListViewceCostPlanning[i].cpIssueBy,
                                    cpUpdateBy = IssueBy,
                                };
                                _MK._ViewceCostPlanning.AddAsync(_ViewceCostPlanning);
                            }

                        }
                        _MK.SaveChanges();
                        //dbContextTransaction.Commit();
                    }
                    else
                    {
                        if (@class._ListViewceCostPlanning.Count() > 0)
                        {
                            var products = _MK._ViewceCostPlanning.Where(p => p.cpCostPlanningNo == RunCostNo).ToList();
                            _MK._ViewceCostPlanning.RemoveRange(products);
                            _MK.SaveChanges();
                            if (@class._ListViewceCostPlanning.Count() > 0)
                            {
                                for (int i = 0; i < @class._ListViewceCostPlanning.Count(); i++)
                                {
                                    var _ViewceCostPlanning = new ViewceCostPlanning()
                                    {
                                        cpCostPlanningNo = RunCostNo,
                                        cpNo = i + 1,
                                        cpDescription = @class.paramCostDes,
                                        cpGroupName = @class._ListViewceCostPlanning[i].cpGroupName,
                                        cpProcessName = @class._ListViewceCostPlanning[i].cpProcessName,
                                        cpLabour_Rate = @class._ListViewceCostPlanning[i].cpLabour_Rate,
                                        cpDP_Rate = @class._ListViewceCostPlanning[i].cpDP_Rate,
                                        cpME_Rate = @class._ListViewceCostPlanning[i].cpME_Rate,
                                        cpIssueBy = @class._ListViewceCostPlanning[i].cpIssueBy == null ? IssueBy : @class._ListViewceCostPlanning[i].cpIssueBy,
                                        cpUpdateBy = IssueBy,
                                    };
                                    _MK._ViewceCostPlanning.AddAsync(_ViewceCostPlanning);
                                }
                                //_MK._ViewceCostPlanning.AddRangeAsync(@class._ListViewceCostPlanning);
                            }

                            List<ViewceMastCostModel> _ViewceMastCostModel = _MK._ViewceMastCostModel.Where(x=>x.mcCostPlanningNo == RunCostNo).ToList();
                            _ViewceMastCostModel.Where(p => p.mcCostPlanningNo == RunCostNo).ToList().ForEach(p =>
                            {
                                p.mcDescription = @class.paramCostDes;
                            });
                            
                            _MK.SaveChanges();
                            // dbContextTransaction.Commit();
                        }


                    }
                    dbContextTransaction.Commit();
                    //save cecostplanning


                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    v_status = "E";
                    v_rundoc = "Error Save: " + ex.InnerException.Message;
                }
            }


            string[] vRunCostNo = { v_status, v_rundoc };
            return vRunCostNo;
        }
    }
}