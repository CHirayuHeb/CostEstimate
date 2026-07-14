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

namespace CostEstimate.Controllers.AddOMaster
{
    public class AddOMasterController : Controller
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

        public AddOMasterController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastType = new List<ViewceMastType>();
            @class._ListceMastType = _MK._ViewceMastType.Where(x => x.mtProgram == "MoldOther").OrderBy(z => z.mtType.Trim()).ToList();

            List<string> _listmtType = _MK._ViewceMastType.Where(x => x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtType).Select(x => x.mtType).Distinct().ToList();
            SelectList _TypemtType = new SelectList(_listmtType);
            ViewBag._TypemtType = _TypemtType;

            //@class._ViewceMastType = new ViewceMastType();

            return View(@class);
        }
        [HttpPost]
        public PartialViewResult SearchMasterItem(int mtId, string itemNname, Class @class)
        {
            try
            {
                //_listTypeMold
                List<string> _listmtType = _MK._ViewceMastType.Where(x =>  x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtType).Select(x => x.mtType).Distinct().ToList();
                SelectList _TypemtType = new SelectList(_listmtType);
                ViewBag._TypemtType = _TypemtType;

                @class._ViewceMastType = new ViewceMastType();
                if (mtId > 0 && itemNname != null)
                {
                    @class._ViewceMastType = _MK._ViewceMastType.Where(x => x.mtId == mtId).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastMaster", @class);

        }
        public ActionResult DeleteMasterItem(int mtId, string ModelName)
        {
            try
            {
                //cerunCostPalnning
                ViewceMastType vRun = _MK._ViewceMastType.Where(x => x.mtId == mtId).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMastType.Remove(vRun);
                }
                _MK.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { res = "error: " + ex.Message });

            }
            return Json(new { res = "success" });

        }
        public ActionResult AddMasterMaster(Class @class)
        {
            string config = "S";
            string[] vRunCostNo;
            string[] vSaveCost;
            string msg = "Save Master Mold  Other Master Item success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if (@class._ViewceMastType.mtId > 0)
                    {
                        ViewceMastType _ViewceMastType = _MK._ViewceMastType.Where(x => x.mtId == @class._ViewceMastType.mtId).FirstOrDefault();
                        if (_ViewceMastType != null)
                        {
                            _ViewceMastType.mtName = @class._ViewceMastType.mtName;
                            _ViewceMastType.mtType = @class._ViewceMastType.mtType;
                            _ViewceMastType.mtProgram = "MoldOther";
                            _ViewceMastType.mtIssueBy = IssueBy;
                            _MK._ViewceMastType.Update(_ViewceMastType);
                        }
                    }
                    else
                    {
                        ViewceMastType _ViewceMastType = new ViewceMastType();
                        // _ViewceMastModel.mpProcessName = @class._ViewceMastProcess.mpProcessName;
                        _ViewceMastType.mtName = @class._ViewceMastType.mtName;
                        _ViewceMastType.mtType = @class._ViewceMastType.mtType;
                        _ViewceMastType.mtProgram = "MoldOther";
                        _ViewceMastType.mtIssueBy = IssueBy;
                        _ViewceMastType.mtIssueBy = IssueBy;
                        _MK._ViewceMastType.Add(_ViewceMastType);
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