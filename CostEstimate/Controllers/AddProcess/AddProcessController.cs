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

namespace CostEstimate.Controllers.AddProcess
{
    public class AddProcessController : Controller
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
        public AddProcessController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastProcess = new List<ViewceMastProcess>();
            @class._ListceMastProcess = _MK._ViewceMastProcess.OrderBy(z => z.mpGroupName.Trim()).ToList();



            return View(@class);
        }

        public ActionResult DeleteMasterProcess(string processGroup, string processName)
        {
            try
            {
                //cerunCostPalnning
                ViewceMastProcess vRun = _MK._ViewceMastProcess.Where(x => x.mpGroupName == processGroup && x.mpProcessName == processName).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMastProcess.Remove(vRun);
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
        public PartialViewResult SearchMastProcess(int mpNo, Class @class)
        {
            try
            {
                @class._ViewceMastProcess = new ViewceMastProcess();
                if(mpNo > 0)
                {
                    @class._ViewceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpNo == mpNo).FirstOrDefault();


                }





            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastProcess", @class);

        }
        [HttpPost]
        public ActionResult AddMasterProcess(Class @class)
        {
            string config = "S";
            string[] vRunCostNo;
            string[] vSaveCost;
            string msg = "Save Master Process success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if(@class._ViewceMastProcess.mpNo > 0)
                    {
                        ViewceMastProcess _viewceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpNo == @class._ViewceMastProcess.mpNo).FirstOrDefault();
                        if (_viewceMastProcess != null)
                        {
                            _viewceMastProcess.mpGroupName = @class._ViewceMastProcess.mpGroupName;
                            _viewceMastProcess.mpProcessName = @class._ViewceMastProcess.mpProcessName;
                            _viewceMastProcess.mpEnable_WTMan = @class._ViewceMastProcess.mpEnable_WTMan;
                            _viewceMastProcess.mpEnable_WTAuto = @class._ViewceMastProcess.mpEnable_WTAuto;
                            _viewceMastProcess.mpUpdateBy = IssueBy;
                            _MK._ViewceMastProcess.Update(_viewceMastProcess);
                        }
                    }
                    else
                    {
                        ViewceMastProcess _viewceMastProcess = new ViewceMastProcess();
                        _viewceMastProcess.mpGroupName = @class._ViewceMastProcess.mpGroupName;
                        _viewceMastProcess.mpProcessName = @class._ViewceMastProcess.mpProcessName;
                        _viewceMastProcess.mpEnable_WTMan = @class._ViewceMastProcess.mpEnable_WTMan;
                        _viewceMastProcess.mpEnable_WTAuto = @class._ViewceMastProcess.mpEnable_WTAuto;
                        _viewceMastProcess.mpIssueBy = IssueBy;
                        _viewceMastProcess.mpUpdateBy = IssueBy;
                        _MK._ViewceMastProcess.Add(_viewceMastProcess);
                    }
                   
                    _MK.SaveChanges();
                   dbContextTransaction.Commit();
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
    }
}