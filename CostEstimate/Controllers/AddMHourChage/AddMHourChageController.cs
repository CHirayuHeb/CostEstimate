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
namespace CostEstimate.Controllers.AddMHourChage
{
    public class AddMHourChageController : Controller
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

        public AddMHourChageController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            //// @class._ListViewceMastHourChage = new List<ViewceMastHourChage>();
            // @class._ListViewceMastHourChage = _MK._ViewceMastHourChage.ToList();
            try
            {
                @class._ListViewceMastHourChage = new List<ViewceMastHourChage>();
                @class._ListViewceMastHourChage = _MK._ViewceMastHourChage.ToList();
            }
            catch (Exception ex)
            {
                string ee = ex.Message;
            }
          
            return View(@class);
        }
        [HttpPost]
        public PartialViewResult SearchMastHourChage(int mpNo, Class @class)
        {
            try
            {
                @class._ViewceMastHourChage = new ViewceMastHourChage();

                //@class._ViewceMastProcess = new ViewceMastProcess();
                if (mpNo > 0)
                {
                    @class._ViewceMastHourChage = _MK._ViewceMastHourChage.Where(x => x.mhId == mpNo).FirstOrDefault();


                }





            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastHourChage", @class);

        }

        [HttpPost]
        public ActionResult AddMastHourChage(Class @class)
        {
            string config = "S";
         
            string msg = "Save Master Hour Chage success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if (@class._ViewceMastHourChage.mhId > 0)
                    {
                        ViewceMastHourChage _ViewceMastHourChage = _MK._ViewceMastHourChage.Where(x => x.mhId == @class._ViewceMastHourChage.mhId).FirstOrDefault();
                        if (_ViewceMastHourChage != null)
                        {
                            //_ViewceMastHourChage.mhId = @class._ViewceMastHourChage.mhId;
                            _ViewceMastHourChage.mhGroupMain = @class._ViewceMastHourChage.mhGroupMain;
                            _ViewceMastHourChage.mhGroupSub = @class._ViewceMastHourChage.mhGroupSub;
                            _ViewceMastHourChage.mhProcessName = @class._ViewceMastHourChage.mhProcessName;
                            //_ViewceMastHourChage.mhId = @class._ViewceMastHourChage.mhId;
                            //_ViewceMastHourChage.mpUpdateBy = IssueBy;
                            _MK._ViewceMastHourChage.Update(_ViewceMastHourChage);
                        }
                    }
                    else
                    {
                        ViewceMastHourChage _ViewceMastHourChage = new ViewceMastHourChage();
                        _ViewceMastHourChage.mhGroupMain = @class._ViewceMastHourChage.mhGroupMain;
                        _ViewceMastHourChage.mhGroupSub = @class._ViewceMastHourChage.mhGroupSub;
                        _ViewceMastHourChage.mhProcessName = @class._ViewceMastHourChage.mhProcessName;
                        _MK._ViewceMastHourChage.Add(_ViewceMastHourChage);
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
        public ActionResult DeleteMastHourChage(int mhid)
        {
            try
            {
                //cerunCostPalnning
                ViewceMastHourChage vRun = _MK._ViewceMastHourChage.Where(x => x.mhId == mhid).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMastHourChage.Remove(vRun);
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