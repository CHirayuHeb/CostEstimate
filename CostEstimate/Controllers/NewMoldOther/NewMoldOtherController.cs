using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using CostEstimate.Models.Common;
using CostEstimate.Models.DBConnect;
using CostEstimate.Models.MyRequest;
using CostEstimate.Models.New;
using CostEstimate.Models.Table.HRMS;
using CostEstimate.Models.Table.IT;
using CostEstimate.Models.Table.LAMP;
using CostEstimate.Models.Table.MOLD;
using CostEstimate.Models.Table.MK;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.AspNetCore.Http;
using System.IO;

using MimeKit;
using MailKit.Net.Smtp;

namespace CostEstimate.Controllers.NewMoldOther
{
    public class NewMoldOtherController : Controller
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
        public NewMoldOtherController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            List<string> _listRequestBy = _MK._ViewceMastType.Where(x => x.mtType.Contains("RequestBy") && x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList _TypeofRequestBy = new SelectList(_listRequestBy);
            ViewBag.TypeofRequestBy = _TypeofRequestBy;

            @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();

            @class._ViewceMastModifyRequest = new ViewceMastModifyRequest();
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "2").ToList();

            List<string> _listTypeofCavity = _MK._ViewceMastType.Where(x => x.mtType.Contains("Cavity") && x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
            ViewBag.TypeofCavity = _TypeofCavity;

            //List<string> _listEvent = _MK._ViewceMastType.Where(x => x.mtType.Contains("Event") && x.mtProgram.Contains("MoldOther")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            //SelectList _TypeofEvent = new SelectList(_listEvent);
            //ViewBag.TypeofEvent = _TypeofEvent;




            // @class._ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == "CE-M-25-07-030").FirstOrDefault();

            return View(@class);
        }


