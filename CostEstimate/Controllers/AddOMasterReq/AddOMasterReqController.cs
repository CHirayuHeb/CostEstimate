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
namespace CostEstimate.Controllers.AddOMasterReq
{
    public class AddOMasterReqController : Controller
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

        public AddOMasterReqController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListViewceMaster = new List<ViewceMaster>();
            @class._ListViewceMaster = _MK._ViewceMaster.Where(x => x.msProgram == "MoldOther").ToList();

            return View(@class);
        }

        public ActionResult DeleteMasterItem(int msId)
        {
            try
            {
                //cerunCostPalnning
                ViewceMaster vRun = _MK._ViewceMaster.Where(x => x.msid == msId).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMaster.Remove(vRun);
                }
                _MK.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new { res = "error: " + ex.Message });

            }
            return Json(new { res = "success" });

        }


        [HttpPost]
        public PartialViewResult SearchMasterItem(int msId, Class @class)
        {
            try
            {
                ////_listTypeMold
                //List<string> _listmtType = _MK._ViewceMastType.Where(x => x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtType).Select(x => x.mtType).Distinct().ToList();
                //SelectList _TypemtType = new SelectList(_listmtType);
                //ViewBag._TypemtType = _TypemtType;

                //@class._ViewceMastType = new ViewceMastType();
                //if (mtId > 0 && itemNname != null)
                //{
                //    @class._ViewceMastType = _MK._ViewceMastType.Where(x => x.mtId == mtId).FirstOrDefault();


                //}

            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastMaster", @class);

        }
    }
}