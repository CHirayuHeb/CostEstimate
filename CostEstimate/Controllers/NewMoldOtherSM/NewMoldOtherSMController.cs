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

namespace CostEstimate.Controllers.NewMoldOtherSM
{
    public class NewMoldOtherSMController : Controller
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
        public NewMoldOtherSMController(LAMP lamp, HRMS hrms, IT it, MK mk, MOLD mold, CacheSettingController cacheController, FunctionsController callfunction)
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
            @class._ListceMastFlowApprove = _MK._ViewceMastFlowApprove.Where(x => x.mfFlowNo == "7").ToList();

            @class._ViewceMastMoldOtherRequest = new ViewceMastMoldOtherRequest();
            @class._ViewceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
            @class._ListceMastModel = new List<ViewceMastModel>();

            @class._ViewceItemInforSlideSystem = new ViewceItemInforSlideSystem();
            @class._ListViewceItemInforRequestPartName = new List<ViewceItemInforRequestPartName>();
            @class._ListViewceItemInforSlideSystem = new List<ViewceItemInforSlideSystem>();
            @class._ListViewceItemInforTypeOfCut = new List<ViewceItemInforTypeOfCut>();
            @class._ListViewceItemInforShibo = new List<ViewceItemInforShibo>();
            @class._ListGroupViewceMastInforSpacMoldRequest = new List<GroupViewceMastInforSpacMoldRequest>();
            @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();
            @class._ViewceHistoryApproved = new ViewceHistoryApproved();