        [Authorize("Checked")]
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            // int v_step = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfStep : 0;
            int v_step = 2;
            string v_issue = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastModifyRequest != null ? @classs._ViewceMastModifyRequest.mfCENo : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            partialUrl = Url.Action("SendMail", "NewMoldOther", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });
            //try
            //{
            //    if (@classs._ViewceMastModifyRequest != null)
            //    {
            //        if (@classs._ViewceMastModifyRequest.mfCENo != "" && @classs._ViewceMastModifyRequest.mfCENo != null)
            //        {
            //            // htCostPlanningNo
            //            String htDocNo = @classs._ViewceMastModifyRequest.mfCENo.ToString(); //htCostPlanningNo
            //                                                                                 //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htStep).ThenBy(x=>x.htDate).ThenBy(x=>x.htTime).ToList();
            //            _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
            //            if (_listHistory.Count() > 0)
            //            {
            //                for (int j = 0; j < _listHistory.Count(); j++)
            //                {
            //                    var v_htcc = _listHistory[j].htCC;
            //                    string v_CCemail = "";
            //                    if (v_htcc != null)
            //                    {
            //                        ViewrpEmail fromEmailCC = new ViewrpEmail();
            //                        string[] splitCC = v_htcc.Split(',');
            //                        foreach (var i in splitCC)
            //                        {
            //                            if (i != " " & i != "")
            //                            {
            //                                var v_cc = "";
            //                                try
            //                                {
            //                                    fromEmailCC = _IT.rpEmails.Where(w => w.emEmpcode == i.Trim()).FirstOrDefault();
            //                                    v_CCemail += fromEmailCC.emName_M365.ToString() + ",";
            //                                }
            //                                catch (Exception e)
            //                                {
            //                                    v_cc = e.Message;
            //                                }
            //                            }
            //                        }
            //                    }

            //                    _listHistory[j].htCC = v_CCemail;


            //                }

            //            }


            //            return Json(new { status = "hasHistory", listHistory = _listHistory, partial = partialUrl });
            //        }
            //    }

            //}
            //catch (Exception e)
            //{

            //}

            //return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
            return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
        }
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;

            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365

            //for (int i = 0; i < 4; i++)
            //{
            //    @class._ListViewceHistoryApproved.Add(new ViewceHistoryApproved
            //    {
            //        htNo = 0,
            //        htDocNo = "",
            //        htStep = 2,
            //        htStatus = "Approve",
            //        htFrom = v_emailFrom,
            //        htTo = "",
            //        htCC="",
            //        htDate = DateTime.Now.ToString("yyyy/MM/dd"),
            //        htTime = "",
            //        htRemark ="",
            //    });
            //}




            //var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            //if (mpNo != null && mpNo != "")
            //{
            //    @class._ViewceMastModifyRequest = _MK._ViewceMastModifyRequest.Where(x => x.mfCENo == mpNo).FirstOrDefault();
            //    s_issue = @class._ViewceMastModifyRequest.mfEmpCodeRequest;
            //}

            ////get emp operator
            //var v_empstep = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "2") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "2").Select(x => x.mfTo).FirstOrDefault() : "";
            //if (v_empstep != null) //step 2-5
            //{
            //    var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empstep).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            //    @class._ViewceHistoryApproved.htTo = v_emailTo;
            //}

            ////step 6 Waiting Apporve By DM For ST Department ==> issue
            //if (s_step == 5)
            //{
            //    //var vdocNo = @class.
            //    var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == s_issue).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            //    @class._ViewceHistoryApproved.htTo = v_emailTo;


            //    //string tbHistory2 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 1).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 1).Select(x => x.htFrom).FirstOrDefault();
            //    //string tbHistory2EMPCODE = tbHistory2 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory2)).Select(x => x.emEmpcode).FirstOrDefault();

            //    string tbHistory3 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault();
            //    string tbHistory3EMPCODE = tbHistory3 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory3)).Select(x => x.emEmpcode).FirstOrDefault();

            //    string tbHistory2 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault();
            //    string tbHistory2EMPCODE = tbHistory2 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory2)).Select(x => x.emEmpcode).FirstOrDefault();

            //    string tbHistory5 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
            //    string tbHistory5EMPCODE = tbHistory5 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory5)).Select(x => x.emEmpcode).FirstOrDefault();


            //    @class._ViewceHistoryApproved.htCC = tbHistory2 + "," + tbHistory3 + "," + tbHistory5 + ",";
            //    //@class._ViewceHistoryApproved.htCC = tbHistory3 + "," + tbHistory5 + ",";

            //}


            ////step 4 fix ST dept check

            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            //ViewBag.step = s_step;
            if (s_step == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    @class._ListViewceHistoryApproved.Add(new ViewceHistoryApproved
                    {
                        htNo = 0,
                        htDocNo = "",
                        htStep = 2,
                        htStatus = "Approve",
                        htFrom = v_emailFrom,
                        htTo = "",
                        htCC = "",
                        htDate = DateTime.Now.ToString("yyyy/MM/dd"),
                        htTime = "",
                        htRemark = "",
                    });
                }
                return PartialView("SendMail_step2", @class);
            }
            else
            {
                return PartialView("SendMail", @class);
            }

        }

        public ActionResult SearchMold_Ledger_Number(string term)
        {

            return Json(
                        _MOLD._ViewLLLedger
                            .Where(p => p.LGLegNo.Contains(term))
                            .Select(p =>
                                p.LGLegNo + "" +
                                p.LGTypeCode + "|" +
                                p.LGCustomer + "|" +
                                p.LGMoldNo + "|" + p.LGMoldName + "|" +
                                "0"
                            )
                            .ToList()
                        );

        }

        public ActionResult SearchCustomer(string term)
        {

            return Json(
                        _MK._ViewceMastType
                            .Where(p => p.mtName.Contains(term) && p.mtProgram == "MoldOther" && p.mtType == "Customer")
                            .Select(p => p.mtName
                            )
                            .ToList()
                        );

        }
        public ActionResult SearchFuntion(string term)
        {

            return Json(
                        _MK._ViewceMastType
                            .Where(p => p.mtName.Contains(term) && p.mtProgram == "MoldOther" && p.mtType == "Function")
                            .Select(p => p.mtName
                            )
                            .ToList()
                        );

        }
        public ActionResult SearchModel(string term)
        {

            return Json(
                        _MK._ViewceMastModel
                            .Where(p => p.mmModelName.Contains(term) && p.mmType == "MoldOther")
                            .Select(p => p.mmModelName
                            )
                            .ToList()
                        );

        }

        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ceItemModifyRequest)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";
            return Json(new { c1 = config, c2 = msg });
        }


    }
}