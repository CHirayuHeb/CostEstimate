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

using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace CostEstimate.Controllers.NewMoldOtherMT
{
    public class NewMoldOtherMTController : Controller
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
        public NewMoldOtherMTController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
        public IActionResult Index(Class @class, string Docno)
        {
            @class._listAttachment = new List<ViewAttachment>();
            @class._ViewceMastModifyRequest = new ViewceMastModifyRequest();
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "5").ToList();

            @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
            @class._ViewceMastMaterialRequest = new ViewceMastMaterialRequest();
            @class._ListceMastModel = new List<ViewceMastModel>();
            @class._ListViewceItemMaterialRequestPartName = new List<ViewceItemMaterialRequestPartName>();

            if (Docno != null)
            {
                @class._ListceMastModel = _MK._ViewceMastModel.Where(x => x.mmType == "MoldOtherMaterial").OrderBy(x => x.mmNo).ToList();

                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == Docno).FirstOrDefault();
                @class._ViewceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == Docno).FirstOrDefault();

                @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == Docno).OrderBy(x => x.ipRunNo).ToList();
                for (int j = 0; j < @class._ListViewceItemPartName.Count(); j++)
                {
                    for (int i = 0; i < @class._ListceMastModel.Count(); i++)
                    {
                        var _ceMastMaterialRequest = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub
                                                                                          && x.mpNoProcess == @class._ListViewceItemPartName[j].ipRunNo
                                                                                          && x.mpItem.StartsWith(@class._ListceMastModel[i].mmModelName.ToString())
                                                                                          ).FirstOrDefault();
                        @class._ListViewceItemMaterialRequestPartName.Add(new ViewceItemMaterialRequestPartName
                        {
                            mpDocumentNoSub = @class._ViewceMastMaterialRequest.mrDocumentNoSub,
                            mpRunNo = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpRunNo : i + 1,
                            mpPartName = @class._ListViewceItemPartName[j].ipPartName,
                            mpCavityNo = @class._ListViewceItemPartName[j].ipCavityNo,
                            mpTypeCavity = @class._ListViewceItemPartName[j].ipTypeCavity,
                            mpNoProcess = @class._ListViewceItemPartName[j].ipRunNo,  // id master partname
                            mpNo = i + 1,
                            mpItem = @class._ListceMastModel[i].mmModelName,
                            mpPCS = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpPCS : 0,
                            mpAmount = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpAmount : 0,
                            mpTotal = _ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpTotal : 0,
                            mpIssueDate = @class._ViewceMastMoldOtherRequest != null ? @class._ViewceMastMoldOtherRequest.mrIssueDate : DateTime.Now.ToString("yyyy/MM/dd"),    //_ceMastMaterialRequest != null ? _ceMastMaterialRequest.mpIssueDate : DateTime.Now.ToString("yyyy/MM/dd"),
                        });
                    }
                }

                @class._ListGroupViewceItemMaterialRequestPartName = new List<GroupViewceItemMaterialRequestPartName>();
                @class._ListGroupViewceItemMaterialRequestPartName = @class._ListViewceItemMaterialRequestPartName.GroupBy(x => new { x.mpPartName, x.mpCavityNo, x.mpTypeCavity, x.mpNoProcess })
                                                                        .Select(g => new GroupViewceItemMaterialRequestPartName
                                                                        {
                                                                            mpPartName = g.Key.mpPartName,
                                                                            mpCavityNo = g.Key.mpCavityNo,
                                                                            mpTypeCavity = g.Key.mpTypeCavity,
                                                                            mpNoProcess = g.Key.mpNoProcess,
                                                                            ItemMaterialRequestPartName = g.ToList()
                                                                        }).ToList();

                @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == @class._ViewceMastMaterialRequest.mrDocumentNoSub).ToList();

            }

            return View(@class);
        }

        [Authorize("Checked")]
        [HttpPost]
        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrStep : 0;
            string v_issue = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastMaterialRequest != null ? @classs._ViewceMastMaterialRequest.mrDocumentNoSub : "";
            List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
            //_listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == v_DocNo).OrderBy(x => x.htStep).ToList();

            _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == v_DocNo).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
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



            partialUrl = Url.Action("SendMail", "NewMoldOtherMT", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });

            //return Json(new { status = "hasHistory", listHistory = _listHistory, partial = partialUrl });

            return Json(new { status = _listHistory.Count() > 0 ? "hasHistory" : "empty", listHistory = _listHistory, partial = partialUrl });
        }
        public ActionResult SendMail(Class @class, int s_step, string s_issue, string mpNo)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            @class._ViewceHistoryApproved = new ViewceHistoryApproved();
            var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365

            //To
            string v_empCodeTo = s_step == 3 ? _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == mpNo).Select(x => x.mrEmpCodeRequest).FirstOrDefault()
               : _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5") != null
                    ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5").Select(x => x.mfTo).FirstOrDefault()
                    : "";

            string v_listemailTo;

            v_empCodeTo = v_empCodeTo != null ? v_empCodeTo.Split(",").Count() > 0 ? v_empCodeTo.Split(",")[0] : v_empCodeTo : "";




            var v_nameTo = _IT.rpEmails.Where(x => x.emEmpcode == v_empCodeTo.Trim()).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
            string v_empCodeCC = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "5") != null ? _MK._ViewceMastFlowApprove.Where(x => x.mfStep == s_step && x.mfFlowNo == "4").Select(x => x.mfCC).FirstOrDefault() : "";


            string[] s_empCodeCC;// = v_empCodeCC.Split(",");
            string v_nameCC = "";

            if (!string.IsNullOrWhiteSpace(v_empCodeCC))
            {
                s_empCodeCC = v_empCodeCC.Split(",");
                for (int l = 0; l < s_empCodeCC.Count(); l++)
                {
                    var v_emailcc = _IT.rpEmails.Where(x => x.emEmpcode == s_empCodeCC[l].ToString()).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365
                    v_nameCC += v_emailcc + ",";
                    //_listNameTo.Add(v_emailcc);
                }
            }





            ViewBag.step = s_step;
            @class._ViewceHistoryApproved.htFrom = v_emailFrom;
            @class._ViewceHistoryApproved.htTo = v_nameTo;
            @class._ViewceHistoryApproved.htCC = v_nameCC;
            @class._ViewceHistoryApproved.htStatus = "Approve";
            return PartialView("SendMail", @class);
        }

        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ItemMaterialRequestPartName)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";
            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            string[] chkSaveSendMail;
            int i_Step = 0;

            string[] vRunDoc;
            string[] vRunDocNo;

            try
            {


                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                i_Step = @class._ViewceMastMaterialRequest != null ? @class._ViewceMastMaterialRequest.mrStep : 0;
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
                        string v_empissue = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub).Select(x => x.mrEmpCodeRequest).First(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
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
                    string vDoc = @class._ViewceMastMaterialRequest.mrDocumentNoSub;
                    if (_ItemMaterialRequestPartName != null)
                    {
                        @class._ListViewceItemMaterialRequestPartName = JsonConvert.DeserializeObject<List<ViewceItemMaterialRequestPartName>>(_ItemMaterialRequestPartName);
                    }

                    //save
                    chkSave = Save(@class, i_Step, files, "S");
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

                    //save history
                    chkSaveHistory = SaveHistory(@class, i_Step, vDoc);
                    if (chkSave[0] == "E")
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


                    //send mail
                    chkSaveSendMail = SendMailHistory(@class, i_Step, vDoc);
                    if (chkSaveSendMail[0] == "E")
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                        return Json(new { c1 = config, c2 = msg });
                    }
                    else
                    {
                        config = chkSaveSendMail[0];
                        msg = chkSaveSendMail[1];
                    }


                }
                else
                {
                    config = "E";
                    return Json(new { c1 = config, c2 = msg });
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;
            }


            return Json(new { c1 = config, c2 = msg });
        }

        public string[] Save(Class @class, int vstep, List<IFormFile> files, string savetype)
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
                    string vDocNo = @class._ViewceMastMaterialRequest.mrDocumentNoSub;
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "5").Select(x => x.mfSubject).First();
                    string empApprove = @class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastMaterialRequest.mrEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastMaterialRequest.mrNameApprove;


                    //checked dis approve   20/10/2025
                    int vStepDis = 0;
                    if (vstep == 8)
                    {
                        string vDocNoMain = @class._ViewceMastMaterialRequest.mrDocumentNo;
                        if (@class._ViewceMastMaterialRequest.mrStep == 1)
                        {
                            vStepDis = 8;
                            empApprove = @class._ViewceMastMaterialRequest.mrEmpCodeRequest;
                            string vstatusPanding = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "4" && x.mfStep == 9).Select(x => x.mfSubject).FirstOrDefault();
                            //update status
                            //wr
                            ViewceMastWorkingTimeRequest _MastWorkingTimeRequest = new ViewceMastWorkingTimeRequest();
                            _MastWorkingTimeRequest = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == vDocNoMain).FirstOrDefault();
                            var approvedNameWr = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _MastWorkingTimeRequest.wrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            var empCodeWr = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameWr).Select(y => y.emEmpcode).FirstOrDefault();
                            var nicknameAppWr = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeWr).Select(u => u.NICKNAME).FirstOrDefault();

                            _MastWorkingTimeRequest.wrStep = 0;
                            _MastWorkingTimeRequest.wrStatus = vstatusPanding;
                            _MastWorkingTimeRequest.wrEmpCodeApprove = empCodeWr;//_IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _MastWorkingTimeRequest.wrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();
                            _MastWorkingTimeRequest.wrNameApprove = nicknameAppWr;// _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _MastWorkingTimeRequest.wrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x => x.NICKNAME).FirstOrDefault();
                            _MK._ViewceMastWorkingTimeRequest.Update(_MastWorkingTimeRequest);
                            _MK.SaveChanges();

                            //Mat


                            //ViewceMastMaterialRequest _ceMastMaterialRequest = new ViewceMastMaterialRequest();
                            //_ceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == vDocNoMain).FirstOrDefault();
                            //var approvedNameMat = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            //var empCodeMat = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameMat).Select(y => y.emEmpcode).FirstOrDefault();
                            //var nicknameAppMat = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeMat).Select(u => u.NICKNAME).FirstOrDefault();


                            //_ceMastMaterialRequest.mrStep = 0;
                            //_ceMastMaterialRequest.mrStatus = vstatusPanding;
                            //_ceMastMaterialRequest.mrEmpCodeApprove = empCodeMat; // _IT.rpEmails.Where(y=>y.emName_M365 ==   _MK._ViewceHistoryApproved.Where(x=>x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep==1).Select(x=>x.htTo).FirstOrDefault()).Select(x=>x.emEmpcode).FirstOrDefault();
                            //_ceMastMaterialRequest.mrNameApprove = nicknameAppMat;//_HRMS.AccEMPLOYEE.Where(u=>u.EMP_CODE ==   _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x=>x.NICKNAME).FirstOrDefault();
                            //_MK._ViewceMastMaterialRequest.Update(_ceMastMaterialRequest);
                            //_MK.SaveChanges();

                            //Tool
                            ViewceMastToolGRRequest _ceMastToolGRRequest = new ViewceMastToolGRRequest();
                            _ceMastToolGRRequest = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == vDocNoMain).FirstOrDefault();
                            var approvedNameTool = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastToolGRRequest.trDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            var empCodeTool = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameTool).Select(y => y.emEmpcode).FirstOrDefault();
                            var nicknameAppTool = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeTool).Select(u => u.NICKNAME).FirstOrDefault();

                            _ceMastToolGRRequest.trStep = 0;
                            _ceMastToolGRRequest.trStatus = vstatusPanding;
                            _ceMastToolGRRequest.trEmpCodeApprove = empCodeTool;//_IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastToolGRRequest.trDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();
                            _ceMastToolGRRequest.trNameApprove = nicknameAppTool;//_HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastToolGRRequest.trDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x => x.NICKNAME).FirstOrDefault();
                            _MK._ViewceMastToolGRRequest.Update(_ceMastToolGRRequest);
                            _MK.SaveChanges();

                            //SM
                            ViewceMastInforSpacMoldRequest _ceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
                            _ceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == vDocNoMain).FirstOrDefault();
                            var approvedNameSM = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            var empCodeSM = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameSM).Select(y => y.emEmpcode).FirstOrDefault();
                            var nicknameAppSM = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeSM).Select(u => u.NICKNAME).FirstOrDefault();

                            _ceMastInforSpacMoldRequest.irStep = 0;
                            _ceMastInforSpacMoldRequest.irStatus = vstatusPanding;
                            _ceMastInforSpacMoldRequest.irEmpCodeApprove = empCodeSM;//_IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();
                            _ceMastInforSpacMoldRequest.irNameApprove = nicknameAppSM;//_HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x => x.NICKNAME).FirstOrDefault();
                            _MK._ViewceMastInforSpacMoldRequest.Update(_ceMastInforSpacMoldRequest);
                            _MK.SaveChanges();


                            //re step main other
                            string reStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3" && x.mfStep == 1).Select(x => x.mfSubject).FirstOrDefault();
                            ViewceMastMoldOtherRequest _ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
                            _ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == vDocNoMain).FirstOrDefault();
                            _ViewceMastMoldOtherRequest.mrStep = 1;
                            _ViewceMastMoldOtherRequest.mrStatus = reStatus;// _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "3" && x.mfStep == 1).Select(x => x.mfSubject).FirstOrDefault();
                            _MK._ViewceMastMoldOtherRequest.Update(_ViewceMastMoldOtherRequest);
                            _MK.SaveChanges();
                        }
                        else
                        {
                            vStepDis = 1;
                            vstep = 1;
                            // empApprove = _IT.rpEmails.Where(w => w.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == vDocNo && x.htStep == vStepDis).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).First();
                            string vname = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == vDocNo && x.htStep == vStepDis).Select(x => x.htTo).FirstOrDefault();
                            empApprove = _IT.rpEmails.Where(w => w.emName_M365 == vname).Select(x => x.emEmpcode).FirstOrDefault();


                        }
                        _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vStepDis && x.mfFlowNo == "5").Select(x => x.mfSubject).First();
                        NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastMaterialRequest.mrNameApprove;
                    }




                    vstep = vstep == 8 ? vstep = 0 : vstep;
                    ViewceMastMaterialRequest _ceMastMaterialRequest = new ViewceMastMaterialRequest();
                    if (savetype == "S")
                    {
                        _ceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == vDocNo).FirstOrDefault();
                        _ceMastMaterialRequest.mrIssueDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                        _ceMastMaterialRequest.mrStep = vstep;
                        _ceMastMaterialRequest.mrStatus = _smStatus;
                        //_ceMastWorkingTimeRequest.wrEmpCodeRequest
                        //_ceMastWorkingTimeRequest.wrNameRequest
                        _ceMastMaterialRequest.mrEmpCodeApprove = empApprove;
                        _ceMastMaterialRequest.mrNameApprove = NickNameApprove;
                        _ceMastMaterialRequest.mrFlowNo = 5;
                        _MK._ViewceMastMaterialRequest.Update(_ceMastMaterialRequest);
                        _MK.SaveChanges();
                    }

                    var itemMT = _MK._ViewceItemMaterialRequestPartName.Where(p => p.mpDocumentNoSub == vDocNo).ToList();
                    if (itemMT.Count > 0)
                    {
                        _MK._ViewceItemMaterialRequestPartName.RemoveRange(itemMT);
                        _MK.SaveChanges();
                    }





                    //SqlBulk insert
                    var connection = (SqlConnection)_MK.Database.GetDbConnection();
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    // ใช้ Transaction เดิมของ EF
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, (SqlTransaction)dbContextTransaction.GetDbTransaction()))
                    {
                        bulkCopy.DestinationTableName = "ceItemMaterialRequestPartName";
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.BulkCopyTimeout = 0;

                        // --- สร้าง DataTable ---
                        DataTable dt = new DataTable();
                        dt.Columns.Add("mpDocumentNoSub", typeof(string));
                        dt.Columns.Add("mpRunNo", typeof(int));
                        dt.Columns.Add("mpPartName", typeof(string));
                        dt.Columns.Add("mpCavityNo", typeof(string));
                        dt.Columns.Add("mpTypeCavity", typeof(string));
                        dt.Columns.Add("mpNoProcess", typeof(string));
                        dt.Columns.Add("mpNo", typeof(string));
                        dt.Columns.Add("mpItem", typeof(string));
                        dt.Columns.Add("mpPCS", typeof(int));
                        dt.Columns.Add("mpAmount", typeof(decimal));
                        dt.Columns.Add("mpTotal", typeof(decimal));
                        dt.Columns.Add("mpIssueDate", typeof(DateTime));

                        foreach (var item in @class._ListViewceItemMaterialRequestPartName)
                        {
                            dt.Rows.Add(
                                item.mpDocumentNoSub,
                                item.mpRunNo,
                                item.mpPartName,
                                item.mpCavityNo,
                                item.mpTypeCavity,
                                item.mpNoProcess,
                                item.mpNo,
                                item.mpItem,
                                item.mpPCS,
                                item.mpAmount,
                                item.mpTotal,
                                item.mpIssueDate
                            );
                        }

                        // Mapping columns
                        bulkCopy.ColumnMappings.Add("mpDocumentNoSub", "mpDocumentNoSub");
                        bulkCopy.ColumnMappings.Add("mpRunNo", "mpRunNo");
                        bulkCopy.ColumnMappings.Add("mpPartName", "mpPartName");
                        bulkCopy.ColumnMappings.Add("mpCavityNo", "mpCavityNo");
                        bulkCopy.ColumnMappings.Add("mpTypeCavity", "mpTypeCavity");
                        bulkCopy.ColumnMappings.Add("mpNoProcess", "mpNoProcess");
                        bulkCopy.ColumnMappings.Add("mpNo", "mpNo");
                        bulkCopy.ColumnMappings.Add("mpItem", "mpItem");
                        bulkCopy.ColumnMappings.Add("mpPCS", "mpPCS");
                        bulkCopy.ColumnMappings.Add("mpAmount", "mpAmount");
                        bulkCopy.ColumnMappings.Add("mpTotal", "mpTotal");
                        bulkCopy.ColumnMappings.Add("mpIssueDate", "mpIssueDate");

                        // Bulk insert
                        bulkCopy.WriteToServer(dt);
                    }




                    _MK.SaveChanges();
                    dbContextTransaction.Commit();


                    string[] v_statusFile = savefile(@class, files, vDocNo);

                    v_status = v_statusFile[0];
                    v_msg = v_statusFile[1];
                }
                catch (Exception ex)
                {
                    try { dbContextTransaction.Rollback(); } catch { }
                    v_status = "E";
                    v_msg = "Error Save: " + ex.InnerException.Message;
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
            var chkData = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == @class._ViewceMastMaterialRequest.mrDocumentNoSub).FirstOrDefault();
            try
            {
                if (chkData != null)
                {
                    if (chkData.mrStep != 4)
                    {
                        //check operator //check create user
                        if (chkData.mrStep == 0 && _UserId == chkData.mrEmpCodeRequest)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (_UserId == chkData.mrEmpCodeApprove)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (chkData.mrStep == 4 && _Permiss.ToUpper() == "ADMIN")
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

        public string[] SaveHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";

            //test send mail
            string vEmpCodeCCemail = "";


            using (var dbContextTransaction = _MK.Database.BeginTransaction())
            {
                try
                {
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "5").Select(x => x.mfSubject).First();

                    //checked dis approve   20/10/2025
                    int vStepDis = 0;
                    if (vstep == 8)
                    {
                        if (@class._ViewceMastMaterialRequest.mrStep > 1)
                        {
                            vStepDis = 1;
                            vstep = 1;
                            @class._ViewceHistoryApproved.htTo = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == RunDoc && x.htStep == vStepDis).Select(x => x.htTo).FirstOrDefault();
                            _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vStepDis && x.mfFlowNo == "4").Select(x => x.mfSubject).First();
                        }
                    }



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
                    }

                    _MK.SaveChanges();
                    dbContextTransaction.Commit();
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


                }
            }
            string[] returnVal = { v_status, v_msg };
            return returnVal;
        }

        public string[] SendMailHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            try
            {
                RunDoc = @class._ViewceMastMaterialRequest.mrDocumentNo;

                string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
                int vRevision = @class._ViewceMastMoldOtherRequest.mrRevision;
                string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
                string v_EmpCodeRequest = @class._ViewceMastMaterialRequest.mrEmpCodeRequest == null || @class._ViewceMastMaterialRequest.mrEmpCodeRequest == ""
                                                                                    ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                    : @class._ViewceMastMaterialRequest.mrEmpCodeRequest + " : " + @class._ViewceMastMaterialRequest.mrNameRequest;



                string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "5").Select(x => x.mfSubject).First();


                //checked dis approve   20/10/2025
                if (vstep == 8)
                {
                    if (@class._ViewceMastMaterialRequest.mrStep == 1)
                    {
                        //wr
                        string Docsubwr = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == RunDoc).Select(x => x.wrDocumentNoSub).FirstOrDefault();
                        string ccwr = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == Docsubwr && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        //mat
                        //string DocsubMat = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == RunDoc).Select(x => x.mrDocumentNoSub).FirstOrDefault();
                        //string ccMat = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubMat && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        //Tool
                        string DocsubTool = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == RunDoc).Select(x => x.trDocumentNoSub).FirstOrDefault();
                        string ccTool = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubTool && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        //SM
                        string DocsubSM = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == RunDoc).Select(x => x.irDocumentNoSub).FirstOrDefault();
                        string ccSM = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubSM && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();

                        @class._ViewceHistoryApproved.htCC = @class._ViewceHistoryApproved.htCC + "," + Docsubwr + "," + ccTool + "," + ccSM;

                    }
                }


                vstep = vstep == 8 ? vstep = 0 : vstep;

                var email = new MimeMessage();
                ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                email.Subject = "Cost Estimate : Mold Other ==> Material   " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
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
                var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther&subType=M";
                //var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther";// + getSrNo[0].ToString();
                var bodyBuilder = new BodyBuilder();
                //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                string EmailBody = $"<div>" +
                    $"<B>Cost Estimate : Mold Other ==> Material </B> <br>" +
                    $"<B>Document No : </B> " + RunDoc + "<br>" +  //v_EmpCodeRequest
                    $"<B>Customer Name : </B> " + @class._ViewceMastMoldOtherRequest.mrCustomerName + "<br>" +
                    $"<B>Function : </B> " + @class._ViewceMastMoldOtherRequest.mrFunction + "<br>" +
                    $"<B>Revision No : </B> " + vRevision.ToString("D2") + " <br>" +
                    $"<B>Model Name : </B> " + @class._ViewceMastMoldOtherRequest.mrModelName + "<br>" +
                    $"<B>Request By : </B> " + v_EmpCodeRequest + "<br>" +
                    $"<B>Status : </B> " + _smStatus + "<br> " +
                    $"<B> หมายเหตุ : </B> " + @class._ViewceHistoryApproved.htRemark + "<br> " +
                    $"คลิ๊กลิงค์เพื่อเปิดเอกสาร <a href='" + varifyUrl + "'>More Detail" +
                    $"</a>" +
                    $"</div>";

                // bodyBuilder.Attachments.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\dev_rfc.log");

                bodyBuilder.HtmlBody = string.Format(EmailBody);
                email.Body = bodyBuilder.ToMessageBody();

                // send email
                var smtp1 = new SmtpClient();
                //smtp.Connect("mail.csloxinfo.com");
                smtp1.Connect("203.146.237.138");
                //smtp.Connect("10.200.128.12");
                smtp1.Send(email);
                smtp1.Disconnect(true);

                v_status = "S";
                v_msg = "File saved and email sent.!!!";
            }
            catch (Exception ex)
            {
                // dbContextTransaction.Rollback();

                v_status = "E";
                v_msg = "Error Save History: " + ex.Message;
            }



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


        [Authorize("Checked")]
        [HttpPost]
        public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ItemMaterialRequestPartName)
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
                i_Step = @class._ViewceMastMaterialRequest.mrStep;

                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                if (_ItemMaterialRequestPartName != null)
                {
                    @class._ListViewceItemMaterialRequestPartName = JsonConvert.DeserializeObject<List<ViewceItemMaterialRequestPartName>>(_ItemMaterialRequestPartName);
                }


                chkSave = Save(@class, i_Step, files, "D");
                if (chkSave[0] == "E")
                {
                    config = chkSave[0];
                    msg = chkSave[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                else
                {
                    config = chkSave[0];
                    msg = "Save Data success ";
                    // msg = chkSave[1];
                }

            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Something is wrong !!!!! : " + ex.Message;

            }
            return Json(new { c1 = config, c2 = msg });
        }


        [HttpPost]
        public IActionResult btnExportExcel(Class @class, string id)
        {
            List<ViewceMastMoldOtherRequest> _ListViewceMastMoldOtherRequest = new List<ViewceMastMoldOtherRequest>();
            @class._ListViewceItemWorkingTimePartName = new List<ViewceItemWorkingTimePartName>();
            string slipMat = id.ToString() + ":" + DateTime.Now.ToString("yyyyMMdd:HHmmss");
            string TempPath = Path.GetTempFileName();
            string fileName = "Export(" + slipMat + ").xlsx";

            //@class._ListceMastProcess = _MK._ViewceMastProcess.Where(x => x.mpType == "MoldOtherWK").OrderBy(x => x.mpNo).ToList();
            @class._ListceMastModel = _MK._ViewceMastModel.Where(x => x.mmType == "MoldOtherMaterial").OrderBy(x => x.mmNo).ToList();

            //check ข้อมูลเดิม
            string vDocNosub = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == id).Select(x => x.mrDocumentNoSub).FirstOrDefault();
            @class._ListViewceItemMaterialRequestPartName = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub).ToList();

            @class._ListViewceItemPartName = new List<ViewceItemPartName>();
            @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == id).OrderBy(x => x.ipRunNo).ToList();

            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ItemMat");
                worksheet.Cells.Style.Font.Size = 12;

                //header main
                //worksheet.Cells[1, 5].Value = "Process (ห้ามแก้)";
                //worksheet.Cells[1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                //worksheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.Red);
                //worksheet.Cells[1, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                int eRowMain = 1;
                int eColMain = 1;

                int eRow = 1;
                int ecol = 1;
                if (id != null)
                {
                    //for loop hearder 
                    for (int i = 0; i < @class._ListViewceItemPartName.Count(); i++)
                    {

                        //+1 col เพื่อขึ้น item อันใหม่
                        //mpDocumentNoSub
                        //mpRunNo
                        //mpPartName
                        //mpCavityNo
                        //mpTypeCavity
                        //mpNoProcess
                        //mpNo
                        worksheet.Cells[eRowMain, eColMain].Value = "Document No (ห้ามแก้)";
                        worksheet.Cells[eRowMain, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain, eColMain].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 1, eColMain].Value = "Part Name (ห้ามแก้)";
                        worksheet.Cells[eRowMain + 1, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 1, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 1, eColMain].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 1, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 2, eColMain].Value = "Cavity No (ห้ามแก้)";
                        worksheet.Cells[eRowMain + 2, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 2, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 2, eColMain].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 2, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 3, eColMain].Value = "Type Cavity (ห้ามแก้)";
                        worksheet.Cells[eRowMain + 3, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 3, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 4, eColMain].Value = "Process (ห้ามแก้)";
                        worksheet.Cells[eRowMain + 4, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 4, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 4, eColMain].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        worksheet.Cells[eRowMain + 4, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        worksheet.Cells[eRowMain, eColMain + 1].Value = vDocNosub;
                        worksheet.Cells[eRowMain, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //worksheet.Cells[eRowMain, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        worksheet.Cells[eRowMain, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 1, eColMain + 1].Value = @class._ListViewceItemPartName[i].ipPartName;
                        worksheet.Cells[eRowMain + 1, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //worksheet.Cells[eRowMain + 1, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        worksheet.Cells[eRowMain + 1, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);


                        worksheet.Cells[eRowMain + 2, eColMain + 1].Value = @class._ListViewceItemPartName[i].ipCavityNo;
                        worksheet.Cells[eRowMain + 2, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //worksheet.Cells[eRowMain + 2, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        worksheet.Cells[eRowMain + 2, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 3, eColMain + 1].Value = @class._ListViewceItemPartName[i].ipTypeCavity;
                        worksheet.Cells[eRowMain + 3, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //worksheet.Cells[eRowMain + 3, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        // worksheet.Cells[eRowMain + 3, eColMain].Style.Fill.BackgroundColor.SetColor(Color.Red);
                        worksheet.Cells[eRowMain + 3, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 4, eColMain + 1].Value = @class._ListViewceItemPartName[i].ipRunNo;
                        worksheet.Cells[eRowMain + 4, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        //worksheet.Cells[eRowMain + 4, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 4, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        //No.	ITEM	PCS.	AMOUNT
                        worksheet.Cells[eRowMain + 5, eColMain].Value = "No";
                        worksheet.Cells[eRowMain + 5, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 5, eColMain].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 5, eColMain].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 5, eColMain].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 5, eColMain + 1].Value = "ITEM";
                        worksheet.Cells[eRowMain + 5, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 5, eColMain + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 5, eColMain + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 5, eColMain + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 5, eColMain + 2].Value = "PCS";
                        worksheet.Cells[eRowMain + 5, eColMain + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 5, eColMain + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 5, eColMain + 2].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 5, eColMain + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        worksheet.Cells[eRowMain + 5, eColMain + 3].Value = "AMOUNT(BAHT)";
                        worksheet.Cells[eRowMain + 5, eColMain + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[eRowMain + 5, eColMain + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[eRowMain + 5, eColMain + 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[eRowMain + 5, eColMain + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);




                        int eRowitem = 7;
                        for (int j = 0; j < @class._ListceMastModel.Count(); j++)
                        {
                            worksheet.Cells[eRowitem, eColMain].Value = j + 1;
                            worksheet.Cells[eRowitem, eColMain + 1].Value = @class._ListceMastModel[j].mmModelName;
                            worksheet.Cells[eRowitem, eColMain + 2].Value = @class._ListViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub && x.mpNoProcess == @class._ListViewceItemPartName[i].ipRunNo && x.mpItem.Contains(@class._ListceMastModel[j].mmModelName)).FirstOrDefault() != null ? @class._ListViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub && x.mpNoProcess == @class._ListViewceItemPartName[i].ipRunNo && x.mpItem.Contains(@class._ListceMastModel[j].mmModelName)).Select(x => x.mpPCS).FirstOrDefault() : 0;
                            worksheet.Cells[eRowitem, eColMain + 3].Value = @class._ListViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub && x.mpNoProcess == @class._ListViewceItemPartName[i].ipRunNo && x.mpItem.Contains(@class._ListceMastModel[j].mmModelName)).FirstOrDefault() != null ? @class._ListViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub && x.mpNoProcess == @class._ListViewceItemPartName[i].ipRunNo && x.mpItem.Contains(@class._ListceMastModel[j].mmModelName)).Select(x => x.mpAmount).FirstOrDefault() * 1000 : 0;
                            eRowitem += 1;
                        }
                        //eRowMain += 5;
                        eColMain += 5;

                        //item mat
                    }
                }



                // Auto fit column width
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                // Export เป็น stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

        }
        //import
        [HttpPost]
        public ActionResult ImportDataFile(string _id, List<IFormFile> files, Class @class)
        {
            string config = "S";
            string msg = "Success!!!";
            string IssueBy = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " - " + HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            @class._ListViewceItemMaterialRequestPartName = new List<ViewceItemMaterialRequestPartName>();
            string vDocsubMain = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == _id).Select(x => x.mrDocumentNoSub).FirstOrDefault();
            try
            {

                if (files == null || files.Count == 0)
                {
                    config = "E";
                    msg = "⚠️ กรุณาเลือกไฟล์ Excel  1 ไฟล์ก่อนอัปโหลด";
                    return Json(new { c1 = config, c2 = msg });
                }
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            file.CopyToAsync(stream);
                            using (var package = new OfficeOpenXml.ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets[1]; // อ่านเฉพาะ sheet แรก
                                int colCount = worksheet.Dimension.End.Column;
                                int rowCount = worksheet.Dimension.End.Row;

                                //DataTable dt = new DataTable(file.FileName); // ใช้ชื่อไฟล์เป็นชื่อ DataTable
                                string vDocNosub = "";
                                string vPartName = "";
                                double vCavityNo;
                                string vTypeCavity = "";
                                int vProcess;

                                string[] chkPermis;
                                chkPermis = chkPermissionImport(worksheet.Cells[1, 2].Text);
                                if (chkPermis[0] != "S")
                                {
                                    config = chkPermis[0];
                                    msg = chkPermis[1];
                                    return Json(new { c1 = config, c2 = msg });
                                }

                                //check col
                                if (colCount > 3)
                                {
                                    int vRow;
                                    int sumTotal;
                                    int cHcol = 2; //header col
                                    //for loop check col
                                    for (int c = 1; c <= colCount; c++)
                                    {
                                        vDocNosub = worksheet.Cells[1, c + 1].Text; //doc no
                                        if (vDocsubMain != vDocNosub)
                                        {
                                            config = "E";
                                            msg = "Excel file incorrect: Please check your file !!!!";
                                            return Json(new { c1 = config, c2 = msg });
                                        }
                                        //partname
                                        vPartName = worksheet.Cells[2, c + 1].Text; // partname
                                        //type cavity
                                        vCavityNo = Convert.ToDouble(worksheet.Cells[3, c + 1].Text);   //cavityno
                                        //vTypeCavity
                                        vTypeCavity = worksheet.Cells[4, c + 1].Text; // partname
                                        //process
                                        vProcess = Convert.ToInt32(worksheet.Cells[5, c + 1].Text); // process


                                        //sum //ไม่ต้องเดี๋ยวหน้า ui มันจะบวกเอง

                                        //cHcol +4
                                        int mRunNo = 0;
                                        for (int row = 7; row <= rowCount; row++)
                                        {

                                            @class._ListViewceItemMaterialRequestPartName.Add(new ViewceItemMaterialRequestPartName
                                            {
                                                mpDocumentNoSub = vDocNosub,
                                                mpRunNo = mRunNo + 1,
                                                mpPartName = vPartName,
                                                mpCavityNo = vCavityNo,
                                                mpTypeCavity = vTypeCavity,
                                                mpNoProcess = vProcess,
                                                mpNo = mRunNo + 1,
                                                mpItem = worksheet.Cells[row, c + 1].Text,
                                                mpPCS = Convert.ToDouble(worksheet.Cells[row, c + 2].Value),
                                                mpAmount = worksheet.Cells[row, c + 3].Value == null || worksheet.Cells[row, c + 3].Value.ToString() == "" ? 0 : Convert.ToDouble(worksheet.Cells[row, c + 3].Value) / 1000,//Convert.ToDouble(worksheet.Cells[row, c + 3].Value) / 1000,
                                                mpTotal = 0,
                                                mpIssueDate = IssueBy,
                                            });
                                            mRunNo += 1;

                                        }
                                        c += 4;
                                        if (c > colCount)
                                        {
                                            c = colCount;
                                        }
                                    }




                                    //+col
                                }




                                using (var dbContextTransaction = _MK.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var _ceItemMatPartName = _MK._ViewceItemMaterialRequestPartName.Where(x => x.mpDocumentNoSub == vDocNosub).ToList();
                                        if (_ceItemMatPartName.Count > 0)
                                        {
                                            _MK._ViewceItemMaterialRequestPartName.RemoveRange(_ceItemMatPartName);
                                            _MK.SaveChanges();
                                        }


                                        var connection = (SqlConnection)_MK.Database.GetDbConnection();
                                        if (connection.State != ConnectionState.Open)
                                            connection.Open();
                                        // ใช้ Transaction เดิมของ EF
                                        using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, (SqlTransaction)dbContextTransaction.GetDbTransaction()))
                                        {
                                            bulkCopy.DestinationTableName = "ceItemMaterialRequestPartName";
                                            bulkCopy.BatchSize = 5000;
                                            bulkCopy.BulkCopyTimeout = 0;

                                            DataTable dt = new DataTable();
                                            dt.Columns.Add("mpDocumentNoSub", typeof(string));
                                            dt.Columns.Add("mpRunNo", typeof(int));
                                            dt.Columns.Add("mpPartName", typeof(string));
                                            dt.Columns.Add("mpCavityNo", typeof(double));
                                            dt.Columns.Add("mpTypeCavity", typeof(string));
                                            dt.Columns.Add("mpNoProcess", typeof(int));
                                            dt.Columns.Add("mpNo", typeof(int));
                                            dt.Columns.Add("mpItem", typeof(string));
                                            dt.Columns.Add("mpPCS", typeof(double));
                                            dt.Columns.Add("mpAmount", typeof(double));
                                            dt.Columns.Add("mpTotal", typeof(double));
                                            dt.Columns.Add("mpIssueDate", typeof(string));

                                            for (int i = 0; i < @class._ListViewceItemMaterialRequestPartName.Count; i++)
                                            {
                                                var item = @class._ListViewceItemMaterialRequestPartName[i];
                                                dt.Rows.Add(
                                                    item.mpDocumentNoSub,
                                                    item.mpRunNo,
                                                    item.mpPartName,
                                                    item.mpCavityNo,
                                                    item.mpTypeCavity,
                                                    item.mpNoProcess,
                                                    item.mpNo,
                                                    item.mpItem,
                                                    item.mpPCS,
                                                    item.mpAmount,
                                                    item.mpTotal,
                                                    item.mpIssueDate
                                                //DateTime.Now.ToString("yyyyMMdd HH:mm:ss")
                                                );
                                            }

                                            // Mapping (กรณีชื่อคอลัมน์ตรง DB อยู่แล้ว อาจไม่ต้องใส่ก็ได้)
                                            bulkCopy.ColumnMappings.Add("mpDocumentNoSub", "mpDocumentNoSub");
                                            bulkCopy.ColumnMappings.Add("mpRunNo", "mpRunNo");
                                            bulkCopy.ColumnMappings.Add("mpPartName", "mpPartName");
                                            bulkCopy.ColumnMappings.Add("mpCavityNo", "mpCavityNo");
                                            bulkCopy.ColumnMappings.Add("mpTypeCavity", "mpTypeCavity");
                                            bulkCopy.ColumnMappings.Add("mpNoProcess", "mpNoProcess");
                                            bulkCopy.ColumnMappings.Add("mpNo", "mpNo");
                                            bulkCopy.ColumnMappings.Add("mpItem", "mpItem");
                                            bulkCopy.ColumnMappings.Add("mpPCS", "mpPCS");
                                            bulkCopy.ColumnMappings.Add("mpAmount", "mpAmount");
                                            bulkCopy.ColumnMappings.Add("mpTotal", "mpTotal");
                                            bulkCopy.ColumnMappings.Add("mpIssueDate", "mpIssueDate");
                                            bulkCopy.WriteToServer(dt);
                                        }

                                        _MK.SaveChanges();
                                        dbContextTransaction.Commit();

                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            dbContextTransaction.Rollback();
                                        }
                                        catch
                                        {
                                            config = "E";
                                            msg = "Insert incorrect :Something is wrong !!!!! : " + ex.Message;
                                            return Json(new
                                            {
                                                c1 = config,
                                                c2 = msg
                                            });
                                        }
                                    }
                                }



                            }



                        }
                    }
                }




            }
            catch (Exception ex)
            {
                config = "E";
                msg = "Excel file incorrect : Something is wrong !!!!! : " + ex.Message;
                return Json(new { c1 = config, c2 = msg });
            }
            return Json(new { c1 = config, c2 = msg });
        }



        public string[] chkPermissionImport(string id)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            try
            {
                var chkData = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNoSub == id).FirstOrDefault();
                if (chkData != null)
                {
                    if (chkData.mrStep != 4)
                    {
                        //check operator //check create user
                        if (chkData.mrStep == 0 && _UserId == chkData.mrEmpCodeRequest)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (_UserId == chkData.mrEmpCodeApprove)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (chkData.mrStep == 4 && _Permiss.ToUpper() == "ADMIN")
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
                string[] returnvar = { "E", ex.Message };
                return returnvar;

            }
        }

    }
}