            List<string> _listSprueSystem = _MK._ViewceMastType.Where(x => x.mtType.Contains("SprueSystem") && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listSprueSystem = new SelectList(_listSprueSystem);
            ViewBag.listSprueSystem = listSprueSystem;

            List<string> _listRunner = _MK._ViewceMastType.Where(x => x.mtType == "Runner" && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listRunner = new SelectList(_listRunner);
            ViewBag.listRunner = listRunner;


            List<string> _listMakerHotRunner = _MK._ViewceMastType.Where(x => x.mtType.Contains("MakerHotRunner ") && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listMakerHotRunner = new SelectList(_listMakerHotRunner);
            ViewBag.listMakerHotRunner = listMakerHotRunner;

            List<string> _listGateType = _MK._ViewceMastType.Where(x => x.mtType.Contains("GateType") && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listGateType = new SelectList(_listGateType);
            ViewBag.listGateType = listGateType;

            List<string> _listSlideSystem = _MK._ViewceMastType.Where(x => x.mtType.Contains("SlideSystem") && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listSlideSystem = new SelectList(_listSlideSystem);
            ViewBag.listSlideSystem = listSlideSystem;


            List<string> _listGroupMaterial = _MK._ViewceMastType.Where(x => x.mtType == "GroupMaterial" && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listGroupMaterial = new SelectList(_listGroupMaterial);
            ViewBag.listGroupMaterial = listGroupMaterial;


            ViewBag.listELECTROFORM = new SelectList(new[] { new { Value = true, Text = "YES" }, new { Value = false, Text = "NO" } }, "Value", "Text");


            List<string> _listTypeOfCut = _MK._ViewceMastType.Where(x => x.mtType == "TypeOfCut" && x.mtProgram.Contains("MoldOtherSM")).OrderBy(x => x.mtName).Select(x => x.mtName).ToList();
            SelectList listTypeOfCut = new SelectList(_listTypeOfCut);
            ViewBag.listTypeOfCut = listTypeOfCut;


            if (Docno != null)
            {
                @class._ViewceMastMoldOtherRequest = _MK._ViewceMastMoldOtherRequest.Where(x => x.mrDocmentNo == Docno).FirstOrDefault();
                @class._ViewceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == Docno).FirstOrDefault();

                @class._ListViewceItemPartName = _MK._ViewceItemPartName.Where(x => x.ipDocumentNo == Docno).OrderBy(x => x.ipRunNo).ToList();
                for (int j = 0; j < @class._ListViewceItemPartName.Count(); j++)
                {
                    var _ceItemInforRequestPartName = _MK._ViewceItemInforRequestPartName.Where(x => x.ipNoProcess == @class._ListViewceItemPartName[j].ipRunNo && x.ipDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).FirstOrDefault();
                    @class._ListViewceItemInforRequestPartName.Add(new ViewceItemInforRequestPartName
                    {
                        ipDocumentNoSub = @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub,
                        ipRunNo = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipRunNo : j + 1,
                        ipPartName = @class._ListViewceItemPartName[j].ipPartName,
                        ipCavityNo = @class._ListViewceItemPartName[j].ipCavityNo,
                        ipTypeCavity = @class._ListViewceItemPartName[j].ipTypeCavity,
                        ipNoProcess = @class._ListViewceItemPartName[j].ipRunNo,
                        ipSprueSystem = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipSprueSystem : "",
                        ipRunner = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipRunner : "",
                        ipMakerHotRunner = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipMakerHotRunner : "",
                        ipGateType1 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipGateType1 : "",
                        ipNumberPoint1 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipNumberPoint1 : 0,
                        ipGateType2 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipGateType2 : "",
                        ipNumberPoint2 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipNumberPoint2 : 0,
                        ipGateType3 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipGateType3 : "",
                        ipNumberPoint3 = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipNumberPoint3 : 0,
                        ipBaseCavity = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipBaseCavity : "",
                        ipInsertCavity = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipInsertCavity : "",
                        ipBaseCode = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipBaseCode : "",
                        ipInsertCode = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipInsertCode : "",
                        ipSlide = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipSlide : "",
                        ipElectroFormType = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipElectroFormType : false,
                        ipElectroFormPcs = _ceItemInforRequestPartName != null ? _ceItemInforRequestPartName.ipElectroFormPcs : 0,
                    });

                    var _ceItemInforSlideSystem = _MK._ViewceItemInforSlideSystem.Where(x => x.isNoProcess == @class._ListViewceItemPartName[j].ipRunNo && x.isDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).ToList();
                    @class._ListViewceItemInforSlideSystem.AddRange(_ceItemInforSlideSystem);

                    var _ceItemInforTypeOfCut = _MK._ViewceItemInforTypeOfCut.Where(x => x.icNoProcess == @class._ListViewceItemPartName[j].ipRunNo && x.icDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).ToList();
                    @class._ListViewceItemInforTypeOfCut.AddRange(_ceItemInforTypeOfCut);

                    var _ceItemInforShibo = _MK._ViewceItemInforShibo.Where(x => x.ibNoProcess == @class._ListViewceItemPartName[j].ipRunNo && x.ibDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).ToList();
                    @class._ListViewceItemInforShibo.AddRange(_ceItemInforShibo);

                }

                @class._ListGroupViewceMastInforSpacMoldRequest = @class._ListViewceItemInforRequestPartName.GroupBy(x => new { x.ipPartName, x.ipCavityNo, x.ipTypeCavity, x.ipNoProcess })
                      .Select(g => new GroupViewceMastInforSpacMoldRequest
                      {
                          ipPartName = g.Key.ipPartName,
                          ipCavityNo = g.Key.ipCavityNo,
                          ipTypeCavity = g.Key.ipTypeCavity,
                          ipNoProcess = g.Key.ipNoProcess,
                          InforRequestPartName = g.ToList(),
                          ItemInforSlideSystem = @class._ListViewceItemInforSlideSystem.Where(x => x.isNoProcess == g.Key.ipNoProcess).ToList(),
                          ItemInforTypeOfCut = @class._ListViewceItemInforTypeOfCut.Where(x => x.icNoProcess == g.Key.ipNoProcess).ToList(),
                          ItemInforShibo = @class._ListViewceItemInforShibo.Where(x => x.ibNoProcess == g.Key.ipNoProcess).ToList(),

                      }).ToList();




                @class._listAttachment = _IT.Attachment.Where(x => x.fnNo == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).ToList();

            }
            return View(@class);
        }





        public JsonResult History(Class @classs)//string getID)
        {
            //Class @class ,
            string partialUrl = "";
            int v_step = @classs._ViewceMastInforSpacMoldRequest != null ? @classs._ViewceMastInforSpacMoldRequest.irStep : 0;
            string v_issue = @classs._ViewceMastInforSpacMoldRequest != null ? @classs._ViewceMastInforSpacMoldRequest.irEmpCodeRequest : "";
            string v_DocNo = @classs._ViewceMastInforSpacMoldRequest != null ? @classs._ViewceMastInforSpacMoldRequest.irDocumentNoSub : "";
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



            partialUrl = Url.Action("SendMail", "NewMoldOtherSM", new { @class = @classs, s_step = v_step, s_issue = v_issue, mpNo = v_DocNo });

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
            string v_empCodeTo = s_step == 3 ? _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNoSub == mpNo).Select(x => x.irEmpCodeRequest).FirstOrDefault()
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
        public ActionResult HistoryPost(Class @class)//string getID)
        {
            try
            {
                int s_step = @class._ViewceMastInforSpacMoldRequest.irStep;
                string mpNo = @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub;
                @class._ListViewceHistoryApproved = new List<ViewceHistoryApproved>();
                List<ViewceHistoryApproved> _listHistory = new List<ViewceHistoryApproved>();
                string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
                ViewBag.vDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm:ss");
                @class._ViewceHistoryApproved = new ViewceHistoryApproved();
                var v_emailFrom = _IT.rpEmails.Where(x => x.emEmpcode == _UserId).Select(p => p.emName_M365).FirstOrDefault(); //chg to m365



                _listHistory = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).OrderBy(x => x.htDate).ThenBy(x => x.htTime).ThenBy(x => x.htStep).ToList();
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

                @class._ListViewceHistoryApproved = _listHistory;


                //To
                string v_empCodeTo = s_step == 3 ? _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNoSub == mpNo).Select(x => x.irEmpCodeRequest).FirstOrDefault()
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
                //ViewBag.v_nameCC = v_nameCC;
                @class._ViewceHistoryApproved.htFrom = v_emailFrom;
                @class._ViewceHistoryApproved.htTo = v_nameTo;
                @class._ViewceHistoryApproved.htCC = v_nameCC;
                @class._ViewceHistoryApproved.htStatus = "Approve";
            }
            catch (Exception ex)
            {

            }

            return PartialView("SendMailPost", @class);
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
        public JsonResult SaveDraft(Class @class, List<IFormFile> files, string _ceItemInforSlideSystem, string _ceItemInforTypeOfCut, string _ceItemInforShibo)
        {

            string config = "S";
            string msg = "Save Data success";

            string[] chkPermis;
            string[] chkSave;

            string[] chkSaveHistory;
            string[] chkSaveSendMail;

            int i_Step = 0;
            string[] vRunDoc;
            string[] vRunDocNo;
            string[] sRunDoc;
            try
            {

                string vDoc = @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub;
                i_Step = @class._ViewceMastInforSpacMoldRequest.irStep;

                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                if (_ceItemInforSlideSystem != null)
                {
                    @class._ListViewceItemInforSlideSystem = JsonConvert.DeserializeObject<List<ViewceItemInforSlideSystem>>(_ceItemInforSlideSystem);
                }
                if (_ceItemInforTypeOfCut != null)
                {
                    @class._ListViewceItemInforTypeOfCut = JsonConvert.DeserializeObject<List<ViewceItemInforTypeOfCut>>(_ceItemInforTypeOfCut);
                }
                if (_ceItemInforShibo != null)
                {
                    @class._ListViewceItemInforShibo = JsonConvert.DeserializeObject<List<ViewceItemInforShibo>>(_ceItemInforShibo);
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
        public string[] chkPermission(Class @class)
        {
            string _UserId = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            string _Permiss = User.Claims.FirstOrDefault(s => s.Type == "Permission")?.Value;
            string message_per = "";
            string status_per = "";
            var chkData = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).FirstOrDefault();
            try
            {
                if (chkData != null)
                {
                    if (chkData.irStep != 4)
                    {
                        //check operator //check create user
                        if (chkData.irStep == 0 && _UserId == chkData.irEmpCodeRequest)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (_UserId == chkData.irEmpCodeApprove)
                        {
                            status_per = "S";
                            message_per = "You have permission ";
                        }
                        else if (chkData.irStep == 4 && _Permiss.ToUpper() == "ADMIN")
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
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "7").Select(x => x.mfSubject).First();
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




        [HttpPost]
        public JsonResult chkSaveData(Class @class, List<IFormFile> files, string _ceItemInforSlideSystem, string _ceItemInforTypeOfCut, string _ceItemInforShibo)
        {
            string config = "S";
            string msg = "Send Mail & Save File Already";
            string vStatus = "";
            string[] chkPermis;
            string[] chkSave;
            string[] chkSaveHistory;
            string[] chkSaveSendMail;
            int i_Step = 0;
            try
            {
                chkPermis = chkPermission(@class);
                if (chkPermis[0] == "P")
                {
                    config = chkPermis[0];
                    msg = chkPermis[1];
                    return Json(new { c1 = config, c2 = msg });
                }
                i_Step = @class._ViewceMastInforSpacMoldRequest != null ? @class._ViewceMastInforSpacMoldRequest.irStep : 0;
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
                        string v_empissue = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNoSub == @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub).Select(x => x.irEmpCodeRequest).FirstOrDefault(); // _IT.rpEmails.Where(x=>x.emName_M365.co)
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
                    string vDoc = @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub;

                    if (_ceItemInforSlideSystem != null)
                    {
                        @class._ListViewceItemInforSlideSystem = JsonConvert.DeserializeObject<List<ViewceItemInforSlideSystem>>(_ceItemInforSlideSystem);
                    }
                    if (_ceItemInforTypeOfCut != null)
                    {
                        @class._ListViewceItemInforTypeOfCut = JsonConvert.DeserializeObject<List<ViewceItemInforTypeOfCut>>(_ceItemInforTypeOfCut);
                    }
                    if (_ceItemInforShibo != null)
                    {
                        @class._ListViewceItemInforShibo = JsonConvert.DeserializeObject<List<ViewceItemInforShibo>>(_ceItemInforShibo);
                    }

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
                    string vDocNo = @class._ViewceMastInforSpacMoldRequest.irDocumentNoSub;
                    string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "7").Select(x => x.mfSubject).First();
                    string empApprove = @class._ViewceHistoryApproved != null ?
                        @class._ViewceHistoryApproved.htTo != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First()
                        : @class._ViewceMastInforSpacMoldRequest.irEmpCodeApprove
                        : @class._ViewceMastInforSpacMoldRequest.irEmpCodeApprove;



                    //@class._ViewceHistoryApproved != null ? _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).Select(x => x.emEmpcode).First() : @class._ViewceMastInforSpacMoldRequest.irEmpCodeApprove;
                    string NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastInforSpacMoldRequest.irNameApprove;


                    //checked dis approve   20/10/2025
                    int vStepDis = 0;
                    if (vstep == 8)
                    {
                        string vDocNoMain = @class._ViewceMastInforSpacMoldRequest.irDocumentNo;
                        if (@class._ViewceMastInforSpacMoldRequest.irStep == 1)
                        {
                            vStepDis = 8;
                            empApprove = @class._ViewceMastInforSpacMoldRequest.irEmpCodeRequest;
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


                            ViewceMastMaterialRequest _ceMastMaterialRequest = new ViewceMastMaterialRequest();
                            _ceMastMaterialRequest = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == vDocNoMain).FirstOrDefault();
                            var approvedNameMat = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            var empCodeMat = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameMat).Select(y => y.emEmpcode).FirstOrDefault();
                            var nicknameAppMat = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeMat).Select(u => u.NICKNAME).FirstOrDefault();


                            _ceMastMaterialRequest.mrStep = 0;
                            _ceMastMaterialRequest.mrStatus = vstatusPanding;
                            _ceMastMaterialRequest.mrEmpCodeApprove = empCodeMat; // _IT.rpEmails.Where(y=>y.emName_M365 ==   _MK._ViewceHistoryApproved.Where(x=>x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep==1).Select(x=>x.htTo).FirstOrDefault()).Select(x=>x.emEmpcode).FirstOrDefault();
                            _ceMastMaterialRequest.mrNameApprove = nicknameAppMat;//_HRMS.AccEMPLOYEE.Where(u=>u.EMP_CODE ==   _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastMaterialRequest.mrDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x=>x.NICKNAME).FirstOrDefault();
                            _MK._ViewceMastMaterialRequest.Update(_ceMastMaterialRequest);
                            _MK.SaveChanges();

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
                            //ViewceMastInforSpacMoldRequest _ceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
                            //_ceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == vDocNoMain).FirstOrDefault();
                            //var approvedNameSM = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                            //var empCodeSM = _IT.rpEmails.Where(y => y.emName_M365 == approvedNameSM).Select(y => y.emEmpcode).FirstOrDefault();
                            //var nicknameAppSM = _HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == empCodeSM).Select(u => u.NICKNAME).FirstOrDefault();

                            //_ceMastInforSpacMoldRequest.irStep = 0;
                            //_ceMastInforSpacMoldRequest.irStatus = vstatusPanding;
                            //_ceMastInforSpacMoldRequest.irEmpCodeApprove = empCodeSM;//_IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault();
                            //_ceMastInforSpacMoldRequest.irNameApprove = nicknameAppSM;//_HRMS.AccEMPLOYEE.Where(u => u.EMP_CODE == _IT.rpEmails.Where(y => y.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == _ceMastInforSpacMoldRequest.irDocumentNo && x.htStep == 1).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).FirstOrDefault()).Select(x => x.NICKNAME).FirstOrDefault();
                            //_MK._ViewceMastInforSpacMoldRequest.Update(_ceMastInforSpacMoldRequest);
                            //_MK.SaveChanges();


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
                            //mpApprove = _IT.rpEmails.Where(w => w.emName_M365 == _MK._ViewceHistoryApproved.Where(x => x.htDocNo == vDocNo && x.htStep == vStepDis).Select(x => x.htTo).FirstOrDefault()).Select(x => x.emEmpcode).First();

                            string vname = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == vDocNo && x.htStep == vStepDis).Select(x => x.htTo).FirstOrDefault();
                            empApprove = _IT.rpEmails.Where(w => w.emName_M365 == vname).Select(x => x.emEmpcode).FirstOrDefault();

                        }
                        _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vStepDis && x.mfFlowNo == "7").Select(x => x.mfSubject).First();
                        NickNameApprove = empApprove != null ? _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == empApprove).Select(x => x.NICKNAME).First() : @class._ViewceMastInforSpacMoldRequest.irNameApprove;
                    }
                    
                    //vstep = vstep == 9 ? vstep = 0 : vstep;
                    vstep = vstep == 8 ? vstep = 0 : vstep;
                    ViewceMastInforSpacMoldRequest _ceMastInforSpacMoldRequest = new ViewceMastInforSpacMoldRequest();
                    if (savetype == "S")
                    {
                        _ceMastInforSpacMoldRequest = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNoSub == vDocNo).FirstOrDefault();
                        _ceMastInforSpacMoldRequest.irIssueDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                        _ceMastInforSpacMoldRequest.irStep = vstep;
                        _ceMastInforSpacMoldRequest.irStatus = _smStatus;
                        //_ceMastWorkingTimeRequest.wrEmpCodeRequest
                        //_ceMastWorkingTimeRequest.wrNameRequest
                        _ceMastInforSpacMoldRequest.irEmpCodeApprove = empApprove;
                        _ceMastInforSpacMoldRequest.irNameApprove = NickNameApprove;
                        _ceMastInforSpacMoldRequest.irFlowNo = 7;
                        _MK._ViewceMastInforSpacMoldRequest.Update(_ceMastInforSpacMoldRequest);
                        _MK.SaveChanges();
                    }

                    var itemSM = _MK._ViewceItemInforRequestPartName.Where(p => p.ipDocumentNoSub == vDocNo).ToList();
                    if (itemSM.Count > 0)
                    {
                        _MK._ViewceItemInforRequestPartName.RemoveRange(itemSM);
                        _MK.SaveChanges();
                    }
                    for (int i = 0; i < @class._ListViewceItemInforRequestPartName.Count(); i++)
                    {
                        var _ceItemInforRequestPartName = new ViewceItemInforRequestPartName()
                        {
                            ipDocumentNoSub = @class._ListViewceItemInforRequestPartName[i].ipDocumentNoSub,
                            ipRunNo = @class._ListViewceItemInforRequestPartName[i].ipRunNo,
                            ipPartName = @class._ListViewceItemInforRequestPartName[i].ipPartName,
                            ipCavityNo = @class._ListViewceItemInforRequestPartName[i].ipCavityNo,
                            ipTypeCavity = @class._ListViewceItemInforRequestPartName[i].ipTypeCavity,
                            ipNoProcess = @class._ListViewceItemInforRequestPartName[i].ipNoProcess,
                            ipSprueSystem = @class._ListViewceItemInforRequestPartName[i].ipSprueSystem,
                            ipRunner = @class._ListViewceItemInforRequestPartName[i].ipRunner,
                            ipMakerHotRunner = @class._ListViewceItemInforRequestPartName[i].ipMakerHotRunner,
                            ipGateType1 = @class._ListViewceItemInforRequestPartName[i].ipGateType1,
                            ipNumberPoint1 = @class._ListViewceItemInforRequestPartName[i].ipNumberPoint1,
                            ipGateType2 = @class._ListViewceItemInforRequestPartName[i].ipGateType2,
                            ipNumberPoint2 = @class._ListViewceItemInforRequestPartName[i].ipNumberPoint2,
                            ipGateType3 = @class._ListViewceItemInforRequestPartName[i].ipGateType3,
                            ipNumberPoint3 = @class._ListViewceItemInforRequestPartName[i].ipNumberPoint3,
                            ipBaseCavity = @class._ListViewceItemInforRequestPartName[i].ipBaseCavity,
                            ipInsertCavity = @class._ListViewceItemInforRequestPartName[i].ipInsertCavity,
                            ipBaseCode = @class._ListViewceItemInforRequestPartName[i].ipBaseCode,
                            ipInsertCode = @class._ListViewceItemInforRequestPartName[i].ipInsertCode,
                            ipSlide = @class._ListViewceItemInforRequestPartName[i].ipSlide,
                            ipElectroFormType = @class._ListViewceItemInforRequestPartName[i].ipElectroFormType,
                            ipElectroFormPcs = @class._ListViewceItemInforRequestPartName[i].ipElectroFormPcs,

                        };
                        _MK._ViewceItemInforRequestPartName.AddAsync(_ceItemInforRequestPartName);

                    }


                    var itemSS = _MK._ViewceItemInforSlideSystem.Where(p => p.isDocumentNoSub == vDocNo).ToList();
                    if (itemSS.Count > 0)
                    {
                        _MK._ViewceItemInforSlideSystem.RemoveRange(itemSS);
                        _MK.SaveChanges();
                    }
                    for (int i = 0; i < @class._ListViewceItemInforSlideSystem.Count(); i++)
                    {
                        var ceItemInforSlideSystem = new ViewceItemInforSlideSystem()
                        {
                            isDocumentNoSub = @class._ListViewceItemInforSlideSystem[i].isDocumentNoSub,
                            isRunNo = @class._ListViewceItemInforSlideSystem[i].isRunNo,
                            isPartName = @class._ListViewceItemInforSlideSystem[i].isPartName,
                            isCavityNo = @class._ListViewceItemInforSlideSystem[i].isCavityNo,
                            isTypeCavity = @class._ListViewceItemInforSlideSystem[i].isTypeCavity,
                            isNoProcess = @class._ListViewceItemInforSlideSystem[i].isNoProcess,
                            isSlideSystemType = @class._ListViewceItemInforSlideSystem[i].isSlideSystemType,
                            isSlideSystemCount = @class._ListViewceItemInforSlideSystem[i].isSlideSystemCount,

                        };
                        _MK._ViewceItemInforSlideSystem.AddAsync(ceItemInforSlideSystem);
                    }


                    var itemTC = _MK._ViewceItemInforTypeOfCut.Where(p => p.icDocumentNoSub == vDocNo).ToList();
                    if (itemTC.Any())
                    {
                        _MK._ViewceItemInforTypeOfCut.RemoveRange(itemTC);
                        _MK.SaveChangesAsync(); // ✅ ใช้ async และ commit ก่อนเพิ่มข้อมูลใหม่
                    }
                    // 🔹 เคลียร์ทุก entity ที่ EF จำไว้ (เทียบเท่า ChangeTracker.Clear())
                    //foreach (var entry in _MK.ChangeTracker.Entries().ToList())
                    //{
                    //    entry.State = EntityState.Detached;
                    //}
                    for (int i = 0; i < @class._ListViewceItemInforTypeOfCut.Count(); i++)
                    {
                        var ceItemInforTypeOfCut = new ViewceItemInforTypeOfCut()
                        {
                            icDocumentNoSub = @class._ListViewceItemInforTypeOfCut[i].icDocumentNoSub,
                            icRunNo = @class._ListViewceItemInforTypeOfCut[i].icRunNo,
                            icPartName = @class._ListViewceItemInforTypeOfCut[i].icPartName,
                            icCavityNo = @class._ListViewceItemInforTypeOfCut[i].icCavityNo,
                            icTypeCavity = @class._ListViewceItemInforTypeOfCut[i].icTypeCavity,
                            icNoProcess = @class._ListViewceItemInforTypeOfCut[i].icNoProcess,
                            icTypeofcut = @class._ListViewceItemInforTypeOfCut[i].icTypeofcut,
                        };
                        _MK._ViewceItemInforTypeOfCut.AddAsync(ceItemInforTypeOfCut);
                    }


                    var itemSB = _MK._ViewceItemInforShibo.Where(p => p.ibDocumentNoSub == vDocNo).ToList();
                    if (itemSB.Count > 0)
                    {
                        _MK._ViewceItemInforShibo.RemoveRange(itemSB);
                        _MK.SaveChanges();
                    }
                    for (int i = 0; i < @class._ListViewceItemInforShibo.Count(); i++)
                    {
                        var ceItemInforShibo = new ViewceItemInforShibo()
                        {
                            ibDocumentNoSub = @class._ListViewceItemInforShibo[i].ibDocumentNoSub,
                            ibRunNo = @class._ListViewceItemInforShibo[i].ibRunNo,
                            ibPartName = @class._ListViewceItemInforShibo[i].ibPartName,
                            ibCavityNo = @class._ListViewceItemInforShibo[i].ibCavityNo,
                            ibTypeCavity = @class._ListViewceItemInforShibo[i].ibTypeCavity,
                            ibNoProcess = @class._ListViewceItemInforShibo[i].ibNoProcess,
                            ibShiboType = @class._ListViewceItemInforShibo[i].ibShiboType,
                            ibSHiboPCS = @class._ListViewceItemInforShibo[i].ibSHiboPCS,
                        };
                        _MK._ViewceItemInforShibo.AddAsync(ceItemInforShibo);
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


        public string[] SendMailHistory(Class @class, int vstep, string RunDoc)
        {
            string v_msg = "";
            string v_status = "";
            string vCCemail = "";
            try
            {
                RunDoc = @class._ViewceMastInforSpacMoldRequest.irDocumentNo;

                string Empcode_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
                int vRevision = @class._ViewceMastMoldOtherRequest.mrRevision;
                string Name_IssueBy = User.Claims.FirstOrDefault(s => s.Type == "NameE")?.Value; // HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NICKNAME)?.Value;
                string v_EmpCodeRequest = @class._ViewceMastInforSpacMoldRequest.irEmpCodeRequest == null || @class._ViewceMastInforSpacMoldRequest.irEmpCodeRequest == ""
                                                                                    ? Empcode_IssueBy + " : " + _HRMS.AccEMPLOYEE.Where(x => x.EMP_CODE == Empcode_IssueBy).Select(x => x.NICKNAME).First()
                                                                                    : @class._ViewceMastInforSpacMoldRequest.irEmpCodeRequest + " : " + @class._ViewceMastInforSpacMoldRequest.irNameRequest;



                string _smStatus = _MK._ViewceMastFlowApprove.Where(x => x.mfStep == vstep && x.mfFlowNo == "7").Select(x => x.mfSubject).First();

                //checked dis approve   20/10/2025
               
                if (vstep == 8)
                {
                    if (@class._ViewceMastInforSpacMoldRequest.irStep == 1)
                    {
                        //wr
                        string Docsubwr = _MK._ViewceMastWorkingTimeRequest.Where(x => x.wrDocumentNo == RunDoc).Select(x => x.wrDocumentNoSub).FirstOrDefault();
                        string ccwr = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == Docsubwr && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        //mat
                        string DocsubMat = _MK._ViewceMastMaterialRequest.Where(x => x.mrDocumentNo == RunDoc).Select(x => x.mrDocumentNoSub).FirstOrDefault();
                        string ccMat = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubMat && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        ////Tool
                        string DocsubTool = _MK._ViewceMastToolGRRequest.Where(x => x.trDocumentNo == RunDoc).Select(x => x.trDocumentNoSub).FirstOrDefault();
                        string ccTool = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubTool && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();
                        //SM
                        //string DocsubSM = _MK._ViewceMastInforSpacMoldRequest.Where(x => x.irDocumentNo == RunDoc).Select(x => x.irDocumentNoSub).FirstOrDefault();
                        //string ccSM = _MK._ViewceHistoryApproved.Where(x => x.htDocNo == DocsubSM && x.htStep == 1).Select(x => x.htTo).FirstOrDefault();

                        @class._ViewceHistoryApproved.htCC = @class._ViewceHistoryApproved.htCC + "," + Docsubwr + "," + DocsubMat + "," + ccTool;

                    }
                }

                
                vstep = vstep == 8 ? vstep = 0 : vstep;

                var email = new MimeMessage();
                ViewrpEmail fromEmailFrom = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htFrom).FirstOrDefault();
                ViewrpEmail fromEmailTO = _IT.rpEmails.Where(w => w.emName_M365 == @class._ViewceHistoryApproved.htTo).FirstOrDefault();

                MailboxAddress FromMailFrom = new MailboxAddress(fromEmailFrom.emName_M365, fromEmailFrom.emEmail_M365);
                MailboxAddress FromMailTO = new MailboxAddress(fromEmailTO.emName_M365, fromEmailTO.emEmail_M365);
                email.Subject = "Cost Estimate : Mold Other ==> Information Spec Mold   " + _smStatus; /*( " + _ViewlrBuiltDrawing.bdDocumentType + " ) " + _ViewlrHistoryApprove.htStatus*/;
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
                var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther&subType=I";
                //var varifyUrl = "http://thsweb/MVCPublish/CostEstimate/Login/index?DocumentNo=" + RunDoc + "&DocType=MoldOther";// + getSrNo[0].ToString();
                var bodyBuilder = new BodyBuilder();
                //var image = bodyBuilder.LinkedResources.Add(@"E:\01_My Document\02_Project\_2023\1. PartTransferUnbalance\PartTransferUnbalance\wwwroot\images\btn\OK.png");
                string vIssue = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                string vIssueName = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;
                string EmailBody = $"<div>" +
                    $"<B>Cost Estimate : Mold Other ==> Information Spec Mold </B> <br>" +
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

    }
}
