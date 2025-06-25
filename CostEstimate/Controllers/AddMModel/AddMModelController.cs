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


namespace CostEstimate.Controllers.AddMModel
{
    public class AddMModelController : Controller
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

        public AddMModelController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastModel = new List<ViewceMastModel>();
            @class._ListceMastModel = _MK._ViewceMastModel.Where(x => x.mmType == "MoldModify").OrderBy(z => z.mmModelName.Trim()).ToList();
            return View(@class);
        }

        [HttpPost]
        public PartialViewResult SearchMastModel(int mmNo, string ModelName, Class @class)
        {
            try
            {
                @class._ViewceMastProcess = new ViewceMastProcess();
                if (mmNo > 0 && ModelName != null)
                {
                    @class._ViewceMastModel = _MK._ViewceMastModel.Where(x => x.mmNo == mmNo && x.mmModelName == ModelName && x.mmType == "MoldModify").FirstOrDefault();


                }

            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastMModel", @class);

        }
        public ActionResult DeleteMasterModel(int mmNo, string ModelName)
        {
            try
            {
                //cerunCostPalnning
                ViewceMastModel vRun = _MK._ViewceMastModel.Where(x => x.mmNo == mmNo && x.mmModelName == ModelName && x.mmType == "MoldModify").FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMastModel.Remove(vRun);
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
        public ActionResult AddMasterModel(Class @class)
        {
            string config = "S";
            string[] vRunCostNo;
            string[] vSaveCost;
            string msg = "Save Master Mold Model success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if (@class._ViewceMastModel.mmNo > 0)
                    {
                        ViewceMastModel _ViewceMastModel = _MK._ViewceMastModel.Where(x => x.mmNo == @class._ViewceMastModel.mmNo).FirstOrDefault();
                        if (_ViewceMastModel != null)
                        {
                            _ViewceMastModel.mmModelName = @class._ViewceMastModel.mmModelName;
                            _ViewceMastModel.mmType = "MoldModify";
                            _ViewceMastModel.mcUpdateBy = IssueBy;
                            _MK._ViewceMastModel.Update(_ViewceMastModel);
                        }
                    }
                    else
                    {
                        ViewceMastModel _ViewceMastModel = new ViewceMastModel();
                        // _ViewceMastModel.mpProcessName = @class._ViewceMastProcess.mpProcessName;
                        _ViewceMastModel.mmModelName = @class._ViewceMastModel.mmModelName;
                        _ViewceMastModel.mmType = "MoldModify";
                        _ViewceMastModel.mcIssueBy = IssueBy;
                        _ViewceMastModel.mcUpdateBy = IssueBy;
                        _MK._ViewceMastModel.Add(_ViewceMastModel);
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