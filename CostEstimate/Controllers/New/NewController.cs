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
using MailKit.Security;
using System.Net.Mail;

namespace CostEstimate.Controllers.New
{
    public class NewController : Controller
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

        public NewController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        //public IActionResult Index(Class @class, string smLotNo, string smOrderNo, string smRevision)
        public IActionResult Index(Class @class, string smDocumentNo, string smRevision)
        {





            @class._ViewOperaterCP = new ViewOperaterCP();
            List<ViewmtMaster_Mold_Control> _ViewmtMaster_Mold_Control = _MOLD._ViewmtMaster_Mold_Control.OrderBy(x => x.mcLedger_Number).Distinct().ToList();
            SelectList formMaster_Mold_Control = new SelectList(_ViewmtMaster_Mold_Control.Select(s => s.mcLedger_Number).Distinct());
            ViewBag.vbformMaster_Mold_Control = formMaster_Mold_Control;


            List<string> _listTypeofCavity = _MK._ViewceMastType.Where(x => x.mtType.Contains("Cavity") && x.mtProgram.Contains("SubMaker")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();

            //List<string> _listTypeofCavity = new List<string>{
            //                                "CAVITIES(R/L =1 Set) x 2MOLD",
            //                                "CAVITIES(R/L =1 Set)",
            //                                "CAVITIES",
            //                                "CAVITIES x 2 MOLD",
            //                                "CAVITY x 2 MOLD",
            //                                "CAVITY"};
            SelectList _TypeofCavity = new SelectList(_listTypeofCavity);
            ViewBag.TypeofCavity = _TypeofCavity;


            // @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.ToList();
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "1").ToList();

            try
            {
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._listAttachment = new List<ViewAttachment>();
                // if (smLotNo != null && smOrderNo != null && smRevision != null)
                if (smDocumentNo != null)
                {
                    //@class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == smLotNo && x.smOrderNo == smOrderNo && x.smRevision == smRevision).FirstOrDefault();
                    @class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == smDocumentNo).FirstOrDefault();
                    //if (@class._ViewceMastSubMakerRequest == null)
                    //{
                    //    return RedirectToAction("Index", "PageNotFound");
                    //    // return View(@class);
                    //}
                    if (@class._ViewceMastSubMakerRequest == null)
                    {
                        return RedirectToAction("Index", "ErrorPage");
                        // return View(@class);
                    }

                    //@class._ListceDetailSubMakerRequest = _MK._ViewceDetailSubMakerRequest.Where(x => x.dsDocumentNo == smDocumentNo).ToList();
                    @class._ViewceMastSubMakerRequest.smTotalProCost = @class._ViewceMastSubMakerRequest.smTotalProcessWT + @class._ViewceMastSubMakerRequest.smTotalProcessCost + @class._ViewceMastSubMakerRequest.smOrderMatl + @class._ViewceMastSubMakerRequest.smTotalCost + @class._ViewceMastSubMakerRequest.smRoundUp;


                    @class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                    @class._ListceDetailSubMakerRequest = _MK._ViewceDetailSubMakerRequest.Where(x => x.dsDocumentNo == smDocumentNo).ToList();

                    if (smRevision != null)
                    {
                        @class._ViewceMastSubMakerRequest.smDocumentNo = "";
                        @class._ViewceMastSubMakerRequest.smRevision = String.Format("{0:D2}", int.Parse(@class._ViewceMastSubMakerRequest.smRevision) + 1);
                        @class._ViewceMastSubMakerRequest.smEmpCodeRequest = "";
                        @class._ViewceMastSubMakerRequest.smNameRequest = "";
                        @class._ViewceMastSubMakerRequest.smEmpCodeApprove = "";
                        @class._ViewceMastSubMakerRequest.smNameApprove = "";
                        @class._ViewceMastSubMakerRequest.smStep = 0;
                        @class._ViewceMastSubMakerRequest.smStatus = "";

                        for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                        {
                            @class._ListceDetailSubMakerRequest[i].dsDocumentNo = "";
                            @class._ListceDetailSubMakerRequest[i].dsRevision = String.Format("{0:D2}", int.Parse(@class._ViewceMastSubMakerRequest.smRevision) + 1);
                        }

                    }
                    else
                    {
                        @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == smDocumentNo).ToList();
                    }



                    @class._ListGroupViewceDetailSubMakerRequest = @class._ListceDetailSubMakerRequest.GroupBy(p => p.dsGroupName).Select(g => new GroupViewceDetailSubMakerRequest
                    {
                        GroupName = g.Key.Trim(),
                        DetailSubMakerRequest = g.ToList()
                    }).ToList();



                }

            }
            catch (Exception ex)
            {
                ViewBag.test = ex.Message;
            }


            //ViewceMastSubMakerRequest _ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == "1").Select(""));
            //ViewceMastSubMakerRequest _ViewceMastSubMakerRequest  = _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == "1").FirstOrDefault(); //  _MK._ViewceMastSubMakerRequest.Where(x => x.smLotNo == "1").FirstOrDefault();
            //@class._ViewceMastSubMakerRequest = _ViewceMastSubMakerRequest;

            //@class._ViewAccEMPLOYEE = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == id).FirstOrDefault();
            //@class._ViewceDetailSubMakerRequest.dsOrderNo = "11";
            // ViewBag.test = "11";
            return View(@class);
        }


        [Authorize("Checked")]
        //public IActionResult Index(Class @class, string smLotNo, string smOrderNo, string smRevision)
        public IActionResult Idex404(Class @class)
        {
            return View(@class);
        }

        public FileResult openFile(string pathFile)
        {
            string locationfile = path + "/" + pathFile;
            // string locationfile = @"//thsweb//MAINTENANCE_MOLD/denso_requestment.txt";
            string extension = Path.GetExtension(locationfile);
            byte[] fileByte = System.IO.File.ReadAllBytes(locationfile);

            return File(fileByte, "application/octet-stream", locationfile);

        }


        public ActionResult SearchMold_Ledger_Number(string term)
        {
            {
                List<ViewLLLedger> __ViewLLLedger = new List<ViewLLLedger>();
                __ViewLLLedger = _MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).ToList();
                // _MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).Select(p => p.LGLegNo + "" + p.LGTypeCode + "|" + p.LGCustomer + "|" + p.LGMoldName + "" + p.LGMoldNo + "|" + "0"+ "|" + p.LGIcsName).ToList());

                return Json(_MOLD._ViewLLLedger.Where(p => p.LGLegNo.Contains(term)).Select(p => p.LGLegNo + "" + p.LGTypeCode + "|" + p.LGCustomer + "|" + p.LGMoldNo + "/" + p.LGMoldName + "|" + "0" + "|" + (p.LGIcsName ?? "") + "|" + (p.LGPart ?? "-")).ToList());
                // return Json(_MOLD._ViewmtMaster_Mold_Control.Where(p => p.mcLedger_Number.Contains(term)).Select(p => p.mcLedger_Number + "|" + p.mcCUS + "|" + p.mcMoldname + "|" + (p.mcCavity != "" ? p.mcCavity : "0")).ToList());
            }
        }
        public ActionResult SearchModelName(string term)
        {
            {
                string a = _MK._ViewceMastCostModel.Where(p => p.mcModelName.Contains(term)).Select(p => p.mcModelName).First();
                return Json(_MK._ViewceMastCostModel.Where(p => p.mcModelName.Contains(term)).Select(p => p.mcModelName).Distinct().ToList());
            }
        }

        [HttpPost]
        public PartialViewResult SearchbyModelName(string search, Class @class)
        {
            string msg = "";
            try
            {

                List<ViewceMastProcess> _ListceMastProcess = new List<ViewceMastProcess>();
                string v_CostPlanningNo = _MK._ViewceMastCostModel.Where(x => x.mcModelName == search).OrderByDescending(x => x.mcCostPlanningNo).Select(x => x.mcCostPlanningNo).First();

                List<ViewceCostPlanning> _ViewceCostPlanning = new List<ViewceCostPlanning>();
                var v_ListceCostPlanning = _MK._ViewceCostPlanning.Where(x => x.cpCostPlanningNo == v_CostPlanningNo).ToList();


                _ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();
                @class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "subMaker").ToList();

                List<ViewceDetailSubMakerRequest> _ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                for (int i = 0; i < v_ListceCostPlanning.Count(); i++)
                {
                    try
                    {


                        _ListceDetailSubMakerRequest.Add(new ViewceDetailSubMakerRequest
                        {
                            dsDocumentNo = "",
                            dsRunNo = 0,
                            dsLotNo = "",
                            dsOrderNo = "",
                            dsRevision = "",
                            dsGroupName = v_ListceCostPlanning[i].cpGroupName.Trim(),
                            dsProcessName = v_ListceCostPlanning[i].cpProcessName,

                            dsWT_Man = 0,
                            dsWT_Auto = 0,

                            dsEnable_WTAuto = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTAuto).First(),  //true,
                            dsEnable_WTMan = _ListceMastProcess.Where(x => x.mpGroupName.Contains(v_ListceCostPlanning[i].cpGroupName) && x.mpProcessName.Contains(v_ListceCostPlanning[i].cpProcessName)).Select(x => x.mpEnable_WTMan).First(),

                            dsLabour_Rate = v_ListceCostPlanning[i].cpLabour_Rate,
                            dsLabour_Cost = 0,

                            dsDP_Rate = v_ListceCostPlanning[i].cpDP_Rate,
                            dsDP_Cost = 0,

                            dsME_Rate = v_ListceCostPlanning[i].cpME_Rate,
                            dsME_Cost = 0,

                            dsTotalCost = 0,
                        });
                        //dsDocumentNo = "";

                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
                //@class._ListceDetailSubMakerRequest = new List<ViewceDetailSubMakerRequest>();
                @class._ListGroupViewceDetailSubMakerRequest = new List<GroupViewceDetailSubMakerRequest>();
                @class._ListGroupViewceDetailSubMakerRequest = _ListceDetailSubMakerRequest.GroupBy(p => p.dsGroupName).Select(g => new GroupViewceDetailSubMakerRequest
                {
                    GroupName = g.Key.Trim(),
                    DetailSubMakerRequest = g.ToList()
                }
                ).ToList();

                //@class._GroupViewceDetailSubMakerRequest = @class._ListceDetailSubMakerRequest.GroupBy(p => p.cpGroupName).ToList();



                // @class._ListceDetailSubMakerRequest = _listceDetailSubMakerRequest;


            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }



            return PartialView("_PartialNewRequestProcess", @class);
        }


        public ActionResult Search(string term)
        {
            {
                return Json(_IT.rpEmails.Where(p => p.emName_M365.Contains(term)).Select(p => p.emEmail_M365 + "|" + p.emName_M365).ToList());
                //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());
            }
        }

        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            string v_status = "";

            int v_step = @classs._ViewceMastSubMakerRequest != null ? @classs._ViewceMastSubMakerRequest.smStep : 0;
            string v_issue = @classs._ViewceMastSubMakerRequest != null ? @classs._ViewceMastSubMakerRequest.smEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastSubMakerRequest != null ? @classs._ViewceMastSubMakerRequest.smDocumentNo : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            partialUrl = Url.Action("SendMail", "New", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });
            try
            {
                if (@classs._ViewceMastSubMakerRequest != null)
                {
                    if (@classs._ViewceMastSubMakerRequest.smDocumentNo != "" && @classs._ViewceMastSubMakerRequest.smDocumentNo != null)
                    {
                        // htCostPlanningNo
                        String htDocNo = @classs._ViewceMastSubMakerRequest.smDocumentNo.ToString(); //htCostPlanningNo
                        //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htStep).ThenBy(x=>x.htDate).ThenBy(x=>x.htTime).ToList();
                        _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == htDocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
                        if (_listHistory.Count() > 0)
                        {
                            for (int j = 0; j < _listHistory.Count(); j++)
                            {
                                var v_htcc = _listHistory[j].htCC;
                                string v_CCemail = "";
                                if (v_htcc != null)
                                {
                                    ViewrpEmail fromEmailCC = new ViewrpEmail();
                                    string[] splitCC = v_htcc.Split(',');
                                    foreach (var i in splitCC)
                                    {
                                        if (i != " " & i != "")
                                        {
                                            var v_cc = "";
                                            try
                                            {
                                                fromEmailCC = _IT.rpEmails.Where(w => w.emEmpcode == i.Trim()).FirstOrDefault();
                                                v_CCemail += fromEmailCC.emName_M365.ToString() + ",";
                                            }
                                            catch (Exception e)
                                            {
                                                v_cc = e.Message;
                                            }
                                        }
                                    }
                                }

                                _listHistory[j].htCC = v_CCemail;


                            }

                        }


                        return Json(new { status = "hasHistory", listHistory = _listHistory, partial = partialUrl });
                    }
                }

            }
            catch (Exception e)
            {

            }

            //return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
            return Json(new { status = "empty", listHistory = _listHistory, partial = partialUrl });
        }

        //[HttpPost]
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;

            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365


            //get emp operator
            var v_empstep = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "1") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "1").Select(x => x.mfTo).FirstOrDefault() : "";
            if (v_empstep != null) //step 2-5
            {
                var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empstep).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;
            }

            //step 6 Waiting Apporve By DM For ST Department ==> issue
            if (s_step == 6)
            {
                //var vdocNo = @class.
                var v_emailTo = _IT.rpEmails.Where(x => x.emEmpcode == s_issue).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                @class._ViewceHistoryApproved.htTo = v_emailTo;


                string tbHistory2 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 2).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory2EMPCODE = tbHistory2 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory2)).Select(x => x.emEmpcode).FirstOrDefault();

                string tbHistory3 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 3).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory3EMPCODE = tbHistory3 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory3)).Select(x => x.emEmpcode).FirstOrDefault();

                //add operator
                string tbHistory4 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory4EMPCODE = tbHistory4 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory4)).Select(x => x.emEmpcode).FirstOrDefault();

                string tbHistory5 = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault();
                string tbHistory5EMPCODE = tbHistory5 == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistory5)).Select(x => x.emEmpcode).FirstOrDefault();


                @class._ViewceHistoryApproved.htCC = tbHistory2 + "," + tbHistory3 + "," + tbHistory4 + "," + tbHistory5 + ",";


            }


            //step 4 fix ST dept check

            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            ViewBag.step = s_step;
            return PartialView("SendMail", @class);
        }

        public JsonResult SaveDraft(Class @class, List<IFormFile> files)
        {
            string config = "S";
            string msg = "";

            string[] chkPermis;
            string[] chkSave;

            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;

            try
            {
                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }

                i_Step = @class._ViewceMastSubMakerRequest != null ? @class._ViewceMastSubMakerRequest.smStep : 0;


                //vStatus = savefile(@class, files);
                vRunDoc = RunDocNo(@class);
                chkSave = Save(@class, i_Step, vRunDoc[0], vRunDoc[1], files);
                if (chkSave[0] == "E")
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                else
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                }


            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }
            return Json(new { c1 = config, c2 = msg });
        }

        [HttpPost]
        public JsonResult SaveForm_SendMail(Class @class, List<IFormFile> files)
        {

            string config = "S";
            string msg = "Send Mail & Save File Already";

            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;




            try
            {
                //chk permission
                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }



                i_Step = @class._ViewceMastSubMakerRequest != null ? @class._ViewceMastSubMakerRequest.smStep : 0;

                //check history
                if (@class._ViewceHistoryApproved.htTo != null || (@class._ViewceHistoryApproved.htTo == null && @class._ViewceHistoryApproved.htStatus == "Disapprove"))
                {
                    if (@class._ViewceHistoryApproved.htStatus == "Approve")
                    {
                        i_Step = i_Step + 1;
                        config = "S";

                        ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();
                        if (fromEmailTO == null)
                        {
                            config = "E";
                            msg = "Please Check your Email to , Email incorrect !!!";
                        }

                    }
                    else if (@class._ViewceHistoryApproved.htStatus == "Disapprove")
                    {
                        i_Step = 8;
                        config = "S";
                        string v_empissue = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == @class._ViewceMastSubMakerRequest.smDocumentNo).Select(x => x.smEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
                        @class._ViewceHistoryApproved.htTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empissue).Select(x => x.emName_M365).First();
                    }
                    else
                    {
                        config = "E";
                        msg = "Please input Status";
                    }
                }
                else
                {
                    config = "E";
                    msg = "Please input e-mail.";
                }




                if (config == "S")
                {
                    //check dm cs , DM Step 0-3  , ST Step 4-7
                    if (i_Step < 3)
                    {
                        //check emp positon
                        try
                        {
                            string v_POS_HCM_CODE = "";
                            string v_empcsup = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).FirstOrDefault();

                            if (i_Step == 1) //GL up
                            {
                                v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "TL").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                                msg = "Please send approval to GL Up of Dept.!!!";
                            }
                            else if (i_Step == 2) //DM up
                            {
                                v_POS_HCM_CODE = _HRMS.AccPOSMAST.Where(x => x.POS_CODE == "DDM").Select(x => x.POS_HCM_CODE).FirstOrDefault();
                                msg = "Please send approval to DM Up of Dept.!!!";
                            }

                            ViewAccEMPLOYEE _ViewAccEMPLOYEE = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == v_empcsup).FirstOrDefault();
                            List<ViewAccPOSMAST> _ViewAccPOSMAST = _HRMS.AccPOSMAST.Where(x => int.Parse(x.POS_HCM_CODE) <= int.Parse(v_POS_HCM_CODE)).ToList();
                            string v_chk = _ViewAccPOSMAST.Where(x => x.POS_CODE == _ViewAccEMPLOYEE.POS_CODE).Select(x => x.POS_CODE).FirstOrDefault();

                            if (v_chk == null || v_chk == "")
                            {
                                config = "E";
                                // msg = msg;
                                return Json(new { c1 = config, c2 = msg });
                            }
                        }
                        catch (Exception ex)
                        {
                            config = "E";
                            msg = "Please check email send to !!!!";
                            return Json(new { c1 = config, c2 = msg });
                        }
                    }

                    //    //save 


                    //RunDoc()
                    //vRunDoc[0] = status  vRunDoc[1]= run doc number
                    // status New
                    // status Old
                    // status Update RunDocNo
                    vRunDoc = RunDocNo(@class);
                    //vRunDoc = RunDoc(@class);
                    //vRunDocNo = RunDocNo(@class);

                    if (vRunDoc[0] == "Fail")
                    {
                        config = "E";
                        msg = "Error Run Doc No : " + vRunDoc[1];
                        return Json(new { c1 = config, c2 = msg });
                    }


                    //sRunDoc = vRunDoc[1]; save , savehistory
                    chkSave = Save(@class, i_Step, vRunDoc[0], vRunDoc[1], files);
                    if (chkSave[0] == "E")
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSave[0];
                        msg = chkSave[1];
                    }

                    //saveHistory
                    chkSaveHistory = saveHistory(@class, i_Step, vRunDoc[0], vRunDoc[1]);
                    if (chkSaveHistory[0] == "E")
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveHistory[0];
                        msg = chkSaveHistory[1];
                    }




                    //vStatus = savefile(@class, files);

                    return Json(new { c1 = config, c2 = msg });
                }
                else
                {
                    config = "E";
                    msg = msg;
                    return Json(new { c1 = config, c2 = msg });
                }
            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }
        }
        public string[] saveHistory(Class @class, int vstep, string status, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            string vEmpCodeCCemail = "";
            string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string vRevision = @class._ViewceMastSubMakerRequest.smRevision != null && @class._ViewceMastSubMakerRequest.smRevision != "" ? @class._ViewceMastSubMakerRequest.smRevision : "00"; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

            //string Empcode_IssueBy = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
            string v_EmpCodeRequest = @class._ViewceMastSubMakerRequest.smEmpCodeRequest == null || @class._ViewceMastSubMakerRequest.smEmpCodeRequest == ""
                                                                                ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                : @class._ViewceMastSubMakerRequest.smEmpCodeRequest + " : " + @class._ViewceMastSubMakerRequest.smNameRequest;
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "1").Select(x => x.mfSubject).First();
                    vstep = vstep == 8 ? vstep = 0 : vstep;

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    vEmpCodeCCemail += fromEmailCC.emEmpcode.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }


                    ViewceHistoryApproved _ViewceHistoryApproved = new ViewceHistoryApproved();
                    _ViewceHistoryApproved.htDocNo = RunDoc;// getSrNo[0].ToString();
                    _ViewceHistoryApproved.htStep = vstep;
                    _ViewceHistoryApproved.htStatus = @class._ViewceHistoryApproved.htStatus;
                    _ViewceHistoryApproved.htFrom = @class._ViewceHistoryApproved.htFrom;
                    _ViewceHistoryApproved.htTo = @class._ViewceHistoryApproved.htTo;
                    _ViewceHistoryApproved.htCC = vEmpCodeCCemail;//@class._ViewceHistoryApproved.htCC;
                    _ViewceHistoryApproved.htDate = DateTime.Now.ToString("yyyy/MM/dd");
                    _ViewceHistoryApproved.htTime = DateTime.Now.ToString("HH:mm:ss");
                    _ViewceHistoryApproved.htRemark = @class._ViewceHistoryApproved.htRemark;
                    _MK._ViewceHistoryApproved.AddAsync(_ViewceHistoryApproved);

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    var email = new MimeMessage();
                    ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                    ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                    MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                    MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                    email.Subject = "CostEstimate Sub Maker Request==> " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
                    //email.From.Add(MailboxAddress.Parse(_ViewlrHistoryApprove.htFrom));
                    email.From.Add(FromMailFrom);
                    email.To.Add(FromMailTO);

                    if (@class._ViewceHistoryApproved.htCC != null)
                    {
                        ViewrpEmail fromEmailCC = new ViewrpEmail();
                        string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                        foreach (var i in splitCC)
                        {
                            if (i != " " & i != "")
                            {
                                var v_cc = "";
                                try
                                {
                                    fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                    MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                    email.Cc.Add(FromMailcc);
                                    vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";
                                }
                                catch (Exception e)
                                {
                                    v_cc = e.Message;
                                }
                            }
                        }
                    }
                    var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=SubMarker";// + getSrNo[0].ToString();
                    var bodyBuilder = new BodyBuilder();
                    //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                    string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                    string EmailBody = $"<div>" +
                        $"<B>Cost Estimate : Sub Maker </B> <br>" +
                        $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                        $"<B>Order No : </B> " + @class._ViewceMastSubMakerRequest.smOrderNo + "<br>" +
                        $"<B>Lot No : </B> " + @class._ViewceMastSubMakerRequest.smLotNo + "<br>" +
                        $"<B>Revision No : </B> " + vRevision + "<br>" +
                        $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                        // $"<B>Request By : </B> " + @class._ViewceMastSubMakerRequest.smEmpCodeRequest + " : " + @class._ViewceMastSubMakerRequest.smNameRequest + "<br>" +
                        //$"<B>Subject : </B>" + _smStatus + "<br>" +
                        $"<B>Status : </B> " + _smStatus + "<br> " +
                        $"<B> หมายเหตุ : </B> " + @class._ViewceHistoryApproved.htRemark + "<br> " +
                        $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                        //$"<img src = 'http://thsweb/MVCPublish/LR_Service_Request/images/btn/mail1.png' alt = 'HTML tutorial' style = 'width: 42px; height: 42px;'>" +
                        $"</a>" +
                        $"</div>";

                    // bodyBuilder.Attachments.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\dev_rfc.log");

                    bodyBuilder.HtmlBody = string.Format(EmailBody);
                    email.Body = bodyBuilder.ToMessageBody();

                    // send email
                    //var smtp1 = new SmtpClient();
                    ////smtp.Connect("mail.csloxinfo.com");
                    //smtp1.Connect("203.146.237.138");
                    ////smtp1.Connect("150.109.165.119"); //change smtp.csloxinfo.com
                    ////smtp1.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                    ////// connect แบบ SSL/TLS
                    ////smtp1.Connect("150.109.165.119", 465, SecureSocketOptions.SslOnConnect);


                    //smtp1.Send(email);
                    //smtp1.Disconnect(true);

                    var senderEmail = new MailAddress(fromEmailFrom.emEmail_M365, fromEmailFrom.emName_M365);
                    var receiverEmail = new MailAddress(fromEmailTO.emEmail_M365, fromEmailTO.emName_M365);


                    System.Net.Mime.ContentType mimeTypeS = new System.Net.Mime.ContentType("text/html");
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(EmailBody, mimeTypeS);
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.csloxinfo.com");
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                    using (MailMessage mess = new MailMessage(senderEmail, receiverEmail))
                    {
                        mess.Subject = "CostEstimate Sub Maker Request==> " + _smStatus;
                        //add CC
                        if (@class._ViewceHistoryApproved.htCC != null)
                        {
                            ViewrpEmail fromEmailCC = new ViewrpEmail();
                            string[] splitCC = @class._ViewceHistoryApproved.htCC.Split(',');
                            foreach (var i in splitCC)
                            {
                                if (i != " " & i != "")
                                {
                                    var v_cc = "";
                                    try
                                    {
                                        fromEmailCC = _IT.rpEmails.Where(w => w.emName_M365 == i).FirstOrDefault();
                                        //MailboxAddress FromMailcc = new MailboxAddress(fromEmailCC.emName_M365, fromEmailCC.emEmail_M365);
                                        //email.Cc.Add(FromMailcc);
                                        //vCCemail += fromEmailCC.emEmail_M365.ToString() + ",";

                                        mess.CC.Add(fromEmailCC.emEmail_M365);
                                    }
                                    catch (Exception e)
                                    {
                                        v_cc = e.Message;
                                    }
                                }
                            }
                        }

                        mess.AlternateViews.Add(alternate);
                        smtp.Send(mess);
                    }






                    v_status = "S";
                    v_msg = "Save fle & send Mail Already";
                }
                catch (Exception ex)
                {
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }

                    //dbContextTransaction.Rollback();
                    v_status = "E";
                    v_msg = "Error Save History: " + ex.Message;
                }
            }


            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }
        public string[] Save(Class @class, int vstep, string status, string RunDoc, List<IFormFile> files)
        {
            string empissue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string UpdateBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            //User.Claims.FirstOrDefault(s => s.Type == "NICKNAME")?.Value;
            string v_msg = "";
            string v_status = "";

            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string vDocCode = "CE";
                    string vDocSub = "S";
                    string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
                    string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);
                    string vRevision = @class._ViewceMastSubMakerRequest.smRevision != null && @class._ViewceMastSubMakerRequest.smRevision != "" ? @class._ViewceMastSubMakerRequest.smRevision : "00"; //String.Format("{0:D3}", RunDoc.Substring(11, 3));

                    string empIssue = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).Select(x => x.emEmpcode).First() :
                                        @class._ViewceMastSubMakerRequest.smEmpCodeRequest == null ? empissue : @class._ViewceMastSubMakerRequest.smEmpCodeRequest;
                    string NickNameIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.NICKNAME).First();
                    string DeptIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empIssue).Select(x => x.DEPT_CODE).First();

                    string empApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastSubMakerRequest.smEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastSubMakerRequest.smNameApprove;


                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "1").Select(x => x.mfSubject).First();

                    //case dis approve
                    vstep = vstep == 8 ? vstep = 0 : vstep;

                    double s_dsWT_Man = 0;
                    double s_dsWT_Auto = 0;
                    double smTotalProcessWT = 0; // sum dsWT_Man + sum   dsWT_Auto
                    double smTotalCost = 0; // sum dsLabour_Cost +  sum dsDP_Cost + sum dsME_Cost

                    // status New  // status Update
                    if (status == "New")
                    {
                        //save run doc //CE-S-25-03-001 10,3


                        ViewceRunDocument _ViewceRunDocument = new ViewceRunDocument();
                        _ViewceRunDocument.rmRunNo = int.Parse(RunDoc.Substring(11, 3));
                        _ViewceRunDocument.rmDocCode = vDocCode;
                        _ViewceRunDocument.rmDocSub = vDocSub;
                        _ViewceRunDocument.rmYear = vYY;
                        _ViewceRunDocument.rmMonth = vMM;
                        _ViewceRunDocument.rmIssueBy = IssueBy;
                        _ViewceRunDocument.rmIssueBy = UpdateBy;
                        _MK._ViewceRunDocument.AddAsync(_ViewceRunDocument);





                        //table ceDetailSubMakerRequest

                        if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        {
                            for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                            {
                                var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                                {
                                    dsDocumentNo = RunDoc,
                                    dsRunNo = i + 1,
                                    dsLotNo = @class._ViewceMastSubMakerRequest.smLotNo,
                                    dsOrderNo = @class._ViewceMastSubMakerRequest.smOrderNo,
                                    dsRevision = vRevision,
                                    dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                                    dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                                    dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                                    dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                                    dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                                    dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                                    dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                                    dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                                    dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                                    dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                                    //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                                    dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                                    dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                                };
                                //s_dsWT_Man += _ViewceDetailSubMakerRequest.dsWT_Man;
                                //s_dsWT_Auto += _ViewceDetailSubMakerRequest.dsWT_Auto;
                                _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                            }

                        }


                        ViewceMastSubMakerRequest _ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                        _ViewceMastSubMakerRequest.smDocumentNo = RunDoc;
                        _ViewceMastSubMakerRequest.smLotNo = @class._ViewceMastSubMakerRequest.smLotNo; // public string 
                        _ViewceMastSubMakerRequest.smOrderNo = @class._ViewceMastSubMakerRequest.smOrderNo; //  public string 
                        _ViewceMastSubMakerRequest.smRevision = vRevision;//@class.int.Parse(RunDoc.Substring(10, 3).smCustomerName; // public string 
                        _ViewceMastSubMakerRequest.smMoldName = @class._ViewceMastSubMakerRequest.smMoldName; // public string 
                        _ViewceMastSubMakerRequest.smModelName = @class._ViewceMastSubMakerRequest.smModelName; //public string 
                        _ViewceMastSubMakerRequest.smCustomerName = @class._ViewceMastSubMakerRequest.smCustomerName; //public string 
                        _ViewceMastSubMakerRequest.smCavityNo = @class._ViewceMastSubMakerRequest.smCavityNo; //public int    
                        _ViewceMastSubMakerRequest.smFunction = @class._ViewceMastSubMakerRequest.smFunction; //public string 
                        _ViewceMastSubMakerRequest.smDevelopmentStage = @class._ViewceMastSubMakerRequest.smDevelopmentStage; //public string 
                        _ViewceMastSubMakerRequest.smMoldNo = @class._ViewceMastSubMakerRequest.smMoldNo; // public string 
                        _ViewceMastSubMakerRequest.smMatOutDate = @class._ViewceMastSubMakerRequest.smMatOutDate; // public string 
                        _ViewceMastSubMakerRequest.smMatOutTime = @class._ViewceMastSubMakerRequest.smMatOutTime; //public string 
                        _ViewceMastSubMakerRequest.smDeliveryDate = @class._ViewceMastSubMakerRequest.smDeliveryDate; // public string 
                        _ViewceMastSubMakerRequest.smDeliveryTime = @class._ViewceMastSubMakerRequest.smDeliveryTime; // public string 
                        _ViewceMastSubMakerRequest.smRemark = @class._ViewceMastSubMakerRequest.smRemark; //public string 
                        _ViewceMastSubMakerRequest.smRemarkOther = @class._ViewceMastSubMakerRequest.smRemarkOther; // public string 
                        _ViewceMastSubMakerRequest.smDetail = @class._ViewceMastSubMakerRequest.smDetail; //public string 
                        _ViewceMastSubMakerRequest.smWeight = @class._ViewceMastSubMakerRequest.smWeight; // public string 
                        _ViewceMastSubMakerRequest.smQty = @class._ViewceMastSubMakerRequest.smQty; //public int    
                        _ViewceMastSubMakerRequest.smTotalProcessWT = @class._ViewceMastSubMakerRequest.smTotalProcessWT;//s_dsWT_Man + s_dsWT_Auto;  //@class._ViewceMastSubMakerRequest.smTotalProcessWT; // public double 
                        _ViewceMastSubMakerRequest.smTotalProcessCost = @class._ViewceMastSubMakerRequest.smTotalProcessCost; //public double  
                        _ViewceMastSubMakerRequest.smOrderMatl = @class._ViewceMastSubMakerRequest.smOrderMatl; //public double 
                        _ViewceMastSubMakerRequest.smTotalCost = @class._ViewceMastSubMakerRequest.smTotalCost; //public double 
                        _ViewceMastSubMakerRequest.smRoundUp = @class._ViewceMastSubMakerRequest.smRoundUp; //public double 
                        _ViewceMastSubMakerRequest.smIssueDate = @class._ViewceMastSubMakerRequest.smIssueDate;// @class._ViewceMastSubMakerRequest.smRevision; //public string 
                        _ViewceMastSubMakerRequest.smIssueDept = DeptIssue; // public string 
                        _ViewceMastSubMakerRequest.smEmpCodeRequest = @class._ViewceMastSubMakerRequest.smEmpCodeRequest != null && @class._ViewceMastSubMakerRequest.smEmpCodeRequest != "" ? @class._ViewceMastSubMakerRequest.smEmpCodeRequest : empIssue; // public string 
                        _ViewceMastSubMakerRequest.smNameRequest = @class._ViewceMastSubMakerRequest.smEmpCodeRequest != null && @class._ViewceMastSubMakerRequest.smEmpCodeRequest != "" ? @class._ViewceMastSubMakerRequest.smNameRequest : NickNameIssue; ; //  public string 
                        _ViewceMastSubMakerRequest.smEmpCodeApprove = empApprove; //public string 
                        _ViewceMastSubMakerRequest.smNameApprove = NickNameApprove; // public string
                        _ViewceMastSubMakerRequest.smFlowNo = 1; // @class._ViewceMastSubMakerRequest.smFlowNo; // public int 
                        _ViewceMastSubMakerRequest.smStep = vstep; // public int 
                        _ViewceMastSubMakerRequest.smStatus = _smStatus;//@class._ViewceMastSubMakerRequest.smStatus; //public int
                        _ViewceMastSubMakerRequest.smTotalProCost = @class._ViewceMastSubMakerRequest.smTotalProCost; // public double 
                        _ViewceMastSubMakerRequest.smIcsName = @class._ViewceMastSubMakerRequest.smIcsName; //30/06/2025
                        _ViewceMastSubMakerRequest.smTypeCavity = @class._ViewceMastSubMakerRequest.smTypeCavity; // public string 
                        _ViewceMastSubMakerRequest.smReqStatus = @class._ViewceMastSubMakerRequest.smReqStatus; // public string 
                        _MK._ViewceMastSubMakerRequest.AddAsync(_ViewceMastSubMakerRequest);


                        _MK.SaveChanges();
                    }

                    // status Old
                    else if (status == "Update")
                    {



                        if (@class._ListceDetailSubMakerRequest.Count() > 0)
                        {
                            var products = _MK._ViewceDetailSubMakerRequest.Where(p => p.dsDocumentNo == RunDoc).ToList();
                            _MK._ViewceDetailSubMakerRequest.RemoveRange(products);
                            //_MK.SaveChangesAsync();
                            _MK.SaveChanges();
                            for (int i = 0; i < @class._ListceDetailSubMakerRequest.Count(); i++)
                            {
                                var _ViewceDetailSubMakerRequest = new ViewceDetailSubMakerRequest()
                                {
                                    dsDocumentNo = RunDoc,
                                    dsRunNo = i + 1,
                                    dsLotNo = @class._ViewceMastSubMakerRequest.smLotNo,
                                    dsOrderNo = @class._ViewceMastSubMakerRequest.smOrderNo,
                                    dsRevision = vRevision,
                                    dsGroupName = @class._ListceDetailSubMakerRequest[i].dsGroupName,
                                    dsProcessName = @class._ListceDetailSubMakerRequest[i].dsProcessName,
                                    dsWT_Man = @class._ListceDetailSubMakerRequest[i].dsWT_Man,
                                    dsWT_Auto = @class._ListceDetailSubMakerRequest[i].dsWT_Auto,
                                    dsLabour_Rate = @class._ListceDetailSubMakerRequest[i].dsLabour_Rate,
                                    dsLabour_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsLabour_Rate / 1000, 2),//   @class._ListceDetailSubMakerRequest[i].dsLabour_Cost,
                                    dsDP_Rate = @class._ListceDetailSubMakerRequest[i].dsDP_Rate,
                                    dsDP_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsDP_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsDP_Cost,
                                    dsEnable_WTAuto = @class._ListceDetailSubMakerRequest[i].dsEnable_WTAuto,
                                    dsEnable_WTMan = @class._ListceDetailSubMakerRequest[i].dsEnable_WTMan,
                                    //_ViewceDetailSubMakerRequest.dsDP_Cost = @class._ListceDetailSubMakerRequest[i].dsDP_Cost;
                                    dsME_Rate = @class._ListceDetailSubMakerRequest[i].dsME_Rate,
                                    dsME_Cost = Math.Round(@class._ListceDetailSubMakerRequest[i].dsWT_Man * @class._ListceDetailSubMakerRequest[i].dsME_Rate / 1000, 2),// @class._ListceDetailSubMakerRequest[i].dsME_Cost,
                                };
                                //s_dsWT_Man += _ViewceDetailSubMakerRequest.dsWT_Man;
                                //s_dsWT_Auto += _ViewceDetailSubMakerRequest.dsWT_Auto;
                                _MK._ViewceDetailSubMakerRequest.AddAsync(_ViewceDetailSubMakerRequest);
                            }
                            // _MK.SaveChanges();
                        }
                        ViewceMastSubMakerRequest _ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == @class._ViewceMastSubMakerRequest.smDocumentNo).FirstOrDefault();
                        if (_ViewceMastSubMakerRequest != null)
                        {
                            //_ViewceMastSubMakerRequest.smDocumentNo = RunDoc;
                            _ViewceMastSubMakerRequest.smLotNo = @class._ViewceMastSubMakerRequest.smLotNo; // public string 
                            _ViewceMastSubMakerRequest.smOrderNo = @class._ViewceMastSubMakerRequest.smOrderNo; //  public string 
                            _ViewceMastSubMakerRequest.smRevision = vRevision;//@class.int.Parse(RunDoc.Substring(10, 3).smCustomerName; // public string 
                            _ViewceMastSubMakerRequest.smMoldName = @class._ViewceMastSubMakerRequest.smMoldName; // public string 
                            _ViewceMastSubMakerRequest.smModelName = @class._ViewceMastSubMakerRequest.smModelName; //public string 
                            _ViewceMastSubMakerRequest.smCustomerName = @class._ViewceMastSubMakerRequest.smCustomerName; //public string 
                            _ViewceMastSubMakerRequest.smCavityNo = @class._ViewceMastSubMakerRequest.smCavityNo; //public int    
                            _ViewceMastSubMakerRequest.smFunction = @class._ViewceMastSubMakerRequest.smFunction; //public string 
                            _ViewceMastSubMakerRequest.smDevelopmentStage = @class._ViewceMastSubMakerRequest.smDevelopmentStage; //public string 
                            _ViewceMastSubMakerRequest.smMoldNo = @class._ViewceMastSubMakerRequest.smMoldNo; // public string 
                            _ViewceMastSubMakerRequest.smMatOutDate = @class._ViewceMastSubMakerRequest.smMatOutDate; // public string 
                            _ViewceMastSubMakerRequest.smMatOutTime = @class._ViewceMastSubMakerRequest.smMatOutTime; //public string 
                            _ViewceMastSubMakerRequest.smDeliveryDate = @class._ViewceMastSubMakerRequest.smDeliveryDate; // public string 
                            _ViewceMastSubMakerRequest.smDeliveryTime = @class._ViewceMastSubMakerRequest.smDeliveryTime; // public string 
                            _ViewceMastSubMakerRequest.smRemark = @class._ViewceMastSubMakerRequest.smRemark; //public string 
                            _ViewceMastSubMakerRequest.smRemarkOther = @class._ViewceMastSubMakerRequest.smRemarkOther; // public string 
                            _ViewceMastSubMakerRequest.smDetail = @class._ViewceMastSubMakerRequest.smDetail; //public string 
                            _ViewceMastSubMakerRequest.smWeight = @class._ViewceMastSubMakerRequest.smWeight; // public string 
                            _ViewceMastSubMakerRequest.smQty = @class._ViewceMastSubMakerRequest.smQty; //public int    
                            _ViewceMastSubMakerRequest.smTotalProcessWT = @class._ViewceMastSubMakerRequest.smTotalProcessWT;//s_dsWT_Man + s_dsWT_Auto;  //@class._ViewceMastSubMakerRequest.smTotalProcessWT; // public double 
                            _ViewceMastSubMakerRequest.smTotalProcessCost = @class._ViewceMastSubMakerRequest.smTotalProcessCost; //public double  
                            _ViewceMastSubMakerRequest.smOrderMatl = @class._ViewceMastSubMakerRequest.smOrderMatl; //public double 
                            _ViewceMastSubMakerRequest.smTotalCost = @class._ViewceMastSubMakerRequest.smTotalCost; //public double 
                            _ViewceMastSubMakerRequest.smRoundUp = @class._ViewceMastSubMakerRequest.smRoundUp; //public double 
                            _ViewceMastSubMakerRequest.smIssueDate = @class._ViewceMastSubMakerRequest.smIssueDate;// @class._ViewceMastSubMakerRequest.smRevision; //public string 
                            _ViewceMastSubMakerRequest.smIssueDept = DeptIssue; // public string 
                            _ViewceMastSubMakerRequest.smEmpCodeRequest = @class._ViewceMastSubMakerRequest.smEmpCodeRequest != null && @class._ViewceMastSubMakerRequest.smEmpCodeRequest != "" ? @class._ViewceMastSubMakerRequest.smEmpCodeRequest : empIssue; // public string 
                            _ViewceMastSubMakerRequest.smNameRequest = @class._ViewceMastSubMakerRequest.smEmpCodeRequest != null && @class._ViewceMastSubMakerRequest.smEmpCodeRequest != "" ? @class._ViewceMastSubMakerRequest.smNameRequest : NickNameIssue; ; //  public string 
                            _ViewceMastSubMakerRequest.smEmpCodeApprove = empApprove; //public string 
                            _ViewceMastSubMakerRequest.smNameApprove = NickNameApprove; // public string
                            _ViewceMastSubMakerRequest.smFlowNo = 1; // @class._ViewceMastSubMakerRequest.smFlowNo; // public int 
                            _ViewceMastSubMakerRequest.smStep = vstep; // public int 
                            _ViewceMastSubMakerRequest.smStatus = _smStatus;//@class._ViewceMastSubMakerRequest.smStatus; //public int
                            _ViewceMastSubMakerRequest.smTotalProCost = @class._ViewceMastSubMakerRequest.smTotalProCost; // public double 

                            _ViewceMastSubMakerRequest.smTypeCavity = @class._ViewceMastSubMakerRequest.smTypeCavity; // public string 
                            _ViewceMastSubMakerRequest.smIcsName = @class._ViewceMastSubMakerRequest.smIcsName; //30/06/2025
                            _ViewceMastSubMakerRequest.smReqStatus = @class._ViewceMastSubMakerRequest.smReqStatus; // public string 
                            //_ViewceMastSubMakerRequest.smEmpCodeApprove = empApprove;
                            //_ViewceMastSubMakerRequest.smNameApprove = NickNameApprove;
                            //_ViewceMastSubMakerRequest.smStep = vstep;
                            //_ViewceMastSubMakerRequest.smStatus = _smStatus;
                            //_ViewceMastSubMakerRequest.smTotalProcessWT = @class._ViewceMastSubMakerRequest.smTotalProcessWT; ;//s_dsWT_Man + s_dsWT_Auto;
                            _MK._ViewceMastSubMakerRequest.Update(_ViewceMastSubMakerRequest);
                            // _MK.SaveChanges();
                        }
                        _MK.SaveChanges();


                        //new check 01 disible 00 old version  Order No.​ lot no
                        if (vstep == 7 && vRevision != "00")
                        {
                            var _RViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smOrderNo == @class._ViewceMastSubMakerRequest.smOrderNo && x.smLotNo == @class._ViewceMastSubMakerRequest.smLotNo && x.smRevision != vRevision).ToList();
                            if (_RViewceMastSubMakerRequest.Count > 0)
                            {
                                foreach (var item in _RViewceMastSubMakerRequest)
                                {
                                    item.smReqStatus = false;
                                }
                                _MK.SaveChanges();
                            }
                        }


                    }


                    //v_status = "S";
                    //v_msg = "Save file & send Mail Already";

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();

                    string[] v_statusFile = savefile(@class, files, RunDoc);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {
                    // dbContextTransaction.Rollback();

                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }
                    v_status = "E";
                    // v_msg = "Error" + ex.InnerException?.InnerException?.Message;
                    v_msg = "Error Save: " + ex.InnerException.Message;
                    //UnderlyingTransaction




                }
            }

            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }
        public string[] chkPermission(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            var chkData = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == @class._ViewceMastSubMakerRequest.smDocumentNo).FirstOrDefault();
            try
            {



                if (chkData != null)
                {
                    //check operator //check create user
                    if (chkData.smStep == 0 && _UserId == chkData.smEmpCodeRequest)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else if (_UserId == chkData.smEmpCodeApprove)
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else if (chkData.smStep == 7 && _Permiss.ToUpper() == "ADMIN")
                    {
                        status_per = "S";
                        message_per = "You have permission ";
                    }
                    else
                    {
                        status_per = "P";
                        message_per = "You don't have permission to access";
                    }
                }
                else
                {
                    status_per = "S";
                    message_per = "You have permission ";
                }

                string[] returnvar = { status_per, message_per };
                return returnvar;
            }
            catch (Exception ex)
            {
                string[] returnvar = { status_per, message_per };
                return returnvar;

            }
        }
        public string[] RunDoc(Class @class)
        {

            string v_msg = "";
            string v_chkupdate = "";
            string v_msg1 = "";
            string v_rundoc = "";
            int i_Revision = 0;
            try
            {
                //check update revision or new

                var entries = _MK._ViewceMastSubMakerRequest.Where(x => x.smOrderNo == @class._ViewceMastSubMakerRequest.smOrderNo
                                                                   && x.smLotNo == @class._ViewceMastSubMakerRequest.smLotNo
                                                                   && x.smRevision == @class._ViewceMastSubMakerRequest.smRevision).FirstOrDefault();
                if (entries != null)
                {
                    //_MK.Entry(entries).State = EntityState.Detached; // ปลดล็อก dbProduct
                    //_MK.Entry(@class._ViewceMastSubMakerRequest).State = EntityState.Modified; // บอก EF ว่ามีการแก้ไข
                    var entryold = _MK.Entry(entries);
                    var entryNew = _MK.Entry(@class._ViewceMastSubMakerRequest);

                    foreach (var property in entryold.Properties)
                    {
                        var oldValue = entryNew.Property(property.Metadata.Name).CurrentValue;
                        var newValue = property.CurrentValue;

                        if (!object.Equals(oldValue, newValue))
                        {
                            v_msg1 += ($"Field: {property.Metadata.Name}");
                            v_msg1 += ($"Old Value: {oldValue}");
                            v_msg1 += ($"New Value: {newValue}");

                            //go insert

                            v_chkupdate = "Update";

                            v_msg = "Update";
                            v_rundoc = String.Format("{0:D2}", int.Parse(entries.smRevision + 1));
                            string[] vRevisionPass = { v_msg, v_rundoc };
                            return vRevisionPass;
                        }

                    }
                    if (v_chkupdate == "")
                    {
                        v_msg = "Old";
                        v_rundoc = entries.smRevision.ToString();
                    }

                }
                else
                {
                    v_msg = "New";
                    i_Revision = 0;
                    v_rundoc = String.Format("{0:D2}", i_Revision);
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
        public string[] RunDocNo(Class @class)
        {

            string v_msg = "";
            string v_rundoc = "";
            int i_rundoc = 0;

            string vIssue = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string vDocCode = "CE";
            string vDocSub = "S";
            string vYY = DateTime.Now.ToString("yyyyMM").Substring(2, 2);
            string vMM = DateTime.Now.ToString("yyyyMM").Substring(4, 2);

            try
            {
                //check update revision or new
                if (@class._ViewceMastSubMakerRequest.smDocumentNo != null && @class._ViewceMastSubMakerRequest.smDocumentNo != "")
                {

                    v_msg = "Update";
                    v_rundoc = @class._ViewceMastSubMakerRequest.smDocumentNo;
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
                        v_rundoc = "CE" + "-" + "S" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", i_rundoc + 1);
                    }
                    else
                    {
                        v_rundoc = "CE" + "-" + "S" + "-" + vYY + "-" + vMM + "-" + String.Format("{0:D3}", 1);
                    }

                    //v_rundoc = "CE" + "S" + "-" + vYY + "-" + vMM + String.Format("{0:D3}", i_rundoc);

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
        public string[] savefile(Class @class, List<IFormFile> file, string RunDoc)
        {
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            string fileName = "";
            string v_error = "";
            string v_fnType = "";// v_type != "" ? v_type : @class._ViewsvsServiceRequest.srType;

            string v_status = "";
            string v_msg = "";
            int v_count = 0;
            try
            {
                if (file is null)
                {
                    v_status = "S";
                    v_msg = "Save & Send Mail Already";
                }
                else
                {
                    List<ViewAttachment> listInsert = new List<ViewAttachment>();
                    using (var dbContextTransaction = _IT.Database.BeginTransaction())
                    {
                        try
                        {
                            if (file is null)
                            {
                                //vStatus = "File not found";
                                v_status = "S";
                                v_msg = "Save & Send Mail Already";
                            }
                            else
                            {
                                for (int i = 0; i < file.Count; i++)
                                {
                                    string IssueDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    fileName = IssueDate + "-" + file[i].FileName;//System.IO.Path.GetExtension(file.FileName).ToLower();
                                    string filePath = path + fileName;
                                    var fileLocation = new FileInfo(filePath);
                                    //filePaths.Add(filePath);
                                    if (!Directory.Exists(filePath))
                                    {
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file[i].CopyTo(stream);
                                        }
                                    }
                                    ViewAttachment _viewAttachment = new ViewAttachment();
                                    _viewAttachment.fnNo = RunDoc; // @class._ViewceMastSubMakerRequest.smRevision;//vSno.ToString();
                                    _viewAttachment.fnPath = filePath;
                                    _viewAttachment.fnFilename = fileName;
                                    _viewAttachment.fnIssueBy = IssueBy;
                                    _viewAttachment.fnUpdateBy = IssueBy;
                                    _viewAttachment.fnType = v_fnType;
                                    _viewAttachment.fnProgram = PgName; //Program  name CostEstimate
                                                                        //_IT.Attachment.AddAsync(_viewAttachment);
                                                                        //_IT.SaveChanges();
                                                                        //vStatus = fileName;
                                    listInsert.Add(_viewAttachment);
                                }

                            }
                            _IT.Attachment.AddRange(listInsert); // Add หลายรายการ
                            _IT.SaveChanges(); // Save ทีเดียว
                            dbContextTransaction.Commit();
                            v_status = "S";
                            v_msg = "Save & Send Mail Already";
                        }
                        catch (Exception e)
                        {
                            v_status = "E";
                            v_error = e.Message;
                            v_msg = "Error Save file :" + e.Message;
                            //dbContextTransaction.Rollback();
                            try
                            {
                                dbContextTransaction.Rollback();
                            }
                            catch
                            {
                                // ignore ถ้า transaction ปิดไปแล้ว
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                v_status = "E";
                v_error = e.Message;
                v_msg = "Error Save file :" + e.Message;

            }
            v_count = v_count;
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public ActionResult DeteleDataFile(string id, string vname)
        {
            try
            {
                //var find = _IT.Attachment(X => X.)

                ViewAttachment find = _IT.Attachment.Where(x => x.fnNo == id && x.fnFilename == vname && x.fnProgram == "CostEstimate").FirstOrDefault();
                var delete = _IT.Attachment.Remove(find);


                _IT.SaveChanges();
            }
            catch
            {
                return Json(new { res = "error" });

            }
            return Json(new { res = "success" });


            //return Json(_IT.rpEmails.Where(p => p.emEmail.Contains(term) || p.emEmail_M365.Contains(term)).Select(p => p.emEmail_M365).ToList());

        }



        [HttpPost]
        public PartialViewResult PrintQUOTATION(string mpNo, Class @class)
        {
            try
            {
                @class._ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                @class._ViewOperaterCP = new ViewOperaterCP();
                @class._ViewceMastFlowApprove = new ViewceMastFlowApprove();
                if (mpNo != null)
                {

                    string tbHistoryIssueName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryIssueEMPCODE = tbHistoryIssueName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryIssueName)).Select(x => x.emEmpcode).FirstOrDefault();

                    string tbHistoryCheckedGLName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 5).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedGLEMPCODE = tbHistoryCheckedGLName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedGLName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryCheckedName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryCheckedEMPCODE = tbHistoryCheckedName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryCheckedName)).Select(x => x.emEmpcode).FirstOrDefault();


                    string tbHistoryApproveByName = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault();
                    string tbHistoryApproveByEMPCODE = tbHistoryApproveByName == "" ? "" : _IT.rpEmails.Where(u => u.emName_M365.Contains(tbHistoryApproveByName)).Select(x => x.emEmpcode).FirstOrDefault();



                    //string tbHistoryIssue3 = _IT.rpEmails.Where(u => u.emName_M365.Contains(_MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault())).Select(x => x.emEmpcode).FirstOrDefault();
                    //string tbHistoryIssue = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //    _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //string tbHistoryChecked = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    //  _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 6).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();

                    //string tbHistoryApproveBy = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault() is null ? "" :
                    // _IT.rpEmails.Where(u => u.emEmail_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 7).Select(x => x.htFrom).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();


                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEIssue = new ViewAccEMPLOYEE();
                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEChecked = new ViewAccEMPLOYEE();
                    //ViewAccEMPLOYEE _ViewAccEMPLOYEEApproveBy = new ViewAccEMPLOYEE();

                    //_ViewAccEMPLOYEEIssue = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssue).FirstOrDefault();
                    //_ViewAccEMPLOYEEChecked = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryChecked).FirstOrDefault();
                    //_ViewAccEMPLOYEEApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveBy).FirstOrDefault();


                    //string tbFlowissue = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 2).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowCheck1 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 3).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowCheck2 = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 4).Select(z => z.mfTo).FirstOrDefault();
                    //string tbFlowApprove = _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault() is null ? "" : _MK._ViewceMastFlowApprove.Where(y => y.mfStep == 5).Select(z => z.mfTo).FirstOrDefault();


                    _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    string vIssueBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryIssueEMPCODE).Select(x => x.NICKNAME).FirstOrDefault();
                    string vCheckByTL = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedGLEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vCheckByTM = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryCheckedEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();
                    string vApproveBy = _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() is null ? "" : _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.NICKNAME).FirstOrDefault() + " , " + _HRMS.AccPOSMAST.Where(y => y.POS_CODE == _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == tbHistoryApproveByEMPCODE).Select(x => x.POS_CODE).FirstOrDefault()).Select(z => z.POS_NAME).FirstOrDefault();

                    @class._ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == mpNo).FirstOrDefault();



                    //revise issue data get 
                    string tbHistoryIssueDate = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).OrderByDescending(x => x.htNo).Select(x => x.htDate).FirstOrDefault() is null ? "" : _MK._ViewceHistoryApproved.Where(x => x.htDocNo == mpNo && x.htStep == 4).OrderByDescending(x => x.htNo).Select(x => x.htDate).FirstOrDefault();
                    if (tbHistoryIssueDate != "" && tbHistoryIssueDate != null)
                    {
                        DateTime dt = DateTime.ParseExact(tbHistoryIssueDate, "yyyy/MM/dd", null);
                        // แปลง DateTime กลับเป็นสตริงรูปแบบ dd/MM/yyyy
                        string output = dt.ToString("dd/MM/yyyy");
                        @class._ViewceMastSubMakerRequest.smIssueDate = output;
                    }


                    @class._ViewOperaterCP.IssueBy = vIssueBy;
                    @class._ViewOperaterCP.CheckedByTL = vCheckByTL;
                    @class._ViewOperaterCP.CheckedByTM = vCheckByTM;
                    @class._ViewOperaterCP.ApproveBy = vApproveBy;

                    @class._ViewOperaterCP.empIssueBy = tbHistoryIssueEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTL = tbHistoryCheckedGLEMPCODE;
                    @class._ViewOperaterCP.empCheckedByTM = tbHistoryCheckedEMPCODE;
                    @class._ViewOperaterCP.empApproveBy = tbHistoryApproveByEMPCODE;





                }


            }
            catch (Exception ex)
            {
                string a = "";
                a = ex.Message;
            }
            // @class._ListceCostPlanning = _ListViewceCostPlanning.;
            return PartialView("_PartialDisplayQuotation", @class);
            //return PartialView("_PartialDisplayQuotationTHSarabun", @class);

        }





        public JsonResult updateStatus(string id, string status)
        {
            string config = "S";
            string msg = "Update Success !!";
            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {

                try
                {
                    string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
                    if (_Permiss.ToUpper() != "ADMIN")
                    {
                        config = "P";
                        msg = "You don't have permission to access";
                        return Json(new { c1 = config, c2 = msg });
                    }


                    ViewceMastSubMakerRequest _ViewceMastSubMakerRequest = new ViewceMastSubMakerRequest();
                    _ViewceMastSubMakerRequest = _MK._ViewceMastSubMakerRequest.Where(x => x.smDocumentNo == id).FirstOrDefault();
                    _ViewceMastSubMakerRequest.smReqStatus = status != null ? status == "0" ? false : true : true;
                    _MK._ViewceMastSubMakerRequest.Update(_ViewceMastSubMakerRequest);
                    _MK.SaveChanges();
                    dbContextTransaction.Commit();
                    return Json(new { c1 = config, c2 = msg });
                }

                catch (Exception ex)
                {
                    try
                    {
                        dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // ignore ถ้า transaction ปิดไปแล้ว
                    }



                    //dbContextTransaction.Rollback();
                    config = "E";
                    msg = "Something is wrong !!!!! : " + ex.Message;
                    return Json(new { c1 = config, c2 = msg });
                }

            }
        }


    }


}
