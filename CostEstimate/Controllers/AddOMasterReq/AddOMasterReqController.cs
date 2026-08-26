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
            try
            {
                @class._ViewceMaster = new ViewceMaster();

                @class._ListViewceMaster = new List<ViewceMaster>();
                @class._ListViewceMaster = _MK._ViewceMaster.Where(x => x.msProgram == "MoldOther").ToList();

                @class._ListViewceMaster_Type = new List<ViewceMaster_Type>();
               // @class._ListViewceMaster_Type = _MK._ViewceMaster_Type.ToList();

                List<string> _listmsType = _MK._ViewceMaster.Where(x => x.msProgram.Contains("MoldOther")).Select(x => x.msDes).Distinct().ToList();
                SelectList _TypemsType = new SelectList(_listmsType);
                ViewBag._TypemsType = _TypemsType;

            }
            catch (Exception ex)
            {
                string megr = ex.Message;
            }
         



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

                List<string> _listmsType = _MK._ViewceMaster.Where(x => x.msProgram.Contains("MoldOther")).Select(x => x.msDes).Distinct().ToList();
                SelectList _TypemsType = new SelectList(_listmsType);
                ViewBag._TypemsType = _TypemsType;
                
                @class._ViewceMaster = new ViewceMaster();
                if (msId > 0 )
                {
                    @class._ViewceMaster = _MK._ViewceMaster.Where(x => x.msid == msId).FirstOrDefault();
                }
              
            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastMasterReq", @class);

        }

        [HttpPost]
        public PartialViewResult SearchMasterParentItem(int msId, Class @class)
        {
            try
            {

                //List<string> _listmsType = _MK._ViewceMaster.Where(x => x.msProgram.Contains("MoldOther")).Select(x => x.msDes).Distinct().ToList();
                //SelectList _TypemsType = new SelectList(_listmsType);
                //ViewBag._TypemsType = _TypemsType;

                @class._ViewceMaster = new ViewceMaster();
                @class._ViewceMaster = _MK._ViewceMaster.Where(x => x.msid == msId).FirstOrDefault();
                //if (msId > 0)
                //{
                //    @class._ViewceMaster = _MK._ViewceMaster.Where(x => x.msid == msId).FirstOrDefault();
                //}
                @class._ListViewceMaster_Type = new List<ViewceMaster_Type>();
                @class._ListViewceMaster_Type = _MK._ViewceMaster_Type.Where(x=>x.mtParent_id == msId && x.mtDes == "ModelName" && x.mtProgram == "MoldOther").ToList();




            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialMastAddMasterReq", @class);

        }

        public ActionResult AddMasterMaster(Class @class)
        {
            string config = "S";
            //string[] vRunCostNo;
           // string[] vSaveCost;
            string msg = "Save Master Mold  Other Master Item success!!";
            string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    if (@class._ViewceMaster.msid > 0)
                    {
                        ViewceMaster _ViewceMaster = _MK._ViewceMaster.Where(x => x.msid == @class._ViewceMaster.msid).FirstOrDefault();
                        if (_ViewceMaster != null)
                        {
                            _ViewceMaster.msItem = @class._ViewceMaster.msItem;
                            _ViewceMaster.msDes = @class._ViewceMaster.msDes;
                            _ViewceMaster.msProgram = "MoldOther";
                            _ViewceMaster.msUpdateBy = IssueBy;
                            _MK._ViewceMaster.Update(_ViewceMaster);
                        }
                    }
                    else
                    {
                        ViewceMaster _ViewceMaster = new ViewceMaster();
                        // _ViewceMastModel.mpProcessName = @class._ViewceMastProcess.mpProcessName;
                        _ViewceMaster.msItem = @class._ViewceMaster.msItem;
                        _ViewceMaster.msDes = @class._ViewceMaster.msDes;
                        _ViewceMaster.msIsActive = true;
                        _ViewceMaster.msProgram = "MoldOther";
                        _ViewceMaster.msUpdateBy = IssueBy;
                        _MK._ViewceMaster.Add(_ViewceMaster);
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


        [HttpPost]
        public IActionResult DeleteMasterItemParent(int id)
        {
            try
            {
                ViewceMaster_Type vRun = _MK._ViewceMaster_Type.Where(x => x.mtid == id).FirstOrDefault();
                if (vRun != null)
                {
                    _MK._ViewceMaster_Type.Remove(vRun);
                }
                _MK.SaveChanges();
                // ... โค้ดลบข้อมูลใน DB ด้วย model.Id ...
                return Json(new { success = true, message = "ลบสำเร็จ" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddMasterItemParent(int msid, string name)
        {
            try
            {
                // สร้างข้อมูล User + Date
                string userId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value ?? "System";
                string IssueBy = DateTime.Now.ToString("yyyy/MM/dd") + " : " + userId;

                // บันทึกลง Database
                var newItem = new ViewceMaster_Type
                {
                    mtParent_id = msid,
                    mtItem = name,
                    mtDes = "ModelName",
                    mtIsActive = true,
                    mtProgram = "MoldOther",
                    mtCreateBy = IssueBy,
                    mtUpdateBy = IssueBy
                };

                _MK._ViewceMaster_Type.Add(newItem);
                _MK.SaveChanges(); // บันทึกและดึง mtid ล่าสุดมาอัตโนมัติ

                // คืนค่ากลับไปวาด Row ในหน้าเว็บ
                return Json(new
                {
                    success = true,
                    message = "บันทึกสำเร็จ",
                    item = new
                    {
                        mtid = newItem.mtid,
                        mtParent_id = newItem.mtParent_id,
                        mtItem = newItem.mtItem,
                        mtDes = newItem.mtDes,
                        mtIsActive = newItem.mtIsActive,
                        mtProgram = newItem.mtProgram
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}